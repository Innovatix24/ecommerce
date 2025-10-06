
namespace Application.Features.Attributes.Commands;

public record DeleteAttributeGroupCommand(short Id) : IRequest<Result>;

public class DeleteAttributeGroupCommandHandler : IRequestHandler<DeleteAttributeGroupCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteAttributeGroupCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteAttributeGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _context.AttributeGroups.FindAsync(new object[] { request.Id }, cancellationToken);

        if (group == null)
            return Result.Failure("Category not found.");

        _context.AttributeGroups.Remove(group);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}