

using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands;

public class CreateDeliveryChargeCommand : IRequest<Result<byte>>
{
    public byte Id { get; set; }
    public string AreaType { get; set; } = string.Empty;
    public decimal ChargeAmount { get; set; }
    public decimal? FreeShippingThreshold { get; set; }
}

public class CreateDeliveryChargeCommandHandler : IRequestHandler<CreateDeliveryChargeCommand, Result<byte>>
{
    private readonly ApplicationDbContext _context;

    public CreateDeliveryChargeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<byte>> Handle(CreateDeliveryChargeCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if(request.Id == 0)
            {
                var exists = _context.DeliveryCharges.Any(x => x.AreaType == request.AreaType);
                if (exists)
                    return Result<byte>.Failure("A delivery charge for this area already exists.");

                var deliveryCharge = new DeliveryCharge
                {
                    AreaType = request.AreaType,
                    ChargeAmount = request.ChargeAmount,
                    FreeShippingThreshold = request.FreeShippingThreshold,
                };

                _context.DeliveryCharges.Add(deliveryCharge);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Result<byte>.Success(deliveryCharge.Id);
            }
            else
            {
                var item = await _context.DeliveryCharges.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (item == null)
                    return Result<byte>.Failure("Update Failed.");

                item.AreaType = request.AreaType;
                item.ChargeAmount = request.ChargeAmount;
                item.FreeShippingThreshold = request.FreeShippingThreshold;

                _context.DeliveryCharges.Update(item);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Result<byte>.Success(item.Id);
            }
        }
        catch (Exception)
        {
            return Result<byte>.Failure("Failed to create delivery charge.");
        }
    }
}

