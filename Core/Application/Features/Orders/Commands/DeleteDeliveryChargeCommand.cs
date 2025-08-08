

using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands;

public record DeleteDeliveryChargeCommand(short Id) : IRequest<Result>;

public class DeleteDeliveryChargeCommandHandler : IRequestHandler<DeleteDeliveryChargeCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteDeliveryChargeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteDeliveryChargeCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.DeliveryCharges.FirstOrDefaultAsync(x=> x.Id == request.Id);

        if (item == null)
            return Result.Failure("Item not found.");

        _context.DeliveryCharges.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}