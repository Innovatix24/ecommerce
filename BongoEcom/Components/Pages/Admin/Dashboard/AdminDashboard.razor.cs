using Application.Features.Dashboard.Queries;
using Application.Features.Orders.Queries;

namespace BongoEcom.Components.Pages.Admin.Dashboard;

public partial class AdminDashboard
{
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            LoadDashboardData();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    DashboardDetail Detail = new();
    bool detailLoading = true;
    private async void LoadDashboardData()
    {
        detailLoading = true;
        var response = await _mediator.Send(new GetDashboardInfoQuery());
        detailLoading = false;
        if (response.IsSuccess) 
        {
            Detail = response.Data ?? new();
            StateHasChanged();
        }
    }
}
