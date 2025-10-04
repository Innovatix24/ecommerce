
using Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Inventories;

public record CreateProductSKUsCommand(short ProductId) : IRequest<Result<int>>;
public class CreateProductAttributesCommandHandler : IRequestHandler<CreateProductSKUsCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public CreateProductAttributesCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateProductSKUsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null) return Result<int>.Failure("Failed to create sku.");

            var attributes = await _context.ProductAttributes
                .Include(x=> x.Values)
                .Where(x => x.ProductId == request.ProductId)
                .ToListAsync();

            var dbSKUs = await _context.SKUs
                .Where(x => x.ProductId == request.ProductId)
                .ToListAsync();

            var skus = new SKUGenerationService().GenerateSKUs(product, attributes);

            foreach (var sku in skus) 
            {
                if (dbSKUs.Any(x => x.SKUCode == sku.SKUCode)) continue;
                await _context.AddAsync(sku);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(1);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure("Failed to create sku.");
        }
    }
}
