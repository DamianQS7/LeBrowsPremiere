using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Models;
using LeBrowsPremiere.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace LeBrowsPremiere.Managers
{
	public class ProductManager
	{
		private AppDbContext _dbContext;

		public ProductManager(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// Retrieves a collection of all Product objects from the database, including associated Category and Supplier information.
		/// </summary>
		/// <returns>A collection of all Product objects in the database.</returns>
		public async Task<IEnumerable<Product>> GetAllProductsAsync()
		{
			return await _dbContext.Products.Include(p => p.Category).Include(p => p.Supplier).ToListAsync();
		}

		/// <summary>
		/// Retrieves a Product object from the database by its ID, including associated Category and Supplier information.
		/// </summary>
		/// <param name="productId">The ID of the product to retrieve.</param>
		/// <returns>A Product object with the specified ID, including associated Category and Supplier information, 
		/// or null if no product was found with the specified ID.</returns>
		public async Task<Product> GetProductById(int? productId)
		{
			return await _dbContext.Products.Include(c => c.Supplier).
				Include(c => c.Category).FirstOrDefaultAsync(p => p.Id == productId);
		}

		/// <summary>
		/// Adds a Product object to the database.
		/// </summary>
		/// <param name="product">The Product object to add to the database.</param>
		/// <returns>A Task representing the asynchronous add operation.</returns>
		public async Task AddProductToDb(Product product)
		{
			await _dbContext.Products.AddAsync(product);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Updates a Product object in the database.
		/// </summary>
		/// <param name="product">The Product object to update in the database.</param>
		/// <returns>A Task representing the asynchronous update operation.</returns>
		public async Task UpdateProductInDb(Product product)
		{
			_dbContext.Products.Update(product);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Removes a Product object from the database.
		/// </summary>
		/// <param name="product">The Product object to remove from the database.</param>
		/// <returns>A Task representing the asynchronous remove operation.</returns>
		public async Task RemoveProductFromDb(Product product)
		{
			_dbContext.Products.Remove(product);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Creates and returns a new ProductViewModel object with an empty Product object and lists of available 
		/// categories and suppliers for use in a create product view.
		/// </summary>
		/// <returns>A new ProductViewModel object with an empty Product object and lists of available categories 
		/// and suppliers.</returns>
		public ProductViewModel CreateNewProductViewModel()
		{
			ProductViewModel viewModel = new()
			{
				Product = new Product(),
				CategoriesList =  _dbContext.Categories.Select(
				c => new SelectListItem
				{
					Text = c.Name,
					Value = c.CategoryId.ToString()
				}),
				SuppliersList = _dbContext.Suppliers.Select(
				c => new SelectListItem
				{
					Text = c.CompanyName,
					Value = c.SupplierId.ToString()
				})
			};

			return viewModel;
		}

		/// <summary>
		/// Updates the CategoriesList and SuppliersList properties of a ProductViewModel object with the latest 
		/// available categories and suppliers from the database.
		/// </summary>
		/// <param name="viewModel">The ProductViewModel object to update with the latest available categories 
		/// and suppliers.</param>
		public void RepopulateProductViewModel(ProductViewModel viewModel)
		{
			viewModel.CategoriesList = _dbContext.Categories.Select(
				c => new SelectListItem
				{
					Text = c.Name,
					Value = c.CategoryId.ToString()
				});

			viewModel.SuppliersList = _dbContext.Suppliers.Select(
				c => new SelectListItem
				{
					Text = c.CompanyName,
					Value = c.SupplierId.ToString()
				});
		}

		/// <summary>
		/// Manages the product image by deleting the previous image file (if one exists), creating a new image 
		/// file in the appropriate directory with the specified file name and extension, and updating the 
		/// ProductViewModel object with the new image URL.
		/// </summary>
		/// <param name="wwwRootFolderPath">The path to the wwwroot folder in the project.</param>
		/// <param name="viewModel">The ProductViewModel object to update with the new image URL.</param>
		/// <param name="file">The new image file to create.</param>
		/// <param name="productImage">The ProductImage object containing the file name and extension info for the new image file.</param>
		/// <param name="fileService">The IFileService implementation to use for file operations.</param>
		public void ManageProductImageUrl(
			string wwwRootFolderPath, 
			ProductViewModel viewModel, 
			IFormFile? file, 
			ProductImage productImage,
			IFileService fileService)
		{
			// Delete the image if previously existed, for when we update.
			if (viewModel.Product.ImageUrl != null)
                ProductManager.DeleteImageFromProject(wwwRootFolderPath, viewModel.Product, fileService);

            fileService.CreateFile(productImage.FilePath, file);

            viewModel.Product.ImageUrl = @"\img\products\" + productImage.FileName + productImage.Extension;
		}

		/// <summary>
		/// Deletes a product image file from the project.
		/// </summary>
		/// <param name="webRootPath">The path to the web root folder in the project.</param>
		/// <param name="product">The Product object containing the URL of the image file to delete.</param>
		/// <param name="fileService">The IFileService implementation to use for file operations.</param>
		public static void DeleteImageFromProject(string webRootPath, Product product, IFileService fileService)
		{
            var oldImgPath = Path.Combine(webRootPath, product.ImageUrl.TrimStart('\\'));
            fileService.DeleteFile(oldImgPath);
        }
	}
}
