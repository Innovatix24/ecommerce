using Application.Features.Products.DTOs;
using Application.Features.Products.Queries;

namespace BongoEcom.Components.Pages.Web.Shop;

public partial class ShopPageComponent
{
    public List<ProductDto> Products = new();
    protected override Task OnInitializedAsync()
    {
        State.OnChange += OnCategoryChanged;
        return base.OnInitializedAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadProducts(0);
            
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async void OnCategoryChanged(int categoryId)
    {
        await LoadProducts(categoryId);
        StateHasChanged();
    }

    private async Task LoadProducts(int categoryId)
    {
        Products = new();
        var query = new GetAllProductsQuery(categoryId);
        var result = await _mediator.Send(query);
        if (result.IsSuccess)
        {
            Products = result.Data ?? new();
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        State.OnChange -= OnCategoryChanged;
    }
}
