
using Microsoft.Data.SqlClient;
using System.Data;

namespace Application.Features.Inventories;

public class SKUDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string SKUCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int StockQuantity { get; set; }
    public int? ReorderLevel { get; set; }
    public string? Barcode { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public List<SkuAttributeDto> AttributeValues { get; set; } = new();

    public string AttributeStr
    {
        get
        {
            var attr = "";
            foreach (var item in AttributeValues)
            {
                attr += item.Attribute + " : " + item.Value + ", ";
            }
            return attr.Trim(',').Trim();
        }
    }
}

public class SkuAttributeDto
{
    public int SkuId { get; set; }
    public required string Attribute { get; set; }
    public required string Value { get; set; }
}


public class Result<T>
{
    public bool Succeeded { get; private set; }
    public string? Error { get; private set; }
    public T? Data { get; private set; }

    public static Result<T> Success(T data) => new() { Succeeded = true, Data = data };
    public static Result<T> Failure(string error) => new() { Succeeded = false, Error = error };
}

public record GetSKUsByProductIdQuery(int ProductId) : IRequest<Result<List<SKUDto>>>;

// Handler
public class GetSKUsByProductIdQueryHandler
    : IRequestHandler<GetSKUsByProductIdQuery, Result<List<SKUDto>>>
{
    private readonly IDbConnection _connection;

    public GetSKUsByProductIdQueryHandler(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Result<List<SKUDto>>> Handle(GetSKUsByProductIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            const string sql = @"
                select 
	Id,
	ProductId,
	SKUCode,
	StockQuantity,
	Price,
	ISNULL(DiscountPrice, Price) DiscountPrice,
	ISNULL(ImageUrl,'') ImageUrl,
	IsActive
from SKUs 
where ProductId = @ProductId;

select sku.Id SkuId, att.Name Attribute, v.[Value] from SKUs sku
    join SKUAttributeValues atv on atv.SKUId = sku.Id
    join Attributes att on att.Id = atv.AttributeId
    join ProductAttributeValues v on v.Id = atv.AttributeValueId
where sku.ProductId =  @ProductId;";

            var command = _connection.CreateCommand();
            command.CommandText = sql;

            var param = command.CreateParameter();
            param.ParameterName = "@ProductId";
            param.Value = request.ProductId;
            command.Parameters.Add(param);

            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync(cancellationToken);

            var result = new List<SKUDto>();

            using (var reader = await ((SqlCommand)command).ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var sku = new SKUDto
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        ProductId = reader.GetInt16(reader.GetOrdinal("ProductId")),
                        SKUCode = reader.GetString(reader.GetOrdinal("SKUCode")),
                        StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        DiscountPrice = reader.GetDecimal(reader.GetOrdinal("DiscountPrice")),
                        ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    };

                    result.Add(sku);
                }

                if (await reader.NextResultAsync(cancellationToken))
                {
                    var attributes = new List<SkuAttributeDto>();
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var attr = new SkuAttributeDto
                        {
                            SkuId = reader.GetInt32(reader.GetOrdinal("SkuId")),
                            Attribute = reader.GetString(reader.GetOrdinal("Attribute")),
                            Value = reader.GetString(reader.GetOrdinal("Value"))
                        };
                        attributes.Add(attr);
                    }

                    foreach (var sku in result)
                    {
                        sku.AttributeValues = attributes.FindAll(a => a.SkuId == sku.Id);
                    }
                }
            }

            await ((SqlConnection)_connection).CloseAsync();

            return Result<List<SKUDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<SKUDto>>.Failure(ex.Message);
        }
    }
}