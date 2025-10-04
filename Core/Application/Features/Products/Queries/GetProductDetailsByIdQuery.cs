

using Application.Features.Products.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Application.Features.Products.Queries;

public record GetProductDetailsByIdQuery(short ProductId) : IRequest<Result<ProductDto>>;

public class GetProductDetailsByIdQueryHandler : IRequestHandler<GetProductDetailsByIdQuery, Result<ProductDto>>
{
    private readonly ApplicationDbContext _context;

    public GetProductDetailsByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProductDto>> Handle(GetProductDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Where(x=> x.Id == request.ProductId)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                NameBangla = p.NameBangla,
                Tag = p.Tag,
                ShortDescription = p.ShortDescription,
                LongDescription = p.Description,
                RegularPrice = p.RegularPrice,
                SalePrice = p.SalePrice,
                CategoryId = (short)p.CategoryId,
                InStock = p.InStock,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);


        if (product == null)
        {
            return Result<ProductDto>.Failure("Product not found");
        }

        var images = await _context.ProductImages.Where(x => x.ProductId == product.Id).Select(x => new ProductImageDto
        {
            Id = x.Id,
            Url = x.Url,
            DisplayOrder = x.DisplayOrder,
            Tag = x.Tag,
        }).ToListAsync();

        product.Images = images;

        var specs = await _context.ProductSpecifications.Where(x => x.ProductId == product.Id).Select(x => new ProductSpecificationDto
        {
            Id = x.Id,
            Key = x.Key,
            Value = x.Value,
        }).ToListAsync();

        product.Specifications = specs;

        var attributes = await _context.ProductAttributes
            .Include(x=> x.Values)
            .Include(x=> x.Attribute)
            .Where(x => x.ProductId == product.Id)
            .Select(x => new ProductAttributeDto
        {
            Id = x.Id,
            AttributeId = x.AttributeId,
            Name = x.Attribute.Name,
            Values = x.Values.Select(v => new ProductAttributeValueDto
            {
                Id = v.Id,
                AttributeId = x.Id,
                Value = v.Value,
            }).ToList(),
        }).ToListAsync();

        foreach (var item in attributes)
        {
            var first = item.Values.First();
            first.IsSelected = true;
        }

        product.Attributes = attributes;

        return Result<ProductDto>.Success(product);
    }
}