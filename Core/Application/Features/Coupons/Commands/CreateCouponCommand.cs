

using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Coupons.Commands;

public class CreateCouponCommand : IRequest<Result<short>>
{
    public short Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public byte DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? MinimumPurchaseAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public int UsageCount { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public CreateCouponCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if(request.Id == 0)
            {
                var exists = await _context.Coupons.AnyAsync(x => x.Code == request.Code);
                if (exists)
                    return Result<short>.Failure("A coupon with this code is already exists.");

                var coupon = new Coupon
                {
                    Code = request.Code,
                    DiscountType = request.DiscountType,
                    DiscountValue = request.DiscountValue,
                    MinimumPurchaseAmount = request.MinimumPurchaseAmount,
                    MaxUsageCount = request.MaxUsageCount,
                    UsageCount = request.UsageCount,
                    ExpiryDate = request.ExpiryDate,
                    IsActive = request.IsActive,
                };

                _context.Coupons.Add(coupon);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<short>.Success(coupon.Id);
            }
            else
            {
                var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.Id == request.Id);
                if(coupon != null)
                {
                    coupon.Code = request.Code;
                    coupon.DiscountType = request.DiscountType;
                    coupon.DiscountValue = request.DiscountValue;
                    coupon.MinimumPurchaseAmount = request.MinimumPurchaseAmount;
                    coupon.MaxUsageCount = request.MaxUsageCount;
                    coupon.UsageCount = request.UsageCount;
                    coupon.ExpiryDate = request.ExpiryDate;
                    coupon.IsActive = request.IsActive;

                    _context.Coupons.Update(coupon);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result<short>.Success(coupon.Id);
                }
                else
                {
                    return Result<short>.Failure("Failed to update coupon.");
                }
            }
            
        }
        catch (Exception ex)
        {
            return Result<short>.Failure("Failed to create coupon.");
        }
    }
}
