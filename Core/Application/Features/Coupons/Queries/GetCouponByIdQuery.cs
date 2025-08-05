
using Application.Features.Orders.Queries;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Coupons.Queries;

public record GetCouponByIdQuery(short Id) : IRequest<Result<CouponDto>>;
public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, Result<CouponDto>>
{
    private readonly ApplicationDbContext _context;

    public GetCouponByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CouponDto>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Coupons.FirstOrDefaultAsync(cancellationToken);
            if (entity is null) return Result<CouponDto>.Failure("Failed to fetch data.");
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
            return Result<CouponDto>.Success(coupon);
        }
        catch (Exception ex)
        {
            return Result<CouponDto>.Failure("Failed to fetch data.");
        }
    }
}
