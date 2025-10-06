using Application.Features.Attributes.Commands;
using Application.Features.Attributes.DTOs;
using Application.Features.Attributes.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BongoEcom.Components.Pages.Admin.Attributes;

public partial class AttributesPage
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<AttributeDto> Attributes = new();
    private List<AttributeGroupDto> Groups = new();
    private bool IsLoading = true;
    private string Message = string.Empty;

    DataGridRowSize rowSize = DataGridRowSize.Medium;
    PaginationState pagination = new PaginationState { ItemsPerPage = 10 };

    protected override async Task OnInitializedAsync()
    {
        await LoadAttributesAsync();
        await LoadAttributeGroupsAsync();
    }

    private async Task LoadAttributesAsync()
    {
        IsLoading = true;
        var result = await _mediator.Send(new GetAttributesQuery());
        if (result.IsSuccess)
        {
            Attributes = result.Data ?? [];
        }
        else
        {
            Message = result.Error ?? "Failed to load attributes.";
        }

        IsLoading = false;
    }
    private async Task LoadAttributeGroupsAsync()
    {
        IsLoading = true;
        var result = await _mediator.Send(new GetAttributeGroups());
        if (result.IsSuccess)
        {
            Groups = result.Data ?? [];
        }
        else
        {
            Message = result.Error ?? "Failed to load groups.";
        }

        IsLoading = false;
    }
    private async Task DeleteAttribute(short id)
    {
        var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Delete category ID {id}?");
        if (!confirmed) return;

        var result = await _mediator.Send(new DeleteAttributeCommand(id));
        if (result.IsSuccess)
        {
            Message = "Attribute deleted successfully.";
            await LoadAttributesAsync();
        }
        else
        {
            Message = result.Error ?? "Failed to delete category.";
        }
    }

    public async void CreateItem()
    {
        OpenModal("Create Category");
    }

    public async void EditGroup(AttributeGroupDto item)
    {

    }
    public async void DeleteGroup(short id)
    {
        var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Delete group ID {id}?");
        if (!confirmed) return;

        var result = await _mediator.Send(new DeleteAttributeGroupCommand(id));
        if (result.IsSuccess)
        {
            Message = "Attribute Group deleted successfully.";
            await LoadAttributesAsync();
        }
        else
        {
            Message = result.Error ?? "Failed to delete Group.";
        }
    }

    public async void EditItem(AttributeDto item)
    {
        OpenModal("Edit Category", item);
    }
    public async void OpenModal(string title, AttributeDto? item = null)
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

        IDialogReference dialog = await DialogService.ShowDialogAsync<AttributeFormModal>(item, parameters);
        DialogResult? result = await dialog.Result;
        if (result.Data is not null)
        {
            await LoadAttributesAsync();
            StateHasChanged();
        }
    }

    public async void OpenGroupModal(string title, AttributeGroupDto? item = null)
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

        IDialogReference dialog = await DialogService.ShowDialogAsync<AttributeGroupFormModal>(item, parameters);
        DialogResult? result = await dialog.Result;
        if (result.Data is not null)
        {
            await LoadAttributeGroupsAsync();
            StateHasChanged();
        }
    }
}
