using Microsoft.EntityFrameworkCore;
using RetailEdge_DAL.Models;

namespace RetailEdge_DAL;

public class RetailEdgeRepository
{
    private readonly RetailDbContext _context;

    public RetailEdgeRepository(RetailDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Authenticates a user by email and password
    /// </summary>
    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password && u.IsActive == true);

        return user;
    }

    /// <summary>
    /// Retrieves all active products
    /// </summary>
    public async Task<List<Product>> ViewProductsAsync()
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(p => p.IsActive == true)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return products;
    }

    /// <summary>
    /// Adds a product to the user's cart or increments quantity if already exists
    /// Also decrements the product stock
    /// </summary>
    public async Task AddProductToCartAsync(int userId, int productId, int quantity)
    {
        // Validate user exists and is active
        var user = await _context.Users.FindAsync(userId);
        if (user == null || user.IsActive == false)
            throw new InvalidOperationException($"User with ID {userId} not found or is inactive");

        // Validate product exists and is active
        var product = await _context.Products.FindAsync(productId);
        if (product == null || product.IsActive == false)
            throw new InvalidOperationException($"Product with ID {productId} not found or is inactive");

        // Validate sufficient stock
        if (product.Stock < quantity)
            throw new InvalidOperationException($"Insufficient stock. Available: {product.Stock}, Requested: {quantity}");

        // Check if product already exists in user's cart
        var existingCartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

        if (existingCartItem != null)
        {
            // Merge: increment existing quantity
            existingCartItem.Quantity += quantity;
        }
        else
        {
            // Add new cart item
            var cartItem = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                AddedDate = DateTime.UtcNow
            };
            _context.CartItems.Add(cartItem);
        }

        // Decrement product stock
        product.Stock -= quantity;
        product.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all cart items for a user with product details (eager loaded)
    /// </summary>
    public async Task<List<CartItem>> ViewCartItemsAsync(int userId)
    {
        var cartItems = await _context.CartItems
            .AsNoTracking()
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .Include(ci => ci.User)
            .OrderByDescending(ci => ci.AddedDate)
            .ToListAsync();

        return cartItems;
    }

    /// <summary>
    /// Removes an item from the cart and restores the product stock
    /// </summary>
    public async Task RemoveItemFromCartAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

        if (cartItem == null)
            throw new InvalidOperationException($"Cart item with ID {cartItemId} not found");

        // Restore product stock
        var product = cartItem.Product;
        product.Stock += cartItem.Quantity;
        product.UpdatedDate = DateTime.UtcNow;

        // Remove cart item
        _context.CartItems.Remove(cartItem);

        await _context.SaveChangesAsync();
    }
}
