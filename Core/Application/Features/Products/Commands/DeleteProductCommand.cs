
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands;

public record DeleteProductCommand(short Id) : IRequest<Result>;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

        if (product == null)
        {
            return Result.Failure("Product not found.");
        }
        _context.Products.Remove(product);

        var images = await _context.ProductImages.Where(x => x.ProductId == request.Id).ToListAsync();
        _context.ProductImages.RemoveRange(images);

        var specs = await _context.ProductSpecifications.Where(x => x.ProductId == request.Id).ToListAsync();
        _context.ProductSpecifications.RemoveRange(specs);

        var attributes = await _context.ProductAttributes.Where(x => x.ProductId == request.Id).ToListAsync();
        _context.ProductAttributes.RemoveRange(attributes);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}