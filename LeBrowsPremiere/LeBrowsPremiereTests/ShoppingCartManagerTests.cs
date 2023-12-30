using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using Moq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using LeBrowsPremiere.Extensions;

namespace LeBrowsPremiereTests
{
    /// <summary>
    ///  The methods that are mainly focused on performing CRUD operations using Entity Framework Core were skipped.
    ///  That is because essentially they are a thin wrapper around the database context. 
    ///  Testing these methods would be testing the behavior of Entity Framework Core itself, which is out of the scope of our test plan.
    /// </summary>
	public class ShoppingCartManagerTests
	{
		private readonly ShoppingCartManager _shoppingCartManager;
		private readonly AppDbContext _mockDbContext;

		public ShoppingCartManagerTests()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "ShoppingCartTestDb")
				.Options;
			_mockDbContext = new AppDbContext(options);

			_shoppingCartManager = new ShoppingCartManager(_mockDbContext);
		}

		[Fact]
		public async Task GetShoppingCartViewModelByUserId_CorrectViewModel()
		{
            // Arrange
            _mockDbContext.Users.Add(new User { Id = "FakeUser" });

            _mockDbContext.Products.Add(new Product
            {
                Id = 1,
                Price = 9.99,
                Name = "FakeProduct",
                Brand = "FakeBrand",
                Description = "Fake Description"
            });

            _mockDbContext.ShoppingCarts.Add(new ShoppingCart
            {
                Id = 1,
                UserId = "FakeUser",
                ProductId = 1,
                Count = 2,
                Price = 19.98
            });

			await _mockDbContext.SaveChangesAsync();

			// Act
			ShoppingCartViewModel result = await _shoppingCartManager.GetShoppingCartViewModelByUserId("FakeUser");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.CustomerCarts);
            Assert.Equal(19.98, result.CartTotal);

        }

        [Fact]
        public async Task CreateShoppingCartWithSelectedProduct_ValidShoppingCart()
        {
            // Arrange
            _mockDbContext.Products.Add(new Product
            {
                Id = 2,
                Price = 9.99,
                Name = "FakeProduct",
                Brand = "FakeBrand",
                Description = "Fake Description",
                Category = new Category { CategoryId = 1, Name = "FakeCategory" },
                Supplier = new Supplier { 
                    SupplierId = 1, 
                    CompanyName = "FakeSupplier",
                    ContactEmail = "Fake@email.com",
                    ContactFirstName = "First",
                    ContactLastName = "Last",
                    ContactPhone = "Phone"
                }
            });

            await _mockDbContext.SaveChangesAsync();
            
            // Act
            ShoppingCart result = await _shoppingCartManager.CreateShoppingCartWithSelectedProduct(2);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Product);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(1, result.Count);
            Assert.Equal(9.99, result.Price);
        }

        [Fact]
        public async Task GetShoppingCartByUserAndProductId_ReturnsCorrectShoppingCart()
        {
            // Arrange
            _mockDbContext.Users.Add(new User { Id = "FakeUser2" });

            _mockDbContext.Products.Add(new Product
            {
                Id = 3,
                Price = 9.99,
                Name = "FakeProduct",
                Brand = "FakeBrand",
                Description = "Fake Description"
            });

            _mockDbContext.ShoppingCarts.Add(new ShoppingCart
            {
                Id = 2,
                UserId = "FakeUser2",
                ProductId = 3,
                Count = 2,
                Price = 19.98
            });

            await _mockDbContext.SaveChangesAsync();

            // Act
            ShoppingCart result = await _shoppingCartManager.GetShoppingCartByUserAndProductId("FakeUser2", 3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("FakeUser2", result.UserId);
            Assert.Equal(3, result.ProductId);
            Assert.Equal(2, result.Count);
            Assert.Equal(19.98, result.Price);
        }

        [Fact]
        public async Task GetShoppingCartByCartId_ReturnsCorrectShoppingCart()
        {
            // Arrange
            _mockDbContext.Users.Add(new User { Id = "FakeUser3" });

            _mockDbContext.Products.Add(new Product
            {
                Id = 4,
                Price = 9.99,
                Name = "FakeProduct",
                Brand = "FakeBrand",
                Description = "Fake Description"
            });

            _mockDbContext.ShoppingCarts.Add(new ShoppingCart
            {
                Id = 5,
                UserId = "FakeUser3",
                ProductId = 4,
                Count = 2,
                Price = 19.98
            });

            await _mockDbContext.SaveChangesAsync();

            // Act
            ShoppingCart result = await _shoppingCartManager.GetShoppingCartByCartId(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("FakeUser3", result.UserId);
            Assert.Equal(4, result.ProductId);
            Assert.Equal(2, result.Count);
            Assert.Equal(19.98, result.Price);
        }

        [Fact]
        public void UpdateShoppingCartStatus_IncreasesCountAndPrice()
        {
            // Arrange
            var cart = new ShoppingCart
            {
                Id = 1,
                UserId = "FakeUser",
                ProductId = 1,
                Count = 2,
                Price = 19.98,
                Product = new Product
                {
                    Id = 1,
                    Price = 9.99
                }
            };

            // Act
            _shoppingCartManager.UpdateShoppingCartStatus(cart, "Increase");

            // Assert
            Assert.Equal(3, cart.Count);
            Assert.Equal(29.97, cart.Price);
        }

        [Fact]
        public void UpdateShoppingCartStatus_DecreasesCountAndPrice()
        {
            // Arrange
            var cart = new ShoppingCart
            {
                Id = 1,
                UserId = "FakeUser",
                ProductId = 1,
                Count = 2,
                Price = 19.98,
                Product = new Product
                {
                    Id = 1,
                    Price = 9.99
                }
            };

            // Act
            _shoppingCartManager.UpdateShoppingCartStatus(cart, "Decrease");

            // Assert
            Assert.Equal(1, cart.Count);
            Assert.Equal(9.99, cart.Price);
        }

    }
}
