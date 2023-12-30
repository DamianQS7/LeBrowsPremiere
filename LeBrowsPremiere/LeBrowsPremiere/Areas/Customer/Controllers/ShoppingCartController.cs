using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Security.Claims;

namespace LeBrowsPremiere.Areas.Customer.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ShoppingCartManager _cartManager;

        public ShoppingCartController(AppDbContext dbContext)
        {
            _cartManager = new ShoppingCartManager(dbContext);
        }

		/// <summary>
		/// Displays the shopping cart for the current user.
		/// </summary>
		/// <returns>The shopping cart view for the current user.</returns>
		public async Task<IActionResult> Index()
        {
            string currentUserId = UserManager.GetUserId(this.User);

            ShoppingCartViewModel viewModel = await _cartManager.GetShoppingCartViewModelByUserId(currentUserId);

            return View(viewModel);
        }

		/// <summary>
		/// GET method to display details of a specific product and create a new shopping cart item for that product.
		/// </summary>
		/// <param name="productId">ID of the selected product to display details of.</param>
		/// <returns>A view with details of the selected product and a new shopping cart item for that product.</returns>
		[HttpGet]
		public async Task<IActionResult> Details(int productId)
		{
			return View(await _cartManager.CreateShoppingCartWithSelectedProduct(productId));
		}

		/// <summary>
		/// HTTP Post action method for adding/updating a product to a user's shopping cart.
		/// </summary>
		/// <param name="shoppingCart">The shopping cart model that contains the product to be added/updated.</param>
		/// <returns>Redirects to the DisplayProducts action method of the Product controller in the Admin area.</returns>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Details(ShoppingCart shoppingCart)
		{
			string userId = UserManager.GetUserId(this.User);
			shoppingCart.UserId = userId;

			ShoppingCart cartFromDb = await _cartManager.GetShoppingCartByUserAndProductId(userId, shoppingCart.ProductId);

			if (cartFromDb == null)
			{
				shoppingCart.Count = 1;
                await _cartManager.AddShoppingCartToDb(shoppingCart);
				TempData["success"] = "Product added to the Shopping Cart";
			}
			else
			{
				_cartManager.UpdateShoppingCartStatus(cartFromDb, "Increase");
                await _cartManager.UpdateShoppingCartInDb(cartFromDb);
				TempData["success"] = "Shopping Cart updated";
			}

            
			return RedirectToAction("DisplayProducts", "Product", new { area = "Admin"});
		}

		/// <summary>
		/// Increases the count of the shopping cart item with the given cart ID by 1 
		/// and updates it in the database.
		/// </summary>
		/// <param name="cartId">The ID of the shopping cart item to update.</param>
		/// <returns>The Index view with the updated shopping cart items.</returns>
		public async Task<IActionResult> Increase(int cartId)
        {
            ShoppingCart? cart = await _cartManager.GetShoppingCartByCartId(cartId);

			_cartManager.UpdateShoppingCartStatus(cart, "Increase");

			await _cartManager.UpdateShoppingCartInDb(cart);

			return RedirectToAction("Index", cartId);
		}

		/// <summary>
		/// Decreases the count of a shopping cart by 1. If the count becomes 0, the shopping cart is removed from the database.
		/// </summary>
		/// <param name="cartId">The ID of the shopping cart to decrease the count of</param>
		/// <returns>A redirect to the shopping cart index page</returns>
		public async Task<IActionResult> Decrease(int cartId)
        {
			ShoppingCart? cart = await _cartManager.GetShoppingCartByCartId(cartId);

			if (cart.Count <= 1)
				await _cartManager.RemoveShoppingCartFromDb(cart);
			else
            {
				_cartManager.UpdateShoppingCartStatus(cart, "Decrease");
				await _cartManager.UpdateShoppingCartInDb(cart);
            }

            return RedirectToAction("Index");
        }

		/// <summary>
		/// Removes a product from the shopping cart and updates the database.
		/// </summary>
		/// <param name="cartId">The id of the shopping cart to remove the product from.</param>
		/// <returns>A redirection to the shopping cart index page with a success message in TempData.</returns>
		public async Task<IActionResult> Remove(int cartId)
        {
			ShoppingCart? cart = await _cartManager.GetShoppingCartByCartId(cartId);

			await _cartManager.RemoveShoppingCartFromDb(cart);

			TempData["success"] = "Product removed successfully from your cart";
			return RedirectToAction("Index");
		}
    }
}
