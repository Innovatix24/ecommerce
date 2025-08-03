
using Application.Features.Categories.DTOs;
using Microsoft.EntityFrameworkCore;
namespace Application.Features.Categories.Queries;

public record GetAllCategoriesQuery() : IRequest<Result<List<CategoryDto>>>;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetAllCategoriesQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();

            var categories = await context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description ?? ""
                })
                .ToListAsync(cancellationToken);

            return Result<List<CategoryDto>>.Success(categories);
        }
        catch (Exception ex)
        {
            return Result<List<CategoryDto>>.Failure("Something went wrong");
        }
    }
}
