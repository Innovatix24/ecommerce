
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Result>
{
    public short Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public UpdateCategoryCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category == null)
            return Result.Failure("Category not found.");

        var nameExists = await _context.Categories
            .AnyAsync(c => c.Id != request.Id && c.Name == request.Name, cancellationToken);

        if (nameExists)
            return Result.Failure("Another category with the same name already exists.");

        category.Name = request.Name.Trim();
        category.Description = request.Description.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
