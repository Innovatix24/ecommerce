
using Domain.Entities.Site;

namespace Application.Features.Site.Banners;

public class AddEditBannerCommand : IRequest<Result<short>>
{
    public byte Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string TargetUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class AddEditBannerCommandHandler : IRequestHandler<AddEditBannerCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public AddEditBannerCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(AddEditBannerCommand request, CancellationToken cancellationToken)
    {
        if(request.Id == 0)
        {
            var banner = new Banner
            {
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                TargetUrl = request.TargetUrl,
                DisplayOrder = request.DisplayOrder,
                Description = request.Description?.Trim() ?? string.Empty,
                IsActive = request.IsActive
            };

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<short>.Success(banner.Id);
        }
        else
        {
            var banner = _context.Banners.FirstOrDefault(banner => banner.Id == request.Id);
            if (banner != null) 
            {
                banner.Title = request.Title;
                banner.ImageUrl = request.ImageUrl;
                banner.TargetUrl = request.TargetUrl;
                banner.DisplayOrder = request.DisplayOrder;
                banner.Description = request.Description?.Trim() ?? string.Empty;
                _context.Banners.Update(banner);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<short>.Success(banner.Id);
            }
        }
        return Result<short>.Failure("Something went wrong");

    }
}