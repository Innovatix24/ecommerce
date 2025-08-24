using Application.Features.CompanyInfoes.Queries;
using Application.Features.Site.Banners;

namespace BongoEcom.Components.Pages.Admin.Site.Banners;

public partial class BannersPage
{
    private List<BannerDto> banners = new List<BannerDto>();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadData();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    CompanyInfoDto CompanyInfo = new();
    private async Task LoadData()
    {
        var query = new GetAllBannersQuery();
        var response = await _mediator.Send(query);
        if (response.IsSuccess)
        {
            banners = response.Data ?? new();
        }
    }


    private void Edit(int id) => Navigation.NavigateTo($"admin/site/banners/edit/{id}");

    private async Task Delete(BannerDto banner)
    {
        var confirmed = await UIService.ShowConfirmationAsync("Delete", "Are you <b>sure</b> to delete?");

        if (confirmed)
        {
            var response = await _mediator.Send(new DeleteBannerCommand((byte)banner.Id));
            if (response.IsSuccess)
            {
                DeleteImage(banner.ImageUrl);
                await UIService.ShowSuccessAsync("Banner has been deleted");
                await LoadData();
            }
        }
    }
}
