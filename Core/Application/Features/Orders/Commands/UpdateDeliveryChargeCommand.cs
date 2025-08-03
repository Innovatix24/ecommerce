
namespace Application.Features.Orders.Commands;

public class UpdateDeliveryChargeCommand : IRequest<Result>
{
    public short Id { get; set; }
    public string AreaType { get; set; }
    public decimal ChargeAmount { get; set; }
    public decimal? FreeShippingThreshold { get; set; }
}

public class UpdateDeliveryChargeCommandHandler : IRequestHandler<UpdateDeliveryChargeCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public UpdateDeliveryChargeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateDeliveryChargeCommand request, CancellationToken cancellationToken)
    {
        var deliveryCharge = await _context.DeliveryCharges.FindAsync(request.Id);
        if (deliveryCharge == null)
            return Result.Failure("Delivery charge not found.");

        deliveryCharge.AreaType = request.AreaType;
        deliveryCharge.ChargeAmount = request.ChargeAmount;
        deliveryCharge.FreeShippingThreshold = request.FreeShippingThreshold;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
