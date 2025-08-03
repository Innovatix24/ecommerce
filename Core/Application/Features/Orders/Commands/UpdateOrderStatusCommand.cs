
using Domain.Entities.Orders;
using Shared.Enums;

namespace Application.Features.Orders.Commands;

public class UpdateOrderStatusCommand : IRequest<Result>
{
    public int OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
}

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public UpdateOrderStatusCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);
        if (order == null)
            return Result.Failure($"Order with ID {request.OrderId} not found.");

        var currentStatus = (OrderStatus)order.Status;
        var newStatus = request.NewStatus;

        if (!OrderStatusStateMachine.CanTransition(currentStatus, newStatus))
            return Result.Failure($"Cannot change status from {currentStatus} to {newStatus}.");

        order.Status = (byte)newStatus;
        _context.Orders.Update(order);

        var log = new OrderHistory
        {
            OrderId = order.Id,
            Status = (byte)newStatus,
            Note = "",
            CreatedAt = DateTime.UtcNow
        };
        _context.OrderHistories.Add(log);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public static class OrderStatusStateMachine
{
    private static readonly Dictionary<OrderStatus, List<OrderStatus>> _transitions = new()
    {
        [OrderStatus.Pending] = new List<OrderStatus> { OrderStatus.Processing, OrderStatus.Cancelled },
        [OrderStatus.Processing] = new List<OrderStatus> { OrderStatus.Shipped, OrderStatus.Cancelled },
        [OrderStatus.Shipped] = new List<OrderStatus> { OrderStatus.Delivered, OrderStatus.Returned },
        [OrderStatus.Delivered] = new List<OrderStatus> { OrderStatus.Returned },
        [OrderStatus.Cancelled] = new List<OrderStatus>(),
        [OrderStatus.Returned] = new List<OrderStatus>(),
    };

    public static bool CanTransition(OrderStatus current, OrderStatus next)
    {
        if (_transitions.TryGetValue(current, out var allowedNextStatuses))
        {
            return allowedNextStatuses.Contains(next);
        }
        return false;
    }
}