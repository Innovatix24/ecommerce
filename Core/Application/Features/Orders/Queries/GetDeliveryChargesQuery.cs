
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries;

public class GetDeliveryChargesQuery : IRequest<Result<List<DeliveryChargeDto>>> { }

public class DeliveryChargeDto
{
    public byte Id { get; set; }
    public string AreaType { get; set; }
    public decimal ChargeAmount { get; set; }
    public decimal? FreeShippingThreshold { get; set; }
}

public class GetDeliveryChargesQueryHandler : IRequestHandler<GetDeliveryChargesQuery, Result<List<DeliveryChargeDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetDeliveryChargesQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<DeliveryChargeDto>>> Handle(GetDeliveryChargesQuery request, CancellationToken cancellationToken)
    {
        var results = await _context.DeliveryCharges
            .AsNoTracking()
            .Select(x => new DeliveryChargeDto
            {
                Id = x.Id,
                AreaType = x.AreaType,
                ChargeAmount = x.ChargeAmount,
                FreeShippingThreshold = x.FreeShippingThreshold,
            })
            .ToListAsync(cancellationToken);

        return Result<List<DeliveryChargeDto>>.Success(results);
    }
}
