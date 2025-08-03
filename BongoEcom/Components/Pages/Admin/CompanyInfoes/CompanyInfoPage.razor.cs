

using Application.Features.CompanyInfoes.Queries;

namespace BongoEcom.Components.Pages.Admin.CompanyInfoes;

public partial class CompanyInfoPage
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadCompanyInfo();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    CompanyInfoDto CompanyInfo = new();
    private async Task LoadCompanyInfo()
    {
        var query = new GetCompanyInfoQuery();
        var response = await _mediator.Send(query);
        if (response.IsSuccess) 
        {
            CompanyInfo = response.Data ?? new();
        }
    }
}
