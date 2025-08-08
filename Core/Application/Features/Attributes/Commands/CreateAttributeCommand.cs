

using Attribute = Domain.Entities.Products.Attribute;


namespace Application.Features.Attributes.Commands;

public class CreateAttributeCommand : IRequest<Result<short>>
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string InputType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateAttributeCommandHandler : IRequestHandler<CreateAttributeCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public CreateAttributeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<short>.Failure("Attribute name is required.");

        bool nameExists = _context.Attributes.Any(c => c.Name == request.Name);
        if (nameExists)
            return Result<short>.Failure("Attribute name already exists.");

        var attribute = new Attribute
        {
            Name = request.Name,
            InputType = request.InputType,
            DataType = request.DataType,
        };

        _context.Attributes.Add(attribute);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<short>.Success(attribute.Id);
    }
}