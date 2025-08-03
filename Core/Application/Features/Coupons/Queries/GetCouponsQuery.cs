using Application.Features.Orders.Queries;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Coupons.Queries;

public record GetCouponsQuery : IRequest<Result<List<CouponDto>>>;
public class GetCouponsQueryHandler : IRequestHandler<GetCouponsQuery, Result<List<CouponDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetCouponsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CouponDto>>> Handle(GetCouponsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _context.Coupons.ToListAsync(cancellationToken);

            var coupons = new List<CouponDto>();

            foreach (var entity in entities) 
            {
                var coupon = new CouponDto
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    DiscountType = (DiscountType)entity.DiscountType,
                    ExpiryDate = entity.ExpiryDate,
                    IsActive = entity.IsActive,
                    MinimumPurchaseAmount = entity.MinimumPurchaseAmount,
                    DiscountValue = entity.DiscountValue,
                    UsageCount = entity.UsageCount,
                    MaxUsageCount = entity.MaxUsageCount,
                };
                coupons.Add(coupon);
            }

            return Result<List<CouponDto>>.Success(coupons);
        }
        catch (Exception ex)
        {
            return Result<List<CouponDto>>.Failure("Failed to fetch data.");
        }
    }
}
