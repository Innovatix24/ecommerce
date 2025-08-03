

using Domain.Entities.Orders;

namespace Application.Features.Orders.Commands;

public class CreateDeliveryChargeCommand : IRequest<Result<short>>
{
    public string AreaType { get; set; }
    public decimal ChargeAmount { get; set; }
    public decimal? FreeShippingThreshold { get; set; }
}

public class CreateDeliveryChargeCommandHandler : IRequestHandler<CreateDeliveryChargeCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public CreateDeliveryChargeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(CreateDeliveryChargeCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var exists = _context.DeliveryCharges.Any(x => x.AreaType == request.AreaType);
            if (exists)
                return Result<short>.Failure("A delivery charge for this area already exists.");

            var deliveryCharge = new DeliveryCharge
            {
                AreaType = request.AreaType,
                ChargeAmount = request.ChargeAmount,
                FreeShippingThreshold = request.FreeShippingThreshold,
            };

            _context.DeliveryCharges.Add(deliveryCharge);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Result<short>.Success(deliveryCharge.Id);
        }
        catch (Exception)
        {
            return Result<short>.Failure("Failed to create delivery charge.");
        }
    }
}

