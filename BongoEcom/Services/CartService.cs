using Application.Features.Attributes.DTOs;
using Application.Features.Inventories;
using Application.Features.Products.DTOs;
using BongoEcom.Services.Contracts;

namespace BongoEcom.Services;

public class ShoppingCart
{
    public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();
    public int TotalItems => Items.Sum(item => item.Qty);
    public decimal TotalPrice => Items.Sum(item => item.Rate * item.Qty);
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}


public class CartItemModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int SKUId { get; set; }
    public int ImageId { get; set; }
    public int Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal Total => Qty * Rate;

    //public Dictionary<string, ProductAttributeValueDto> Attributes = new();
    public List<ItemAttribute> Attributes { get; set; } = new();
    public string AttributeStr { 
        get 
        {
            var str = "";
            foreach (var item in Attributes)
            {
                str += "<b>" + item.Key + "</b> : ";
                str += item.Value + ", ";
            }
            str = str.Trim();
            str = str.Trim(',');
            return str;
        } 
    }
}

public class CartService(SweetAlertService SAlert, UIHelperService UIService, ICartService cartService)
{
    private List<CartItemModel> items = new();
    private SweetAlertService alertService = SAlert;

    public IReadOnlyList<CartItemModel> Items => items;

    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public async Task LoadShoppingCartAsync()
    {
        var cart = await cartService.GetCartAsync();
        items = cart.Items;
        NotifyStateChanged();
    }

    public async Task<bool> AddProduct(ProductDto product, List<ItemAttribute>? attributes = null, int qty = 1)
    {
        var existingItem = items.FirstOrDefault(i => i.Id == product.Id);

        if (existingItem != null)
        {
            UIService.ErrorToastMessage("Already added");
            //await alertService.ShowErrorAsync("Already added");
            return false;
        }
        else
        {
            int imageId = 0;
            var firstImage = product.Images.FirstOrDefault();
            if (firstImage is not null) imageId = firstImage.Id;

            var item = new CartItemModel
            {
                Id = product.Id,
                Name = product.Name,
                Rate = product.SalePrice,
                Qty = qty,
                ImageUrl = product.FeatureImageUrl,
                ImageId = imageId,
                Attributes = attributes ?? new(),
            };
            items.Add(item);
            await cartService.AddItemAsync(item);
        }
        UIService.SuccessToastMessage("Product is added to cart");
        NotifyStateChanged();
        return true;
    }

    public async Task<bool> AddProduct(ProductDto product, SKUDto sku, List<ItemAttribute>? attributes = null, int qty = 1)
    {
        var existingItem = items.FirstOrDefault(i => i.Id == product.Id);

        if (existingItem != null)
        {
            UIService.ErrorToastMessage("Already added");
            //await alertService.ShowErrorAsync("Already added");
            return false;
        }
        else
        {
            int imageId = 0;
            var firstImage = product.Images.FirstOrDefault();
            if (firstImage is not null) imageId = firstImage.Id;

            var item = new CartItemModel
            {
                Id = product.Id,
                Name = product.Name,
                Rate = sku.DiscountPrice ?? sku.Price,
                Qty = qty,
                ImageUrl = product.FeatureImageUrl,
                SKUId = sku.Id,
                ImageId = imageId,
                Attributes = attributes ?? new(),
            };
            items.Add(item);
            await cartService.AddItemAsync(item);
        }
        UIService.SuccessToastMessage("Product is added to cart");
        NotifyStateChanged();
        return true;
    }


    public void AddItem(CartItemModel newItem)
    {
        var existingItem = items.FirstOrDefault(i => i.Id == newItem.Id);
        if (existingItem != null)
        {
            existingItem.Qty += newItem.Qty;
        }
        else
        {
            items.Add(newItem);
        }

        NotifyStateChanged();
    }

    public async void RemoveItem(int productId)
    {
        items.RemoveAll(i => i.Id == productId);
        await cartService.RemoveItemAsync(productId);
        NotifyStateChanged();
    }

    public async void ClearCart()
    {
        items.Clear();
        await cartService.ClearCartAsync();
        NotifyStateChanged();
    }

    public decimal GetTotalPrice() => items.Sum(i => i.Total);
}
