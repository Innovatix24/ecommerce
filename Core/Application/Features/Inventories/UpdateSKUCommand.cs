
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Inventories;

public class UpdateSKUCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateSKUCommandHandler : IRequestHandler<UpdateSKUCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public UpdateSKUCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(UpdateSKUCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sku = await _context.SKUs.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (sku != null)
            {
                sku.Price = request.Price;
                sku.DiscountPrice = request.DiscountPrice;
                sku.StockQuantity = request.StockQuantity;
                sku.ImageUrl = request.ImageUrl;
                sku.IsActive = request.IsActive;

                _context.SKUs.Update(sku);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<int>.Success(sku.Id);
            }
            else
            {
                return Result<int>.Failure("Failed to update sku.");
            }
        }
        catch (Exception ex)
        {
            return Result<int>.Failure("Failed to create sku.");
        }
    }
}
