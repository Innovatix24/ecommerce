using Application.Features.Attributes.Commands;
using Application.Features.Attributes.DTOs;
using Application.Features.Attributes.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BongoEcom.Components.Pages.Admin.Attributes;

public partial class AttributeGroupPageComponent
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<AttributeGroupDto> Groups = new();
    private bool IsLoading = true;
    private string Message = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadAttributeGroupsAsync();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
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

    public async Task CreateGroup()
    {
        await OpenGroupModal("Create Group");
    }

    public async Task EditGroup(AttributeGroupDto item)
    {
        await OpenGroupModal("Edit Group", item);
    }
    private async Task DeleteGroup(short id)
    {
        var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Delete category ID {id}?");
        if (!confirmed) return;

        var result = await _mediator.Send(new DeleteAttributeGroupCommand(id));
        if (result.IsSuccess)
        {
            Message = "Attribute deleted successfully.";
            await LoadAttributeGroupsAsync();
        }
        else
        {
            Message = result.Error ?? "Failed to delete category.";
        }
    }
    public async Task OpenGroupModal(string title, AttributeGroupDto? item = null)
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
