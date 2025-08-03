

using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts;
public class CartDto
{
    public int CartId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}

public class CartItemDto
{
    public short ProductId { get; set; }
    public string ProductName { get; set; }
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class GetCartQuery : IRequest<CartDto>
{
    public short UserId { get; set; }
}

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDto>
{
    private readonly ApplicationDbContext _context;

    public GetCartQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart == null) return null;

        return new CartDto
        {
            CartId = cart.Id,
            Items = cart.Items.Select(i => new CartItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                SKU = i.SKU,
                Price = i.Price,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}

