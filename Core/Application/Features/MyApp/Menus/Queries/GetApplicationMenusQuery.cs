

using Application.Features.MyApp.Menus.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MyApp.Menus.Queries;

public record GetApplicationMenusQuery() : IRequest<Result<List<NavigationMenuDto>>>;

public class GetApplicationMenusHandler : IRequestHandler<GetApplicationMenusQuery, Result<List<NavigationMenuDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetApplicationMenusHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<NavigationMenuDto>>> Handle(GetApplicationMenusQuery request, CancellationToken cancellationToken)
    {
        var menus = await _context.NavigationMenus
            .Select(m => new NavigationMenuDto
            {
                Id = m.Id,
                Title = m.Title,
                Url = m.Url,
                IconName = m.IconName,
                ParentId = m.ParentId,
                Children = new List<NavigationMenuDto>()
            })
            .ToListAsync(cancellationToken);

        return Result<List<NavigationMenuDto>>.Success(menus);
    }
}