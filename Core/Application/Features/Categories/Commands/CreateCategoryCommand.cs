
using Domain.Entities;
using Domain.Entities.Categories;

namespace Application.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<Result<short>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public CreateCategoryCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<short>.Failure("Category name is required.");

        bool nameExists = _context.Categories.Any(c => c.Name == request.Name);
        if (nameExists)
            return Result<short>.Failure("Category name already exists.");

        var category = new Category
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<short>.Success(category.Id);
    }
}