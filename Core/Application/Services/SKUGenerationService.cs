
using Application.Features.Products.DTOs;
using Domain.Entities.Inventories;
using Domain.Entities.Products;

namespace Application.Services;

public class SKUGenerationService
{
    public List<SKU> GenerateSKUs(Product product, List<ProductAttribute> attributes)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (attributes == null || attributes.Count == 0)
            throw new ArgumentException("Product must have at least one attribute.", nameof(attributes));

        var combinations = GetCombinations(attributes.Select(a => a.Values).ToList());

        var skus = new List<SKU>();
        foreach (var combo in combinations)
        {
            var ordered = combo.OrderBy(x => x.Id).ToList();
            string codeSuffix = string.Join("-", ordered.Select(v => v.Value.ToUpper().Replace(" ", "")));
            string skuCode = $"{product.Id}-{codeSuffix}";

            var sku = new SKU
            {
                ProductId = product.Id,
                SKUCode = skuCode,
                Price = product.SalePrice,
                StockQuantity = 0,
                IsActive = true,
            };

            var values = new List<SKUAttributeValue>();
            foreach (var item in combo)
            {
                var pAtt = attributes.Where(x => x.Id == item.ProductAttributeId).FirstOrDefault();
                if (pAtt == null) continue;

                values.Add(new SKUAttributeValue
                {
                    AttributeId = pAtt.AttributeId,
                    AttributeValueId = item.Id,
                });
            }

            sku.AttributeValues = values;

            skus.Add(sku);
        }

        return skus;
    }

    private List<List<ProductAttributeValue>> GetCombinations(List<List<ProductAttributeValue>> lists)
    {
        var result = new List<List<ProductAttributeValue>> 
        { 
            new List<ProductAttributeValue>() 
        };

        foreach (var list in lists)
        {
            var newResult = new List<List<ProductAttributeValue>>();
            foreach (var combination in result)
            {
                foreach (var value in list)
                {
                    var newCombo = new List<ProductAttributeValue>(combination) { value };
                    newResult.Add(newCombo);
                }
            }
            result = newResult;
        }

        return result;
    }
}