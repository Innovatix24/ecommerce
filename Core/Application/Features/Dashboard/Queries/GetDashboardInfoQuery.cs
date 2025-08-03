
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Application.Features.Dashboard.Queries;

public class DashboardDetail
{
    public int TotalOrder { get; set; }
    public int PendingOrder { get; set; }
    public int CancelledOrder { get; set; }
    public int DeliveredOrder { get; set; }
    public int ReturnedOrder { get; set; }
    public int TotalCustomer { get; set; }
    public int TotalProducts { get; set; }
}

public record GetDashboardInfoQuery : IRequest<Result<DashboardDetail>>;

public class GetDashboardInfoQueryHandler : IRequestHandler<GetDashboardInfoQuery, Result<DashboardDetail>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetDashboardInfoQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<DashboardDetail>> Handle(GetDashboardInfoQuery request, CancellationToken cancellationToken)
    {
        var _context = _contextFactory.CreateDbContext();
        var orderCount = await _context.Orders.CountAsync();
        var pendingOrder = await _context.Orders.Where(x=> x.Status == (byte)OrderStatus.Pending).CountAsync();
        var cancelledOrder = await _context.Orders.Where(x=> x.Status == (byte)OrderStatus.Cancelled).CountAsync();
        var deliveredOrder = await _context.Orders.Where(x=> x.Status == (byte)OrderStatus.Delivered).CountAsync();
        var returnedOrder = await _context.Orders.Where(x=> x.Status == (byte)OrderStatus.Returned).CountAsync();
        var userCount = await _context.Users.CountAsync();
        var totalProducts = await _context.Products.CountAsync();

        var data = new DashboardDetail
        {
            TotalCustomer = userCount,
            TotalOrder = orderCount,
            PendingOrder = pendingOrder,
            DeliveredOrder = deliveredOrder,
            ReturnedOrder = returnedOrder,
            CancelledOrder = cancelledOrder,
            TotalProducts = totalProducts,
        };

        return Result<DashboardDetail>.Success(data);
    }
}
