using Application.Features.Categories.Commands;
using Application.Features.Categories.DTOs;
using Application.Features.Categories.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BongoEcom.Components.Pages.Admin.Categories;

public partial class CategoryPageComponent
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<CategoryDto> Categories = new();
    private bool IsLoading = true;
    private string Message = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadCategoriesAsync();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadCategoriesAsync()
    {
        IsLoading = true;
        StateHasChanged();
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        if (result.IsSuccess)
        {
            Categories = result.Data ?? [];
        }
        else
        {
            Message = result.Error ?? "Failed to load categories.";
        }

        IsLoading = false;
        StateHasChanged();
    }

    private async Task DeleteCategory(short id)
    {
        var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Delete category ID {id}?");
        if (!confirmed) return;

        var result = await _mediator.Send(new DeleteCategoryCommand(id));
        if (result.IsSuccess)
        {
            Message = "Category deleted successfully.";
            await LoadCategoriesAsync();
        }
        else
        {
            Message = result.Error ?? "Failed to delete category.";
        }
    }

    private void EditCategory(short id)
    {
        Navigation.NavigateTo($"categories/create/{id}");
    }


    public async void CreateCategory()
    {
        OpenModal("Create Category");
    }

    public async void EditCategory(CategoryDto category)
    {
        OpenModal("Edit Category", category);
    }
    public async void OpenModal(string title, CategoryDto? category = null)
    {
        DialogParameters parameters = new()
        {
            Title = title,
            PrimaryAction = "Yes",
            PrimaryActionEnabled = false,
            SecondaryAction = "No",
            Width = "450px",
            TrapFocus = false,
            Modal = false,
            PreventScroll = true
        };

        IDialogReference dialog = await DialogService.ShowDialogAsync<CategoyFormModal>(category, parameters);
        DialogResult? result = await dialog.Result;
        if (result.Data is not null)
        {
            await LoadCategoriesAsync();
        }
    }
}
