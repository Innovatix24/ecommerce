

using Azure.Core;
using Domain.Entities.Products;

namespace Application.Features.Products.Commands;

public record GenerateProductVariantsCommand(short ProductId) : IRequest<Result<List<ProductVariant>>>;


public class GenerateProductVariantsCommandHandler : IRequestHandler<GenerateProductVariantsCommand, Result<List<ProductVariant>>>
{
    private readonly ApplicationDbContext _context;

    public GenerateProductVariantsCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ProductVariant>>> Handle(GenerateProductVariantsCommand request, CancellationToken cancellationToken) 
    { 
        try
        {
            //var attributes = await _context.Attributes
            //.Include(a => a.Values)
            //.Where(a => a.ProductId == request.ProductId)
            //.ToListAsync();

            //var valueGroups = attributes
            //    .Select(attr => attr.Values.ToList())
            //    .ToList();

            //// 3. Generate cartesian product (recursive)
            //var combinations = GetCombinations(valueGroups);

            var variants = new List<ProductVariant>();
            //foreach (var combo in combinations)
            //{
            //    var variant = new ProductVariant
            //    {
            //        ProductId = request.ProductId,
            //        Sku = GenerateSku(combo),
            //        Stock = 0,
            //        Price = 0,
            //        AttributeValues = combo.Select(val => new VariantAttributeValue
            //        {
            //            AttributeValueId = val.Id
            //        }).ToList()
            //    };

            //    variants.Add(variant);
            //}

            // Optional: Save to DB
            // _context.ProductVariants.AddRange(variants);
            // await _context.SaveChangesAsync();

            return Result<List<ProductVariant>>.Success(variants);
        }
        catch (Exception ex)
        {
            return Result<List<ProductVariant>>.Failure("Product is not created");
        }
    }

    private List<List<ProductAttributeValue>> GetCombinations(List<List<ProductAttributeValue>> input)
    {
        var result = new List<List<ProductAttributeValue>>();

        void Recurse(List<ProductAttributeValue> current, int depth)
        {
            if (depth == input.Count)
            {
                result.Add(new List<ProductAttributeValue>(current));
                return;
            }

            foreach (var value in input[depth])
            {
                current.Add(value);
                Recurse(current, depth + 1);
                current.RemoveAt(current.Count - 1);
            }
        }

        Recurse(new List<ProductAttributeValue>(), 0);
        return result;
    }

    private string GenerateSku(List<ProductAttributeValue> values)
    {
        return string.Join("-", values.Select(v => v.Value.ToUpper().Replace(" ", "")));
    }
}