

using Application.Features.Attributes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Attributes.Queries;

public class AttributeGroupDto
{
    public short Id { get; set; }
    public string Name { get; set; }
    public List<AttributeDto> Attributes { get; set; } = new();
}

public record GetAttributeGroups(bool withDetails = false) : IRequest<Result<List<AttributeGroupDto>>>;

public class GetAttributeGroupsHandler : IRequestHandler<GetAttributeGroups, Result<List<AttributeGroupDto>>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetAttributeGroupsHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<List<AttributeGroupDto>>> Handle(GetAttributeGroups request, CancellationToken cancellationToken)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();

            if (request.withDetails)
            {
                var groups = await context.AttributeGroups
                    .Include(x => x.Attributes)
                    .Select(c => new AttributeGroupDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Attributes = c.Attributes.Select(x => new AttributeDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            DataType = "",
                            InputType = "",
                        }).ToList()
                    })
                    .ToListAsync(cancellationToken);
                return Result<List<AttributeGroupDto>>.Success(groups);
            }
            else
            {
                var groups = await context.AttributeGroups
                    .Select(c => new AttributeGroupDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToListAsync(cancellationToken);
                return Result<List<AttributeGroupDto>>.Success(groups);
            }
        }
        catch (Exception ex)
        {
            return Result<List<AttributeGroupDto>>.Failure("Something went wrong");
        }
    }
}
