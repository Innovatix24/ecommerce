

using Application.Features.Attributes.DTOs;
using Domain.Entities.Products.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Attributes.Commands;

public class UpdateAttributeCommand : IRequest<Result>
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public short GroupId { get; set; }
    public bool IsVariantable { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<AttributeValueDto> Values { get; set; } = new();
}

public class UpdateAttributeCommandHandler : IRequestHandler<UpdateAttributeCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public UpdateAttributeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (attribute == null)
            return Result.Failure("Attribute not found.");

        var nameExists = await _context.Categories
            .AnyAsync(c => c.Id != request.Id && c.Name == request.Name, cancellationToken);

        if (nameExists)
            return Result.Failure("Another attribute with the same name already exists.");

        attribute.Name = request.Name.Trim();
        attribute.GroupId = request.GroupId;
        attribute.IsVariantable = request.IsVariantable;

        var values = await _context.AttributeValues.Where(c => c.AttributeId == request.Id).ToListAsync();
        var deletedItems = new List<AttributeValue>();

        var valueIds = request.Values.Select(x => x.Id).ToArray();
        deletedItems = values.Where(x => !valueIds.Contains(x.Id)).ToList();

        await _context.SaveChangesAsync(cancellationToken);

        foreach (var item in request.Values)
        {
            if (item.Id == 0)
            {
                var value = new AttributeValue
                {
                    Value = item.Value,
                    AttributeId = attribute.Id,
                    DisplayOrder = item.DisplayOrder,
                };
                _context.AttributeValues.Add(value);
            }
            else
            {
                var value = await _context.AttributeValues.FirstOrDefaultAsync(x => x.Id == item.Id);
                if (value == null) continue;
                value.Value = item.Value;
                value.DisplayOrder = item.DisplayOrder;
                _context.AttributeValues.Update(value);
            }
        }

        _context.AttributeValues.RemoveRange(deletedItems);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
