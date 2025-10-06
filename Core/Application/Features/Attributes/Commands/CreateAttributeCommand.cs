using Application.Features.Attributes.DTOs;
using Domain.Entities.Products.Attributes;
using Microsoft.EntityFrameworkCore;
using Attribute = Domain.Entities.Products.Attributes.Attribute;


namespace Application.Features.Attributes.Commands;

public class CreateAttributeCommand : IRequest<Result<short>>
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public short GroupId { get; set; }
    public bool IsVariantable { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<AttributeValueDto> Values { get; set; } = new();
}

public class CreateAttributeCommandHandler : IRequestHandler<CreateAttributeCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public CreateAttributeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<short>.Failure("Attribute name is required.");

        bool nameExists = _context.Attributes.Any(c => c.Name == request.Name);
        if (nameExists)
            return Result<short>.Failure("Attribute name already exists.");

        var attribute = new Attribute
        {
            Name = request.Name,
            GroupId = request.GroupId,
            IsVariantable = request.IsVariantable,
        };

        _context.Attributes.Add(attribute);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var item in request.Values)
        {
            if(item.Id == 0)
            {
                var value = new AttributeValue
                {
                    Value = request.Name,
                    AttributeId = attribute.Id,
                    DisplayOrder = item.DisplayOrder,
                };
                _context.AttributeValues.Add(value);
            }
            else
            {
                var value = await _context.AttributeValues.FirstOrDefaultAsync(x=> x.Id == item.Id);
                if (value == null) continue;
                value.Value = item.Value;
                value.DisplayOrder = item.DisplayOrder;
                _context.AttributeValues.Update(value);
            }
        }
        await _context.SaveChangesAsync(cancellationToken);

        return Result<short>.Success(attribute.Id);
    }
}