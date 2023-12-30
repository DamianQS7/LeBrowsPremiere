using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using Microsoft.AspNetCore.Builder;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBrowsPremiereTests
{
	public class StripeManagerTests
	{
		[Fact]
		public void CreateNewSessionOptions_ValidSessionOptionsObject()
		{
			// Arrange
			string domain = "https://localhost:44347/";
			int orderId = 123;

			// Act
			var options = StripeManager.CreateNewSessionOptions(domain, orderId);

			// Assert
			Assert.NotNull(options);
			Assert.Equal("payment", options.Mode);
			Assert.Equal(domain + $"Customer/Order/OrderConfirmation?id={orderId}", options.SuccessUrl);
			Assert.Equal(domain + $"Customer/ShoppingCart/Index", options.CancelUrl);
			Assert.NotNull(options.PaymentMethodTypes);
			Assert.Single(options.PaymentMethodTypes, "card");
			Assert.NotNull(options.LineItems);
		}

		[Fact]
		public void AddSessionLineItems_PopulatesLineItemsProperty()
		{
			// Arrange
			var options = new SessionCreateOptions() { LineItems = new List<SessionLineItemOptions>() };
			var viewModel = new OrderSummaryViewModel
			{
				CustomerCarts = new List<ShoppingCart>
				{
					new ShoppingCart
					{
						Product = new Product
						{
							Name = "Fake Product",
							Brand = "Fake Brand",
							Price = 10.0
						},
						Count = 2
					}
				}
			};

			// Act
			StripeManager.AddSessionLineItems(options, viewModel);
			var lineItem = options.LineItems[0];

			// Assert
			Assert.Single(options.LineItems);
			Assert.Equal("Fake Product", lineItem.PriceData.ProductData.Name);
			Assert.Equal(1000L, lineItem.PriceData.UnitAmount);
			Assert.Equal("cad", lineItem.PriceData.Currency);
			Assert.Equal(2, lineItem.Quantity);
		}
	}
}
