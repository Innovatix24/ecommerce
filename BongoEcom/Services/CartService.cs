using Application.Features.Attributes.DTOs;
using Application.Features.Products.DTOs;

namespace BongoEcom.Services;

public class CartItemModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
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

public class CartService(SweetAlertService SAlert)
{
    private List<CartItemModel> items = new();
    private SweetAlertService alertService = SAlert;

    public IReadOnlyList<CartItemModel> Items => items;

    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public async Task<bool> AddProduct(ProductDto product, List<ItemAttribute>? attributes = null, int qty = 1)
    {
        var existingItem = items.FirstOrDefault(i => i.Id == product.Id);

        if (existingItem != null)
        {
            await alertService.ShowErrorAsync("Already added");
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
        }
        await alertService.ShowSuccessAsync("Product is added to cart");
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

    public void RemoveItem(int productId)
    {
        items.RemoveAll(i => i.Id == productId);
        NotifyStateChanged();
    }

    public void ClearCart()
    {
        items.Clear();
        NotifyStateChanged();
    }

    public decimal GetTotalPrice() => items.Sum(i => i.Total);
}
