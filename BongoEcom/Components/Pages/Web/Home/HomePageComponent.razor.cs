using Application.Features.Products.DTOs;
using Application.Features.Products.Queries;
namespace BongoEcom.Components.Pages.Web.Home;

public partial class HomePageComponent
{
    bool productLoading = false;

    public List<ProductDto> Products = new();

    protected override Task OnInitializedAsync()
    {
        State.OnChange += OnCategoryChanged;
        FilterService.SearchHandler += HandleProductSearch;
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

    int selectedCategoryId = 0;
    private async void OnCategoryChanged(int categoryId)
    {
        selectedCategoryId = categoryId;
        await LoadProducts(categoryId);
        StateHasChanged();
    }

    private async void HandleProductSearch(string searchKey)
    {
        await LoadProducts(0, searchKey, 1);
        //productLoading = true;
        //StateHasChanged();

        //Products = new();
        //var query = new GetFilterProductsQuery(searchKey);
        //var result = await _mediator.Send(query);
        //if (result.IsSuccess)
        //{
        //    Products = result.Data ?? new();
        //}

        //productLoading = false;
        //StateHasChanged();
    }

    PaginatedResult<ProductDto>? PaginatedResult;

    private async Task LoadProducts(int categoryId, string filter = "", int pageNumber = 1)
    {
        if (productLoading) return;

        var query = new GetPaginatedProductsQuery()
        {
            PageNumber = pageNumber,
            PageSize = 5,
            CategoryId = categoryId,
            SearchTerm = filter
        };
        UIService.ShowLoader();
        productLoading = true;
        var result = await _mediator.Send(query);
        productLoading = false;
        if (result.IsSuccess)
        {
            PaginatedResult = result.Data;
            if (PaginatedResult is not null)
            {
                Products = PaginatedResult.Items;
            }
        }
        UIService.HideLoader();
        StateHasChanged();
    }

    private async void HandlePageChanged(int pageNumber)
    {
        await LoadProducts(selectedCategoryId, pageNumber : pageNumber);
    }

    public void Dispose()
    {
        State.OnChange -= OnCategoryChanged;
    }
}
