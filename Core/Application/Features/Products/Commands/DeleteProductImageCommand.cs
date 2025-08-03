
namespace Application.Features.Products.Commands;

public record DeleteProductImageCommand(int Id) : IRequest<Result>;

public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteProductImageCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.ProductImages.FindAsync(new object[] { request.Id }, cancellationToken);

        if (category == null)
            return Result.Failure("Image not found.");

        _context.ProductImages.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}