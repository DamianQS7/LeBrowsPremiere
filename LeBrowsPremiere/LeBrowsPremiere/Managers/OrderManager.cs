using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using Stripe.Checkout;

namespace LeBrowsPremiere.Managers
{
	public class OrderManager
	{
		private AppDbContext _dbContext;

		public OrderManager(AppDbContext appDbContext)
		{
			_dbContext = appDbContext;
		}

		/// <summary>
		/// Retrieves an OrderSummaryViewModel for a specific user, which contains their shopping cart information,
		/// user information, and a list of provinces. The method calculates the cart total using the OrderManager class.
		/// </summary>
		/// <param name="userId"> The ID of the user to retrieve the OrderSummaryViewModel for. </param>
		/// <returns>An OrderSummaryViewModel object containing the user's shopping cart, user info, and a list of provinces</returns>
		public async Task<OrderSummaryViewModel> GetOrderSummaryViewModelByUserId(string userId)
		{
			OrderSummaryViewModel viewModel = new OrderSummaryViewModel()
			{
				CustomerCarts = await _dbContext.ShoppingCarts.Include(c => c.Product).Where(c => c.UserId == userId).ToListAsync(),
				User = _dbContext.Users.Include(p => p.Province).FirstOrDefault(u => u.Id == userId),
				NewOrder = new Order(),
				ProvincesList = await _dbContext.Provinces.Select(
				c => new SelectListItem
				{
					Text = c.ProvinceName,
					Value = c.ProvinceId.ToString()
				}).ToListAsync()
			};

			viewModel.CartTotal = OrderManager.GetCartTotal(viewModel.CustomerCarts);

			return viewModel;
		}

		/// <summary>
		/// Retrieves a User object by their user ID, including the associated Province object.
		/// </summary>
		/// <param name="userId"> The ID of the user to retrieve.</param>
		/// <returns>A User object including their associated Province object, 
		/// or null if no user was found with the specified ID.</returns>
		public async Task<User> GetUserByUserIdAsync(string userId)
		{
			return await _dbContext.Users.Include(p => p.Province).FirstOrDefaultAsync(u => u.Id == userId);
		}

		/// <summary>
		/// Updates the shipping address info of a User object with the values from an OrderSummaryViewModel.
		/// </summary>
		/// <param name="viewModel">The OrderSummaryViewModel object containing the updated shipping address info</param>
		/// <param name="user"> The User object to update with the new shipping address information. </param>
		/// <returns> A Task representing the asynchronous save operation. </returns>
		public async Task UpdateUserShippingAddress(OrderSummaryViewModel viewModel, User user)
		{
			user.Address = viewModel.Address;
			user.PhoneNumber = viewModel.PhoneNumber;
			user.City = viewModel.City;
			user.PostalCode = viewModel.PostalCode;
			user.ProvinceId = viewModel.ProvinceId;

			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Maintains the state of an OrderSummaryViewModel by repopulating its province dropdown list,
		/// creating a new Order object, and retrieving the user's shopping cart information.
		/// </summary>
		/// <param name="userId"> The ID of the user to retrieve shopping cart information for. </param>
		/// <param name="viewModel"> The OrderSummaryViewModel object to maintain state for. </param>
		public void MaintainViewModelState(string userId, OrderSummaryViewModel viewModel)
		{
			// Repopulate the dropdown for the provinces
			viewModel.ProvincesList = _dbContext.Provinces.Select(
			c => new SelectListItem
			{
				Text = c.ProvinceName,
				Value = c.ProvinceId.ToString()
			});

			viewModel.NewOrder = new Order() 
			{ 
				UserId = userId,
				Status = Enumerables.OrderStatus.PaymentPending,
				OrderDate = DateTime.Now
			};

			viewModel.CustomerCarts = _dbContext.ShoppingCarts.Include(c => c.Product).Where(c => c.UserId == userId);
		}

		/// <summary>
		/// Completes the customer's order by creating an Order object and associated OrderItem objects
		/// based on the items in the user's shopping cart, and saves the order to the database.
		/// </summary>
		/// <param name="viewModel"> The OrderSummaryViewModel object containing the user's shopping cart 
		/// and new order information.</param>
		/// <returns> A Task representing the asynchronous save operation. </returns>
		public async Task CompleteCustomerOrder(OrderSummaryViewModel viewModel)
		{
			ICollection<OrderItem> items = new List<OrderItem>();

			foreach (var cart in viewModel.CustomerCarts)
			{
				OrderItem orderItem = new OrderItem()
				{
					Quantity = cart.Count,
					ProductId = cart.ProductId,
					Price = cart.Product.Price * cart.Count,
					OrderId = viewModel.NewOrder.Id
				};

				items.Add(orderItem);
				viewModel.NewOrder.OrderTotal += cart.Product.Price * cart.Count;
			}

			viewModel.NewOrder.Items = items;

			await _dbContext.Orders.AddAsync(viewModel.NewOrder);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Updates an Order object with the session ID and payment intent ID from a Stripe Session object,
		/// and saves the changes to the database.
		/// </summary>
		/// <param name="session"> The Stripe Session object containing the session ID and payment intent ID 
		/// to update the order with.</param>
		/// <param name="order">The Order object to update.</param>
		/// <returns> A Task representing the asynchronous save operation.</returns>
		public async Task UpdateOrderWithStripeSessionInfo(Session session, Order order)
		{
			order.SessionId = session.Id;
			order.PaymentIntentId = session.PaymentIntentId;
			_dbContext.Orders.Update(order);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Retrieves an Order object by its ID.
		/// </summary>
		/// <param name="orderId">The ID of the order to retrieve.</param>
		/// <returns>An Order object with the specified ID, or null if no order was found with the specified ID.</returns>
		public async Task<Order> GetOrderByOrderId(int orderId)
		{
			return await _dbContext.Orders.FindAsync(orderId);
		}

		/// <summary>
		/// Calculates the total price of a collection of ShoppingCart objects.
		/// </summary>
		/// <param name="carts">The collection of ShoppingCart objects to calculate the total price for.</param>
		/// <returns>The total price of the shopping carts.</returns>
		public static double GetCartTotal(IEnumerable<ShoppingCart> carts)
		{
			double total = 0;

			foreach (ShoppingCart cart in carts)
			{
				total += cart.Price;
			}

			return total;
		}
	}
}
