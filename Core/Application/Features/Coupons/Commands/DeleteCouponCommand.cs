
namespace Application.Features.Coupons.Commands;

public record DeleteCouponCommand(short Id) : IRequest<Result>;

public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteCouponCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Coupons.FindAsync(new object[] { request.Id }, cancellationToken);

        if (category == null)
            return Result.Failure("Category not found.");

        _context.Coupons.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}