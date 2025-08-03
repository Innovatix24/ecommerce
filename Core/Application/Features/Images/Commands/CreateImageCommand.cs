
using Domain.Entities;
using Shared.Enums;

namespace Application.Features.Images.Commands;

public class CreateImageCommand : IRequest<Result<int>>
{
    public string Url { get; set; } = string.Empty;

    public int OwnerId { get; set; }
    public ImageOwnerType OwnerType { get; set; }

    public string? Tag { get; set; }
}

public class CreateImageHandler : IRequestHandler<CreateImageCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public CreateImageHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var image = new Image
            {
                Url = request.Url,
                OwnerId = request.OwnerId,
                OwnerType = (byte)request.OwnerType,
                Tag = request.Tag
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(image.Id);
        }
        catch (Exception)
        {
            return Result<int>.Failure("Image could not be created.");
        }
    }
}

