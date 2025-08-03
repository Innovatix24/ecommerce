

using Application.Features.Products.DTOs;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands;

public class UpdateProductCommand : IRequest<Result<int>>
{
    public short ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameBangla { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal RegularPrice { get; set; }
    public decimal SalePrice { get; set; }
    public short CategoryId { get; set; }
    public List<ProductAttributeDto> Attributes { get; set; } = new();
    public List<ProductImageDto> Images { get; set; } = new();
    public List<ProductSpecificationDto> Specifications { get; set; } = new();
    public bool InStock { get; set; }
    public string Code { get; set; } = string.Empty;
}


public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public UpdateProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId);
            if(product is null)
            {
                return Result<int>.Failure("Product is not found");
            }
            product.Name = request.Name;
            product.ShortDescription = request.ShortDescription;
            product.Tag = request.Tag;
            product.Code = request.Code;

            product.Description = request.Description;
            product.RegularPrice = request.RegularPrice;
            product.SalePrice = request.SalePrice;
            product.CategoryId = request.CategoryId;
            product.InStock = request.InStock;

            _context.Products.Update(product);
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

            var specs = await _context.ProductSpecifications.Where(x => x.ProductId == product.Id).ToListAsync();
            foreach (var att in specs)
            {
                _context.ProductSpecifications.Remove(att);
            }
            await _context.SaveChangesAsync();

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
        catch (Exception ex)
        {
            await _context.Database.RollbackTransactionAsync();
            return Result<int>.Failure("Product is not created");
        }

    }
}