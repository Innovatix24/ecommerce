
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts;

public class UpdateCartItemCommand : IRequest<Result>
{
    public int CartId { get; set; }
    public short ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public UpdateCartItemCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.CartItems
            .FirstOrDefaultAsync(i => i.Id == request.CartId && i.ProductId == request.ProductId, cancellationToken);

        if (item is null)
            return Result.Failure("Item not found");

        item.Quantity = request.Quantity;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

