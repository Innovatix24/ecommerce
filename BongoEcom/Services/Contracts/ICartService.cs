
using Domain.Entities.Carts;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BongoEcom.Services.Contracts;

public interface ICartService
{
    event Action OnCartUpdated;
    Task<ShoppingCart> GetCartAsync();
    Task AddItemAsync(CartItemModel item);
    Task RemoveItemAsync(int productId);
    Task UpdateQuantityAsync(int productId, int quantity);
    Task ClearCartAsync();
    Task<int> GetCartItemCountAsync();
    void SetUserId(string userId);
}


public class CartService : ICartService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CartService> _logger;
    private readonly NavigationManager _navigationManager;

    public event Action OnCartUpdated;

    private string _userId = "anonymous";

    public CartService(IDistributedCache cache, ILogger<CartService> logger, NavigationManager navigationManager)
    {
        _cache = cache;
        _logger = logger;
        _navigationManager = navigationManager;
    }

    public void SetUserId(string userId)
    {
        _userId = userId ?? "anonymous";
    }

    private string GetCacheKey() => $"cart:{_userId}";

    private DistributedCacheEntryOptions GetCacheOptions()
    {
        return new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(2),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };
    }

    public async Task<ShoppingCart> GetCartAsync()
    {
        try
        {
            var cacheKey = GetCacheKey();
            var cartData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cartData))
            {
                return JsonConvert.DeserializeObject<ShoppingCart>(cartData) ?? new ShoppingCart();
            }

            return new ShoppingCart();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cart from Redis");
            return new ShoppingCart();
        }
    }

    private async Task SaveCartAsync(ShoppingCart cart)
    {
        try
        {
            cart.LastUpdated = DateTime.UtcNow;
            var cacheKey = GetCacheKey();
            var cartData = JsonConvert.SerializeObject(cart);

            await _cache.SetStringAsync(cacheKey, cartData, GetCacheOptions());
            OnCartUpdated?.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving cart to Redis");
        }
    }

    public async Task AddItemAsync(CartItemModel item)
    {
        var cart = await GetCartAsync();
        var existingItem = cart.Items.FirstOrDefault(i => i.Id == item.Id);

        if (existingItem != null)
        {
            existingItem.Qty += item.Qty;
        }
        else
        {
            cart.Items.Add(item);
        }

        await SaveCartAsync(cart);
    }

    public async Task RemoveItemAsync(int productId)
    {
        var cart = await GetCartAsync();
        var item = cart.Items.FirstOrDefault(i => i.Id == productId);

        if (item != null)
        {
            cart.Items.Remove(item);
            await SaveCartAsync(cart);
        }
    }

    public async Task UpdateQuantityAsync(int productId, int quantity)
    {
        var cart = await GetCartAsync();
        var item = cart.Items.FirstOrDefault(i => i.Id == productId);

        if (item != null)
        {
            if (quantity <= 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Qty = quantity;
            }
            await SaveCartAsync(cart);
        }
    }

    public async Task ClearCartAsync()
    {
        var cart = new ShoppingCart();
        await SaveCartAsync(cart);
    }

    public async Task<int> GetCartItemCountAsync()
    {
        var cart = await GetCartAsync();
        return cart.TotalItems;
    }
}