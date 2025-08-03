

using Microsoft.EntityFrameworkCore;

namespace Application.Features.Attributes.Commands;

public class UpdateAttributeCommand : IRequest<Result>
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string InputType { get; set; } = string.Empty;
}

public class UpdateAttributeCommandHandler : IRequestHandler<UpdateAttributeCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public UpdateAttributeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (attribute == null)
            return Result.Failure("Attribute not found.");

        var nameExists = await _context.Categories
            .AnyAsync(c => c.Id != request.Id && c.Name == request.Name, cancellationToken);

        if (nameExists)
            return Result.Failure("Another attribute with the same name already exists.");

        attribute.Name = request.Name.Trim();
        attribute.InputType = request.InputType.Trim();
        attribute.DataType = request.DataType.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
