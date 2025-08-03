

namespace Shared.Enums;

public enum OrderStatus
{
    Pending = 0,        // Order placed, waiting to be processed
    Processing = 1,     // Actively being prepared
    Shipped = 2,        // On the way
    Delivered = 3,      // Reached customer
    Cancelled = 4,      // Cancelled by user or admin
    Returned = 5        // Product returned
}