
using Application.Features.Categories.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Queries;

public record GetCategoryByIdQuery(short Id) : IRequest<Result<CategoryDto>>;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetCategoryByIdQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();

            var category = await context.Categories
                .Where(x=> x.Id == request.Id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description ?? ""
                })
                .FirstOrDefaultAsync(cancellationToken);

            return Result<CategoryDto>.Success(category);
        }
        catch (Exception ex)
        {
            return Result<CategoryDto>.Failure("Category not found");
        }
    }
}
