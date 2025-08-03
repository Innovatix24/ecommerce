

using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Images.Commands;

public record DeleteImagesByOwnerCommand(IImageOwner Owner) : IRequest<Result<int>>;
public class DeleteImagesByOwnerHandler : IRequestHandler<DeleteImagesByOwnerCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public DeleteImagesByOwnerHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeleteImagesByOwnerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var images = await _context.Images
                .Where(img => img.OwnerId == request.Owner.OwnerId && img.OwnerType == request.Owner.OwnerType)
                .ToListAsync(cancellationToken);

            if (images.Count == 0)
                return Result<int>.Success(0);

            _context.Images.RemoveRange(images);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(images.Count);
        }
        catch (Exception)
        {
            return Result<int>.Failure("Failed to delete images for this owner.");
        }
    }
}
