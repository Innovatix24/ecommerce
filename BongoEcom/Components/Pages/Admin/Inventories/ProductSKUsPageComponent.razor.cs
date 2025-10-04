using Application.Features.Dashboard.Queries;
using Application.Features.Inventories;
using Application.Features.Products.DTOs;
using Application.Features.Products.Queries;
using BongoEcom.Components.Common;

namespace BongoEcom.Components.Pages.Admin.Inventories;

public partial class ProductSKUsPageComponent
{
    List<SKUDto> SKUs = new List<SKUDto>();
    private List<ProductDto> products = new();
    int productId = 0;
    ProductDto selectedProduct = new();
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //await LoadProducts();
            await LoadDashboardData((short)ProductId);
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    DashboardDetail Detail = new();
    bool detailLoading = true;
    private async Task LoadDashboardData(short productId)
    {
        detailLoading = true;
        var response = await _mediator.Send(new GetSKUsByProductIdQuery(productId));
        detailLoading = false;
        if (response.Succeeded)
        {
            SKUs = response.Data ?? new();
            StateHasChanged();
        }
    }

    private async Task HandleProductSelectionChanged(ProductDto product)
    {
        if (product is null) return;
        await LoadDashboardData((short)ProductId);
    }

    private async Task LoadProducts()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        if (result.IsSuccess)
        {
            products = result.Data ?? new();
        }
    }

    private void Edit(SKUDto sku)
    {

    }

    private void Remove(SKUDto sku)
    {

    }
}
