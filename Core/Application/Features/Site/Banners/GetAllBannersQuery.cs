
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Site.Banners;

public record GetAllBannersQuery() : IRequest<Result<List<BannerDto>>>;

public class GetAllBannersQueryHandler : IRequestHandler<GetAllBannersQuery, Result<List<BannerDto>>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetAllBannersQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<List<BannerDto>>> Handle(GetAllBannersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();

            var categories = await context.Banners
                .Select(c => new BannerDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    ImageUrl = c.ImageUrl,
                    TargetUrl = c.TargetUrl,
                    DisplayOrder = c.DisplayOrder,
                    IsActive = c.IsActive,
                    Description = c.Description ?? ""
                })
                .ToListAsync(cancellationToken);

            return Result<List<BannerDto>>.Success(categories);
        }
        catch (Exception ex)
        {
            return Result<List<BannerDto>>.Failure("Something went wrong");
        }
    }
}


public class BannerDto
{
    public byte Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string TargetUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}