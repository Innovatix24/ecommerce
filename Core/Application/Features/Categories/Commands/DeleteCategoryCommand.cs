

namespace Application.Features.Categories.Commands;
public record DeleteCategoryCommand(short Id) : IRequest<Result>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteCategoryCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

        if (category == null)
            return Result.Failure("Category not found.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}