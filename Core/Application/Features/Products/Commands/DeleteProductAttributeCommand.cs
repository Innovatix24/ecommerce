
namespace Application.Features.Products.Commands;

public record DeleteProductAttributeCommand(short Id) : IRequest<Result>;

public class DeleteProductAttributeCommandHandler : IRequestHandler<DeleteProductAttributeCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteProductAttributeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteProductAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await _context.ProductAttributes.FindAsync(new object[] { request.Id }, cancellationToken);

        if (attribute == null)
            return Result.Failure("Image not found.");

        _context.ProductAttributes.Remove(attribute);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}