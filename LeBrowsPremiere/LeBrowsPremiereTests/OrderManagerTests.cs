using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBrowsPremiereTests
{
	public class OrderManagerTests
	{
        private readonly OrderManager _orderManager;
        private readonly AppDbContext _mockDbContext;

        public OrderManagerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderManagerTestDb")
                .Options;
            _mockDbContext = new AppDbContext(options);

            _orderManager = new OrderManager(_mockDbContext);
        }

        [Fact]
		public async Task GetOrderSummaryViewModelByUserId_ReturnsValidViewModel()
		{
            // Arrange
            _mockDbContext.Users.Add(new User { Id = "FakeUser" });
            _mockDbContext.Provinces.Add(new Province { ProvinceId = 1, ProvinceName = "FakeProvince" });
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
			OrderSummaryViewModel viewModel = await _orderManager.GetOrderSummaryViewModelByUserId("FakeUser");

			// Assert
			Assert.NotNull(viewModel);
			Assert.Equal("FakeUser", viewModel.User.Id);
			Assert.Single(viewModel.CustomerCarts);
			Assert.Equal(19.98, viewModel.CartTotal);
			Assert.Single(viewModel.ProvincesList);
		}

		[Fact]
		public async Task GetUserByUserIdAsync_ReturnsUser()
		{
            // Arrange

            _mockDbContext.Users.Add(new User { Id = "FakeUser2" });
            await _mockDbContext.SaveChangesAsync();

            // Act
            User user = await _orderManager.GetUserByUserIdAsync("FakeUser2");

			// Assert
			Assert.Equal("FakeUser2", user.Id);
		}

		[Fact]
		public async Task UpdateUserShippingAddress_UserUpdated()
		{
			// Arrange
			User user = new User()
			{
				Id = "user1",
				Address = "old address",
				PhoneNumber = "123-456-7890",
				PostalCode = "M3H 2J3",
				City = "old city",
				ProvinceId = 2
			};

			_mockDbContext.Users.Add(user);
			await _mockDbContext.SaveChangesAsync();

			var viewModel = new OrderSummaryViewModel
			{
				Address = "new address",
				PhoneNumber = "555-555-5555",
				City = "city",
				PostalCode = "postal",
				ProvinceId = 1
			};

			// Act
			await _orderManager.UpdateUserShippingAddress(viewModel, user);

            // Assert
			Assert.Equal(viewModel.Address, user.Address);
			Assert.Equal(viewModel.PhoneNumber, user.PhoneNumber);
			Assert.Equal(viewModel.City, user.City);
			Assert.Equal(viewModel.PostalCode, user.PostalCode);
			Assert.Equal(viewModel.ProvinceId, user.ProvinceId);
		}

        [Fact]
        public async Task CompleteCustomerOrder_CreatesOrderWithOrderItems()
        {
            // Arrange
            _mockDbContext.Users.Add(new User { Id = "FakeUser3" });

            _mockDbContext.Products.AddRange(new List<Product>
			{
				new Product { Id = 2, Name = "FakeProduct", Price = 9.99, Brand = "FakeBrand", Description = "FakeDescription" },
				new Product { Id = 3, Name = "FakeProduct2", Price = 19.99, Brand = "FakeBrand", Description = "FakeDescription" },
			});

            _mockDbContext.ShoppingCarts.AddRange(new List<ShoppingCart>
			{
				new ShoppingCart { Id = 3, UserId = "FakeUser3", ProductId = 2, Count = 2, Price = 19.98 },
				new ShoppingCart { Id = 4, UserId = "FakeUser3", ProductId = 3, Count = 1, Price = 19.99 }
			});

            await _mockDbContext.SaveChangesAsync();

            var viewModel = new OrderSummaryViewModel
            {
                CustomerCarts = _mockDbContext.ShoppingCarts.Where(c => c.UserId == "FakeUser3").ToList(),
                NewOrder = new Order { Id = 1, UserId = "FakeUser3", OrderTotal = 0 }
            };

            // Act
            await _orderManager.CompleteCustomerOrder(viewModel);

            // Assert
            Order createdOrder = await _mockDbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == 1);
            Assert.NotNull(createdOrder);
            Assert.Equal(39.97, createdOrder.OrderTotal);
            Assert.Equal(2, createdOrder.Items.Count);

            OrderItem item1 = createdOrder.Items.FirstOrDefault(i => i.ProductId == 2);
            Assert.NotNull(item1);
            Assert.Equal(2, item1.Quantity);
            Assert.Equal(19.98, item1.Price);

            OrderItem item2 = createdOrder.Items.FirstOrDefault(i => i.ProductId == 3);
            Assert.NotNull(item2);
            Assert.Equal(1, item2.Quantity);
            Assert.Equal(19.99, item2.Price);
        }

        [Fact]
        public async Task UpdateOrderWithStripeSessionInfo_UpdatesOrderWithStripeInfo()
        {
            // Arrange
            _mockDbContext.Orders.Add(new Order { Id = 2, UserId = "FakeUser4", OrderTotal = 29.97 });
            await _mockDbContext.SaveChangesAsync();

            var session = new Session
            {
                Id = "stripeSessionId",
                PaymentIntentId = "stripePaymentIntentId"
            };

            var order = await _mockDbContext.Orders.FindAsync(2);

            // Act
            await _orderManager.UpdateOrderWithStripeSessionInfo(session, order);

            // Assert
            Order updatedOrder = await _mockDbContext.Orders.FindAsync(2);
            Assert.NotNull(updatedOrder);
            Assert.Equal("stripeSessionId", updatedOrder.SessionId);
            Assert.Equal("stripePaymentIntentId", updatedOrder.PaymentIntentId);
        }

    }
}

