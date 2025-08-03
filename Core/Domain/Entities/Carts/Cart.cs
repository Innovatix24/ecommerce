
namespace Domain.Entities.Carts;

public class Cart : Entity<int>
{
    public short UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<CartItem> Items { get; set; }
}

public class CartItem : Entity<int>
{
    public short ProductId { get; set; }
    public string ProductName { get; set; }
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
