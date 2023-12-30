using LeBrowsPremiere.Models;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace LeBrowsPremiere.Managers
{
	public static class StripeManager
	{
		/// <summary>
		/// Creates a new SessionCreateOptions object with the specified payment method types, line items,
		/// success and cancel URLs, and automatic tax options.
		/// </summary>
		/// <param name="domain">The root domain URL of the application.</param>
		/// <param name="orderId">The ID of the order to create a session for.</param>
		/// <returns>A new SessionCreateOptions object with the specified options.</returns>
		public static SessionCreateOptions CreateNewSessionOptions(string domain, int orderId)
		{
			SessionCreateOptions options = new SessionCreateOptions()
			{
				PaymentMethodTypes = new List<string>
					{
					"card",
					},
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
				SuccessUrl = domain + $"Customer/Order/OrderConfirmation?id={orderId}",
				CancelUrl = domain + $"Customer/ShoppingCart/Index",
				AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true }
			};

			return options;
		}

		/// <summary>
		/// Adds line items to a SessionCreateOptions object based on the products in the user's shopping cart.
		/// </summary>
		/// <param name="options">The SessionCreateOptions object to add line items to.</param>
		/// <param name="viewModel">The OrderSummaryViewModel object containing the user's shopping cart information.</param>
		public static void AddSessionLineItems(SessionCreateOptions options, OrderSummaryViewModel viewModel)
		{
			foreach (var cart in viewModel.CustomerCarts)
			{

				SessionLineItemOptions sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(cart.Product.Price * 100), //The prices must be in cents
						Currency = "cad",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = cart.Product.Name
						},

					},
					Quantity = cart.Count,
				};

				options.LineItems.Add(sessionLineItem);
			}
		}
	}
}
