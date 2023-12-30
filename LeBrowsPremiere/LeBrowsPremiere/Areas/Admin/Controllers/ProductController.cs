using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using LeBrowsPremiere.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LeBrowsPremiere.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private ProductManager _productManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        public string WebRootPath { get; set; }
        public string ImagesFolderPath { get; set; }

        public ProductController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _webHostEnvironment = webHostEnvironment;
            _productManager = new ProductManager(dbContext);
            _fileService= fileService;
            WebRootPath = _webHostEnvironment.WebRootPath;
            ImagesFolderPath = Path.Combine(WebRootPath, @"img\products");
        }

		/// <summary>
		/// Retrieves all products from the database and displays them on a view.
		/// </summary>
		/// <returns>The view containing all products.</returns>
		/// <remarks>Uses a GET request to fetch the data.</remarks>
		[HttpGet]
        public async Task<IActionResult> DisplayProducts()
        {
            IEnumerable<Product> products = await _productManager.GetAllProductsAsync();
            return View(products);
        }

		#region IventoryManagement

		/// <summary>
		/// Displays the inventory management page.
		/// </summary>
		/// <returns>The view for inventory management.</returns>
		/// <remarks>Uses a GET request to retrieve the view.</remarks>
		[HttpGet]
        public IActionResult Inventory()
        {
            return View();
        }

		/// <summary>
		/// Displays a form for creating or editing a product.
		/// </summary>
		/// <param name="id">The ID of the product to be edited (optional).</param>
		/// <returns>The view for creating or editing a product.</returns>
		/// <remarks>
		/// If an ID is provided, retrieves the product with that ID from the database and
		/// includes it in the view model for editing. Otherwise, returns a view with a new product.
		/// Uses a GET request to retrieve the view.
		/// </remarks>
		[HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductViewModel viewModel = _productManager.CreateNewProductViewModel();

            if (id == null || id == 0)
				return View(viewModel); // Returns ViewModel with new Product
			else
            {
                // Update ViewModel´s Product
                viewModel.Product = await _productManager.GetProductById(id);
                return View(viewModel);
            }
            
        }

		/// <summary>
		/// This method is called when the user submits the "Upsert" form to create a new product or update an existing one.
		/// If the submitted data is valid, it checks if an image file was uploaded, and manages its storage accordingly.
		/// If the product already exists in the database, it updates its data, otherwise it adds a new product to the database.
		/// </summary>
		/// <param name="viewModel">The ProductViewModel containing the data submitted by the user.</param>
		/// <param name="file">The uploaded image file, if any.</param>
		/// <returns>A redirect to the "Index" action if the operation is successful, otherwise 
        /// it returns the "Upsert" view with the original form data and error messages.</returns>
		[HttpPost]
        public async Task<IActionResult> Upsert(ProductViewModel viewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    ProductImage? productImage = _fileService.GenerateFilePathForImage(WebRootPath,
                    ImagesFolderPath, file);

                    _productManager.ManageProductImageUrl(WebRootPath, viewModel, file, productImage, _fileService);
                }

                if (viewModel.Product.Id == 0)
                {
                    await _productManager.AddProductToDb(viewModel.Product);
                    TempData["success"] = "The product has been successfully created";
                }
                else
                {
                    await _productManager.UpdateProductInDb(viewModel.Product);
                    TempData["success"] = "The product has been successfully updated";
                }

                return RedirectToAction("Inventory");
            }

            //Fix the state of the view model if the state of the Model is invalid
            _productManager.RepopulateProductViewModel(viewModel);
            return View(viewModel);
        }

		/// <summary>
		/// Returns all products in the database in JSON format for use in client-side script
		/// </summary>
		[HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productList = await _productManager.GetAllProductsAsync();
            return Json(new { data = productList });
        }

		/// <summary>
		/// Action method that deletes a product from the database and its image from the project.
		/// </summary>
		/// <param name="id">The id of the product to be deleted</param>
		/// <returns>A Json object with a success message if the product was successfully deleted or an error message if not</returns>
		[HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            Product? product = await _productManager.GetProductById(id);
            
            if (product == null)
				return Json(new { success = false, message = "Error while deleting" });

			// We will delete the image as well from the project.
			ProductManager.DeleteImageFromProject(_webHostEnvironment.WebRootPath, product, _fileService);

            await _productManager.RemoveProductFromDb(product);
            
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
