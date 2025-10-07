

using Microsoft.EntityFrameworkCore;

namespace Application.Features.Attributes.Commands;

public record DeleteAttributeCommand(short Id) : IRequest<Result>;

public class DeleteAttributeCommandHandler : IRequestHandler<DeleteAttributeCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteAttributeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes.FindAsync(new object[] { request.Id }, cancellationToken);

        if (attribute == null)
            return Result.Failure("Category not found.");

        var deletedItems = await _context.AttributeValues.Where(x => x.AttributeId == attribute.Id).ToListAsync();
        
        _context.Attributes.Remove(attribute);
        await _context.SaveChangesAsync(cancellationToken);

        _context.AttributeValues.RemoveRange(deletedItems);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
