

using Application.Features.Products.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;


public record GetFilterProductsQuery(string filterKey) : IRequest<Result<List<ProductDto>>>;

public class GetFilterProductsQueryHandler : IRequestHandler<GetFilterProductsQuery, Result<List<ProductDto>>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetFilterProductsQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetFilterProductsQuery request, CancellationToken cancellationToken)
    {
        using var _context = _contextFactory.CreateDbContext();

        var products = await _context.Products
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id, 
                Name = p.Name,
                Code = p.Code,
                LongDescription = p.Description,
                RegularPrice = p.RegularPrice,
                SalePrice = p.SalePrice,
                InStock = p.InStock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty
            })
            .Where(p => p.Name.ToLower().Trim().Contains(request.filterKey.ToLower().Trim()))
           
            .ToListAsync(cancellationToken);

        foreach (var item in products)
        {
            var images = await _context.ProductImages.Where(x => x.ProductId == item.Id).Select(x => new ProductImageDto
            {
                Id = x.Id,
                Url = x.Url,
                DisplayOrder = x.DisplayOrder,
                Tag = x.Tag,
            }).ToListAsync();

            item.Images = images;
        }

        return Result<List<ProductDto>>.Success(products);
    }
}