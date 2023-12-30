using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Security.Claims;

namespace LeBrowsPremiere.Areas.Customer.Controllers
{
	public class OrderController : Controller
	{
		private OrderManager _orderManager;
		private ShoppingCartManager _cartManager;

		public OrderController(AppDbContext dbContext)
		{
			_orderManager = new OrderManager(dbContext);
			_cartManager= new ShoppingCartManager(dbContext);

		}

		/// <summary>
		/// Gets the order summary view model for the current user.
		/// </summary>
		/// <returns>The order summary view model</returns>
		[HttpGet]
		public async Task<IActionResult> Summary()
		{
			string userId = UserManager.GetUserId(this.User);

			OrderSummaryViewModel viewModel = await _orderManager.GetOrderSummaryViewModelByUserId(userId);
			
			return View(viewModel);
		}

		/// <summary>
		/// HTTP POST action method to handle the order summary view form submission.
		/// Validates the model state, updates the user's shipping address in the database, completes the order object 
		/// with the order line items, and adds it to the database. Creates a new Stripe payment session 
		/// and associates it with the order, then redirects the user to the session's URL.
		/// </summary>
		/// <param name="viewModel">The order summary view model.</param>
		/// <returns>The view for the order summary or a redirect to the Stripe payment session URL.</returns>
		[HttpPost]
		public async Task<IActionResult> Summary(OrderSummaryViewModel viewModel)
		{
			string userId = UserManager.GetUserId(this.User);

			_orderManager.MaintainViewModelState(userId, viewModel);

			if (viewModel.ProvinceId == 0)
				ModelState.AddModelError("ProvinceId", "Please select a Province");
			
			if(ModelState.IsValid)
			{
				// Update the user's shipping address in the db
				await _orderManager.UpdateUserShippingAddress(viewModel,
					await _orderManager.GetUserByUserIdAsync(userId));

				// Complete the order object with the order line items, and add it to the db
				await _orderManager.CompleteCustomerOrder(viewModel);

				// Stripe settings 
				var domain = "https://localhost:44347/";
				var options = StripeManager.CreateNewSessionOptions(domain, viewModel.NewOrder.Id);

				StripeManager.AddSessionLineItems(options, viewModel);

				SessionService service = new SessionService();
				Session session = service.Create(options);

				await _orderManager.UpdateOrderWithStripeSessionInfo(session, viewModel.NewOrder);

				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);
				
			}
			else
			{
				return View(viewModel);
			}
		}

		/// <summary>
		/// Displays the order confirmation page and updates the order status in the database if the payment 
		/// has been made.
		/// </summary>
		/// <param name="id">The ID of the order to display.</param>
		/// <returns>The order confirmation view.</returns>
		public async Task<IActionResult> OrderConfirmation(int id)
		{
			Order currentOrder = await _orderManager.GetOrderByOrderId(id);

			var service = new SessionService();
			Session session = service.Get(currentOrder.SessionId);

			if (session.PaymentStatus.ToLower() == "paid")
			{
				currentOrder.Status = Enumerables.OrderStatus.Confirmed;
			}

			await _cartManager.RemoveShoppingCartsFromDbByUserId(currentOrder.UserId);
			
			return View(id);
		}
	}
}
