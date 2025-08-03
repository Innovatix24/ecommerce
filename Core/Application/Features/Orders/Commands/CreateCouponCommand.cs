
using Domain.Entities.Orders;

namespace Application.Features.Orders.Commands;

public class CreateCouponCommand : IRequest<Result<short>>
{
    public string Code { get; set; }
    public DiscountType DiscountType { get; set; }
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
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var count = _context.Coupons.Where(x => x.Code == request.Code).Count();
            if (count > 0) {
                return Result<short>.Failure("Already exists");
            }

            var coupon = new Coupon
            {
                Code = request.Code,
                DiscountType = (byte)request.DiscountType,
                ExpiryDate = request.ExpiryDate,
                IsActive = request.IsActive,
                MinimumPurchaseAmount = request.MinimumPurchaseAmount,
                DiscountValue = request.DiscountValue,
                UsageCount = request.UsageCount,
                MaxUsageCount = request.MaxUsageCount,
            };

            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Result<short>.Success(coupon.Id);
        }
        catch (Exception)
        {
            return Result<short>.Failure("Coupon could not be created.");
        }
    }

}
