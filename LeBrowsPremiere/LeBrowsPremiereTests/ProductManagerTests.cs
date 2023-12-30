using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using LeBrowsPremiere.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBrowsPremiereTests
{
    public class ProductManagerTests
    {
        private readonly ProductManager _productManager;
        private readonly AppDbContext _mockDbContext;

        public ProductManagerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductTestDb")
                .Options;
            _mockDbContext = new AppDbContext(options);

            _productManager = new ProductManager(_mockDbContext);
        }

        [Fact]
        public void CreateNewProductViewModel_ReturnsProperViewModel()
        {
            // Arrange
            List<Category> categories = new List<Category>
            {
                new Category { CategoryId = 1, Name = "Category 1" },
                new Category { CategoryId = 2, Name = "Category 2" },
            };

            List<Supplier> suppliers = new List<Supplier>
            {
                new Supplier { SupplierId = 1, CompanyName = "Supplier 1", ContactEmail = "email@email.com",
                            ContactFirstName = "First", ContactLastName = "Last", ContactPhone = "phone"},
                new Supplier { SupplierId = 2, CompanyName = "Supplier 2", ContactEmail = "email2@email.com",
                            ContactFirstName = "First2", ContactLastName = "Last2", ContactPhone = "phone2"}
            };

            _mockDbContext.Categories.AddRange(categories);
            _mockDbContext.Suppliers.AddRange(suppliers);
            _mockDbContext.SaveChanges();

            // Act
            ProductViewModel viewModel = _productManager.CreateNewProductViewModel();
            
            // Assert
            Assert.NotNull(viewModel.Product);
            Assert.NotNull(viewModel.CategoriesList);
            Assert.Equal(2, viewModel.CategoriesList.Count());
            Assert.Equal("Category 1", viewModel.CategoriesList.First().Text);
            Assert.Equal("1", viewModel.CategoriesList.First().Value);

            Assert.NotNull(viewModel.SuppliersList);
            Assert.Equal(2, viewModel.SuppliersList.Count());
            Assert.Equal("Supplier 1", viewModel.SuppliersList.First().Text);
            Assert.Equal("1", viewModel.SuppliersList.First().Value);
        }

        [Fact]
        public void ManageProductImageUrl_ManagesImageUrlCorrectly()
        {
            // Arrange
            var wwwRootFolderPath = "path/to/wwwroot";
            var fileService = new Mock<IFileService>();
            var productImage = new ProductImage
            {
                FileName = "new-image",
                Extension = ".jpg",
                FilePath = Path.Combine(wwwRootFolderPath, "img", "products", "new-image.jpg")
            };

            var fileMock = new Mock<IFormFile>();
            var productViewModel = new ProductViewModel { Product = new Product { ImageUrl = "old-image.jpg" } };
            var productManager = new ProductManager(null);

            // Act
            productManager.ManageProductImageUrl(wwwRootFolderPath, productViewModel, fileMock.Object, productImage, fileService.Object);

            // Assert
            Assert.Equal(@"\img\products\new-image.jpg", productViewModel.Product.ImageUrl);
            fileService.Verify(fs => fs.CreateFile(productImage.FilePath, fileMock.Object), Times.Once);
        }
    }
}

