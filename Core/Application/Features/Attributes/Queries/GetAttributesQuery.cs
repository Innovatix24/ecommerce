
using Application.Features.Attributes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Attributes.Queries;

public record GetAttributesQuery() : IRequest<Result<List<AttributeDto>>>;

public class GetAttributesQueryHandler : IRequestHandler<GetAttributesQuery, Result<List<AttributeDto>>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetAttributesQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<List<AttributeDto>>> Handle(GetAttributesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();

            var categories = await context.Attributes
                .Select(c => new AttributeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    InputType = c.InputType ?? "",
                    DataType = c.DataType ?? ""
                })
                .ToListAsync(cancellationToken);

            return Result<List<AttributeDto>>.Success(categories);
        }
        catch (Exception ex)
        {
            return Result<List<AttributeDto>>.Failure("Something went wrong");
        }
    }
}
