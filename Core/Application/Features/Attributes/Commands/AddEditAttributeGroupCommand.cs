

using Domain.Entities.Products.Attributes;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Features.Attributes.Commands;

public class AddEditAttributeGroupCommand : IRequest<Result<short>>
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class AddEditAttributeGroupCommandHandler : IRequestHandler<AddEditAttributeGroupCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public AddEditAttributeGroupCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(AddEditAttributeGroupCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<short>.Failure("Group name is required.");

        bool nameExists = _context.Attributes.Any(c => c.Name == request.Name);
        if (nameExists)
            return Result<short>.Failure("Group name already exists.");

        var group = new AttributeGroup();

        if (request.Id > 0)
        {
            group = await _context.AttributeGroups.FirstOrDefaultAsync(x=> x.Id == request.Id);
            if (group != null) 
            {
                group.Name = request.Name;
                _context.AttributeGroups.Update(group);
            }
        }
        else
        {
            group = new AttributeGroup
            {
                Name = request.Name,
            };
            _context.AttributeGroups.Add(group);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<short>.Success(group.Id);
    }
}