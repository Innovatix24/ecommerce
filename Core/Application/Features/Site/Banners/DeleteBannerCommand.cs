
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Site.Banners;

public record DeleteBannerCommand(byte Id) : IRequest<Result>;

public class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteBannerCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
    {
        var Banner = await _context.Banners.FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken);
        if (Banner == null)
        {
            return Result.Failure("Banner not found.");
        }
        _context.Banners.Remove(Banner);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}