using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LeBrowsPremiere.Managers
{
	public class ShoppingCartManager
	{
		private AppDbContext _dbContext;

		public ShoppingCartManager(AppDbContext appDbContext)
		{
			_dbContext = appDbContext;
		}

		/// <summary>
		/// Retrieves a ShoppingCartViewModel object for a specific user, which contains their shopping cart information
		/// and the total price of the items in their cart.
		/// </summary>
		/// <param name="userId">The ID of the user to retrieve the ShoppingCartViewModel for.</param>
		/// <returns>A ShoppingCartViewModel object containing the user's shopping cart and total cart price.</returns>
		public async Task<ShoppingCartViewModel> GetShoppingCartViewModelByUserId(string userId)
		{
			ShoppingCartViewModel viewModel = new ShoppingCartViewModel()
			{
				CustomerCarts = await _dbContext.ShoppingCarts.Include(c => c.Product).Where(c => c.UserId == userId).ToListAsync(),
				CartTotal = 0
			};

			foreach (var cart in viewModel.CustomerCarts)
			{
				viewModel.CartTotal += cart.Price;
			}

			return viewModel;
		}

		/// <summary>
		/// Creates a new ShoppingCart object for the specified product ID, including the associated product, category,
		/// and supplier information, and initializes the cart count and price based on the product's price.
		/// </summary>
		/// <param name="productId">The ID of the product to create the shopping cart for.</param>
		/// <returns>A new ShoppingCart object for the specified product ID.</returns>
		public async Task<ShoppingCart> CreateShoppingCartWithSelectedProduct(int productId)
		{
			ShoppingCart cart = new()
			{
				Count = 1,
				ProductId = productId,
				Product = await _dbContext.Products.Include(p => p.Category).Include(p => p.Supplier)
							.Where(p => p.Id == productId).FirstOrDefaultAsync()
			};

			cart.Price = cart.Product.Price;

			return cart;
		}

		/// <summary>
		/// Retrieves a ShoppingCart object for a specific user and product ID, including the associated product information.
		/// </summary>
		/// <param name="userId">The ID of the user to retrieve the shopping cart for.</param>
		/// <param name="productId">The ID of the product to retrieve the shopping cart for.</param>
		/// <returns>A ShoppingCart object for the specified user and product ID, or null if no shopping cart was found.</returns>
		public async Task<ShoppingCart> GetShoppingCartByUserAndProductId(string userId, int productId)
		{
			return await _dbContext.ShoppingCarts.Include(c => c.Product).
				FirstOrDefaultAsync(p => p.UserId == userId && p.ProductId == productId);
		}

		/// <summary>
		/// Retrieves a ShoppingCart object by its ID, including the associated product information.
		/// </summary>
		/// <param name="cartId">The ID of the shopping cart to retrieve.</param>
		/// <returns>A ShoppingCart object with the specified ID, including the associated product information, 
		/// or null if no shopping cart was found with the specified ID.</returns>
		public async Task<ShoppingCart> GetShoppingCartByCartId(int cartId)
		{
			return await _dbContext.ShoppingCarts.Include(c => c.Product).
				FirstOrDefaultAsync(u => u.Id == cartId);
		}

		/// <summary>
		/// Updates the status of a ShoppingCart object by increasing or decreasing the cart count and price based on the specified operation type.
		/// </summary>
		/// <param name="cart">The ShoppingCart object to update.</param>
		/// <param name="operationType">The type of operation to perform on the cart ("Increase" or "Decrease").</param>
		public void UpdateShoppingCartStatus(ShoppingCart cart, string operationType)
		{
			if(operationType == "Increase")
			{
				cart.Count++;
				cart.Price = cart.Count* cart.Product.Price;
			}
			else if(operationType == "Decrease")
			{
				cart.Count--;
				cart.Price = cart.Count * cart.Product.Price;
			}
		}

		/// <summary>
		/// Adds a ShoppingCart object to the database.
		/// </summary>
		/// <param name="cart">The ShoppingCart object to add to the database.</param>
		/// <returns>A Task representing the asynchronous add operation.</returns>
		public async Task AddShoppingCartToDb(ShoppingCart cart)
		{
			await _dbContext.ShoppingCarts.AddAsync(cart);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Updates a ShoppingCart object in the database.
		/// </summary>
		/// <param name="cart">The ShoppingCart object to update in the database.</param>
		/// <returns>A Task representing the asynchronous update operation.</returns>
		public async Task UpdateShoppingCartInDb(ShoppingCart cart)
		{
			_dbContext.ShoppingCarts.Update(cart);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Removes a ShoppingCart object from the database.
		/// </summary>
		/// <param name="cart">The ShoppingCart object to remove from the database.</param>
		/// <returns>A Task representing the asynchronous remove operation.</returns>
		public async Task RemoveShoppingCartFromDb(ShoppingCart cart)
		{
			_dbContext.ShoppingCarts.Remove(cart);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Removes all ShoppingCart objects associated with a specific user from the database.
		/// </summary>
		/// <param name="userId">The ID of the user to remove shopping carts for.</param>
		/// <returns>A Task representing the asynchronous remove operation.</returns>
		public async Task RemoveShoppingCartsFromDbByUserId(string userId)
		{
			List<ShoppingCart> carts = _dbContext.ShoppingCarts.Where(u => u.UserId == userId).ToList();
			_dbContext.ShoppingCarts.RemoveRange(carts);
			await _dbContext.SaveChangesAsync();
		}

	}
}
