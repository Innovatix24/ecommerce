
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

            var attributes = await context.Attributes
                .Include(x=> x.Group)
                .Include(a => a.Values)
                .Select(a => new AttributeDto
                {
                    Id = a.Id,
                    GroupId = a.GroupId,
                    GroupName = a.Group.Name,
                    Name = a.Name,
                    InputType = "",
                    DataType = "", 
                    Values = a.Values
                      .OrderBy(v => v.DisplayOrder)
                      .Select(v => new AttributeValueDto
                      {
                          Id = v.Id,
                          Value = v.Value,
                          DisplayOrder = v.DisplayOrder
                      })
                      .ToList()
                })
                .ToListAsync(cancellationToken);

            return Result<List<AttributeDto>>.Success(attributes);
        }
        catch (Exception ex)
        {
            return Result<List<AttributeDto>>.Failure("Something went wrong");
        }
    }
}
