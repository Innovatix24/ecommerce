
using Application.Features.Products.DTOs;
using Domain.Entities.Products;

namespace Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = string.Empty;
    public string NameBangla { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal RegularPrice { get; set; }
    public decimal SalePrice { get; set; }
    public string Tag { get; set; } = string.Empty;
    public bool InStock { get; set; }
    public string Description { get; set; } = string.Empty;
    public short CategoryId { get; set; }
    public List<ProductAttributeDto> Attributes { get; set; } = new();
    public List<ProductImageDto> Images { get; set; } = new();
    public List<ProductSpecificationDto> Specifications { get; set; } = new();
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public CreateProductHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var product = new Product
            {
                Name = request.Name,
                NameBangla = request.NameBangla,
                ShortDescription = request.ShortDescription,
                Tag = request.Tag,
                InStock = request.InStock,
                Description = request.Description,
                RegularPrice = request.RegularPrice,
                SalePrice = request.SalePrice,
                CategoryId = request.CategoryId,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var item in request.Images)
            {
                var image = new ProductImage
                {
                    ProductId = product.Id,
                    Url = item.Url,
                    Tag = item.Tag,
                    DisplayOrder = item.DisplayOrder,
                    IsPrimary = item.IsPrimary,
                };

                _context.ProductImages.Add(image);
            }

            foreach (var item in request.Attributes)
            {
                var attribute = new ProductAttribute
                {
                    AttributeId = item.AttributeId,
                    ProductId = product.Id,
                    Values = item.Values.Select(x=> new ProductAttributeValue
                    {
                        Value = x.Value,
                    }).ToList(),
                };

                _context.ProductAttributes.Add(attribute);
            }

            foreach (var item in request.Specifications)
            {
                var spec = new ProductSpecification
                {
                    ProductId = product.Id,
                    Key = item.Key,
                    Value = item.Value,
                };

                _context.ProductSpecifications.Add(spec);
            }

            await _context.SaveChangesAsync(cancellationToken);

            await _context.Database.CommitTransactionAsync();
            return Result<int>.Success(product.Id);
        }
        catch(Exception ex)
        {
            await _context.Database.RollbackTransactionAsync();
            return Result<int>.Failure("Product is not created");
        }
    }
}