using Application.Features.Products.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;


public record GetPaginatedProductsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    int CategoryId = 0,
    string SearchTerm = "",
    string SortColumn = "Name",
    bool SortAscending = true
) : IRequest<Result<PaginatedResult<ProductDto>>>;

public record PaginatedResult<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public class GetPaginatedProductsQueryHandler : IRequestHandler<GetPaginatedProductsQuery, Result<PaginatedResult<ProductDto>>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public GetPaginatedProductsQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<PaginatedResult<ProductDto>>> Handle(
        GetPaginatedProductsQuery request,
        CancellationToken cancellationToken)
    {
        using var _context = _contextFactory.CreateDbContext();

        // Base query
        var query = _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == request.CategoryId || request.CategoryId == 0)
            .Where(p => string.IsNullOrEmpty(request.SearchTerm) ||
                       p.Name.Contains(request.SearchTerm) ||
                       p.Code.Contains(request.SearchTerm));

        // Apply sorting
        //query = request.SortColumn?.ToLower() switch
        //{
        //    "name" => request.SortAscending
        //        ? query.OrderBy(p => p.Name)
        //        : query.OrderByDescending(p => p.Name),
        //    "price" => request.SortAscending
        //        ? query.OrderBy(p => p.RegularPrice)
        //        : query.OrderByDescending(p => p.RegularPrice),
        //    _ => query.OrderBy(p => p.Name)
        //};

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Tag = p.Tag,
                ShortDescription = p.ShortDescription,
                LongDescription = p.Description,
                RegularPrice = p.RegularPrice,
                SalePrice = p.SalePrice,
                CategoryId = p.CategoryId,
                InStock = p.InStock,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty
            })
            .ToListAsync(cancellationToken);

        // Load images for the paginated products
        var productIds = products.Select(p => p.Id).ToList();
        var images = await _context.ProductImages
            .Where(x => productIds.Contains(x.ProductId))
            .Select(x => new ProductImageDto
            {
                Id = x.Id,
                Url = x.Url,
                DisplayOrder = x.DisplayOrder,
                Tag = x.Tag,
                ProductId = x.ProductId
            })
            .ToListAsync(cancellationToken);

        // Assign images to products
        var imagesLookup = images.ToLookup(x => x.ProductId);
        foreach (var product in products)
        {
            product.Images = imagesLookup[product.Id].ToList();
        }

        return Result<PaginatedResult<ProductDto>>.Success(new PaginatedResult<ProductDto>(
            Items: products,
            TotalCount: totalCount,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)request.PageSize)
        ));
    }
}