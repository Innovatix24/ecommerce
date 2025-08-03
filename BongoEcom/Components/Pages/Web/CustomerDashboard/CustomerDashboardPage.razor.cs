using Application.Features.Orders.Queries;

namespace BongoEcom.Components.Pages.Web.CustomerDashboard;

public partial class CustomerDashboardPage
{
    private CustomerDto? customer;
    private List<OrderDto> orders = new();
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            //await LoadOrders();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load dashboard: {ex.Message}");
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task LoadOrders()
    {
        var query  = new GetOrdersByCustomerIdQuery();
        var response = await _mediator.Send(query);
        if (response.IsSuccess) 
        {
            orders = response.Data;
        }
    }

    private void EditProfile()
    {
        // You can implement modal or redirect to profile update page
        Console.WriteLine("Profile edit clicked");
    }

    private string GetStatusBadge(string status) => status switch
    {
        "Pending" => "bg-warning text-dark",
        "Shipped" => "bg-info text-white",
        "Delivered" => "bg-success",
        "Cancelled" => "bg-danger",
        _ => "bg-secondary"
    };
}


public class CustomerDto
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    // other fields as needed
}
