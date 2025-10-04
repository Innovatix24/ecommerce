using Application.Features.Dashboard.Queries;
using Application.Features.Inventories;
using Application.Features.Products.DTOs;
using Application.Features.Products.Queries;
using BongoEcom.Components.Common;
using BongoEcom.Components.Pages.Admin.Products;
using Microsoft.FluentUI.AspNetCore.Components;

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
            await LoadProduct();
            await LoadDashboardData((short)ProductId);
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    ProductDto product = new ProductDto();
    private async Task LoadProduct()
    {
        var response = await _mediator.Send(new GetProductDetailsByIdQuery((short)ProductId));
        if (response.IsSuccess)
        {
            product = response.Data ?? new();
        }
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

    private async Task Edit(SKUDto sku)
    {
        DialogParameters parameters = new()
        {
            Title = $"Edit SKU",
            PrimaryAction = "Yes",
            PrimaryActionEnabled = false,
            SecondaryAction = "No",
            Width = "450px",
            TrapFocus = false,
            Modal = false,
            PreventScroll = true
        };


        IDialogReference dialog = await dialogService.ShowDialogAsync<SKUEditModalComponent>(sku, parameters);
        DialogResult? result = await dialog.Result;

        if (result?.Data is SKUDto content)
        {
            //await LoadDashboardData((short)ProductId);
        }
        await LoadDashboardData((short)ProductId);
    }

    private void Remove(SKUDto sku)
    {

    }

    private async Task CreateSKUs()
    {
        var response = await _mediator.Send(new CreateProductSKUsCommand((short)ProductId));
        if (response.Succeeded)
        {
            await LoadDashboardData((short)ProductId);
        }
    }
}
