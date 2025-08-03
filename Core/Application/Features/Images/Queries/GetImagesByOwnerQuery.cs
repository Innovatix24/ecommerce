
using Application.Features.Images.DTOs;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Images.Queries;

public record GetImagesByOwnerQuery(IImageOwner ImageOwner) : IRequest<Result<List<ImageDto>>>;

public class GetImagesByOwnerHandler : IRequestHandler<GetImagesByOwnerQuery, Result<List<ImageDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetImagesByOwnerHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ImageDto>>> Handle(GetImagesByOwnerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var images = await _context.Images
                .Where(img => img.OwnerId == request.ImageOwner.OwnerId && img.OwnerType == request.ImageOwner.OwnerType)
                .Select(img => new ImageDto
                {
                    Id = img.Id,
                    Url = img.Url,
                    Tag = img.Tag,
                })
                .ToListAsync(cancellationToken);

            return Result<List<ImageDto>>.Success(images);
        }
        catch (Exception)
        {
            return Result<List<ImageDto>>.Failure("Failed to retrieve images.");
        }
    }
}
