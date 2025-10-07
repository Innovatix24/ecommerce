

using Application.Features.Attributes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Attributes.Queries;

public record GetAttributeDetailsByIdQuery(short Id) : IRequest<Result<AttributeDto>>;

public class GetAttributeDetailsByIdQueryHandler : IRequestHandler<GetAttributeDetailsByIdQuery, Result<AttributeDto>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetAttributeDetailsByIdQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<AttributeDto>> Handle(GetAttributeDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();

            var attribute = await context.Attributes
                .Include(a => a.Values)
                .Where(x=> x.Id == request.Id)
                .Select(a => new AttributeDto
                {
                    Id = a.Id,
                    GroupId = a.GroupId,
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
                .FirstOrDefaultAsync(cancellationToken);

            if (attribute == null) 
            {
                return Result<AttributeDto>.Failure("Something went wrong");
            }

            return Result<AttributeDto>.Success(attribute);
        }
        catch (Exception ex)
        {
            return Result<AttributeDto>.Failure("Something went wrong");
        }
    }
}
