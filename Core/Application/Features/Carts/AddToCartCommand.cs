
using Domain.Entities.Carts;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts;

public class AddToCartCommand : IRequest<Result<int>>
{
    public short UserId { get; set; }
    public short ProductId { get; set; }
    public string ProductName { get; set; }
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public AddToCartCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync(cancellationToken); // To generate ID
        }

        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(i => i.ProductId == request.ProductId && i.Id == cart.Id, cancellationToken);

        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                Id = cart.Id,
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                SKU = request.SKU,
                Price = request.Price,
                Quantity = request.Quantity
            });
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(cart.Id);
    }
}
