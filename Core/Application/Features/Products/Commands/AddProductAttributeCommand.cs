
using Application.Features.Products.DTOs;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Features.Products.Commands;

public class AddProductAttributeCommand : IRequest<Result<short>>
{
    public short Id { get; set; }
    public short ProductId { get; set; }
    public short AttributeId { get; set; }
    public List<ProductAttributeValueDto> Values { get; set; } = new();
}

public class AddProductAttributeCommandHandler : IRequestHandler<AddProductAttributeCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public AddProductAttributeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(AddProductAttributeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == 0)
            {
                var attribute = new ProductAttribute
                {
                    AttributeId = request.AttributeId,
                    ProductId = request.ProductId,
                    Values = request.Values.Select(x => new ProductAttributeValue
                    {
                        Value = x.Value,
                    }).ToList(),
                };

                _context.ProductAttributes.Add(attribute);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<short>.Success(attribute.Id);
            }
            else
            {
                var attribute = await _context.ProductAttributes
                    .Include(x => x.Values)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (attribute is null)
                {
                    return Result<short>.Failure("Attribute not found");
                }

                var incomingValues = request.Values.ToDictionary(v => v.Id);

                var toRemove = attribute.Values
                    .Where(v => v.Id > 0 && !incomingValues.ContainsKey(v.Id))
                    .ToList();

                foreach (var item in toRemove)
                {
                    attribute.Values.Remove(item);
                }

                foreach (var existing in attribute.Values)
                {
                    if (incomingValues.TryGetValue(existing.Id, out var dto))
                    {
                        existing.Value = dto.Value;
                    }
                }

                var newValues = request.Values
                    .Where(v => v.Id == 0)
                    .Select(v => new ProductAttributeValue
                    {
                        Value = v.Value
                    });

                foreach (var newVal in newValues)
                {
                    attribute.Values.Add(newVal);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Result<short>.Success(attribute.Id);
            }
        }
        catch(Exception ex)
        {
            return Result<short>.Failure(ex.Message);
        }
    }
}