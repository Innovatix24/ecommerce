
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries;

public class CouponDto
{
    public short Id { get; set; }
    public string Code { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? MinimumPurchaseAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public int UsageCount { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
}

public record GetCouponInfoQuery(string Code) : IRequest<Result<CouponDto>>;
public class GetCouponInfoQueryHandler : IRequestHandler<GetCouponInfoQuery, Result<CouponDto>>
{
    private readonly ApplicationDbContext _context;

    public GetCouponInfoQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CouponDto>> Handle(GetCouponInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                return Result<CouponDto>.Failure("Invalid coupon code");
            }
            var entity = await _context.Coupons
                .Where(x => x.Code == request.Code)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null) 
            {
                return Result<CouponDto>.Failure("This is not a valid coupon");
            }

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
            return Result<CouponDto>.Failure("Failed to fetch coupon.");
        }
    }
}
