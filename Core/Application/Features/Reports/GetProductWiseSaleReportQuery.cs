
using System.Data;

namespace Application.Features.Reports;

public class ProductWiseSaleDto
{
    public string ProductName { get; set; } = default!;
    public int Qty { get; set; }
    public decimal TotalSale { get; set; }
}

public record GetProductWiseSaleReportQuery(DateOnly From, DateOnly To) : IRequest<Result<List<ProductWiseSaleDto>>>;

public class GetProductWiseSaleReportQueryHandler : IRequestHandler<GetProductWiseSaleReportQuery, Result<List<ProductWiseSaleDto>>>
{
    private readonly IDbConnection _connection;

    public GetProductWiseSaleReportQueryHandler(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Result<List<ProductWiseSaleDto>>> Handle(GetProductWiseSaleReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var sql = string.Format($@"
            Declare @from Date = '{request.From}', @to Date = '{request.To}';

            select 
	            p.Name ProductName,
	            sum(i.Quantity) Qty,
	            SUM(i.Quantity * i.UnitPrice) TotalSale
            from Orders o
	            join OrderItem i on o.Id = i.OrderId
	            join Products p on p.Id = i.ProductId
            where o.CreatedAt between @from and @to
            group by p.Name");

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            _connection.Open();

            var reader = cmd.ExecuteReader();
            List<ProductWiseSaleDto> data = new();
            while (reader.Read())
            {
                var item = new ProductWiseSaleDto
                {
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    Qty = reader.GetInt32(reader.GetOrdinal("Qty")),
                    TotalSale = reader.GetDecimal(reader.GetOrdinal("TotalSale")),
                };
                data.Add(item);
            }

            _connection.Close();

            return Result<List<ProductWiseSaleDto>>.Success(data);
        }
        catch (Exception ex) 
        {
            return Result<List<ProductWiseSaleDto>>.Failure(ex.Message);
        }
    }
}
