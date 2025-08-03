
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts;

public class RemoveCartItemCommand : IRequest<Result>
{
    public int CartId { get; set; }
    public short ProductId { get; set; }
}

public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public RemoveCartItemCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.CartItems
            .FirstOrDefaultAsync(i => i.Id == request.CartId && i.ProductId == request.ProductId, cancellationToken);

        if (item is null)
            return Result.Failure("Item not found");

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
