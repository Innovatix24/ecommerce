
using Domain.Entities.Products;

namespace Application.Features.Products.Commands;

public class AddProductImageCommand : IRequest<Result<int>>
{
    public short ProductId { get; set; }
    public string Url { get; set; } = "";
    public string Tag { get; set; } = "";
    public byte DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public class AddProductImageCommandHandler : IRequestHandler<AddProductImageCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public AddProductImageCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Url))
            return Result<int>.Failure("Image path is required");

        var image = new ProductImage
        {
            ProductId = request.ProductId,
            Url = request.Url,
            Tag = request.Tag,
            DisplayOrder = request.DisplayOrder,
            IsPrimary = request.IsPrimary,
        };

        _context.ProductImages.Add(image);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(image.Id);
    }
}