using Application.Features.Attributes.Commands;
using Application.Features.Attributes.DTOs;
using Application.Features.Attributes.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BongoEcom.Components.Pages.Admin.Attributes;

public partial class AttributePageComponent
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<AttributeDto> Attributes = new();
    private List<AttributeDto> FilteredAttributes = new();
    private List<AttributeGroupDto> Groups = new();
    private bool IsLoading = true;
    private string Message = string.Empty;

    protected override async Task OnInitializedAsync()
    {

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadAttributesAsync();
            await LoadAttributeGroupsAsync();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadAttributesAsync()
    {
        IsLoading = true;
        var result = await _mediator.Send(new GetAttributesQuery());
        if (result.IsSuccess)
        {
            Attributes = result.Data ?? [];

            if (group.Id == 0)
            {
                FilteredAttributes = Attributes;
            }
            else
            {
                FilteredAttributes = Attributes.Where(x => x.GroupId == group.Id).ToList();
            }
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

    private void Create() => Navigation.NavigateTo($"/admin/attribute/form");
    private void Edit(int id) => Navigation.NavigateTo($"/admin/attribute/form/{id}");

    public async Task CreateAttribute()
    {
        await OpenModal("Create Attribute");
    }

    public async Task CreateGroup()
    {
        await OpenGroupModal("Create Group");
    }

    public async Task EditGroup(AttributeGroupDto item)
    {
        await OpenGroupModal("Edit Group", item);
    }
    public async Task DeleteGroup(short id)
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
        await OpenModal("Edit Attribute", item);
    }
    public async Task OpenModal(string title, AttributeDto? item = null)
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

    AttributeGroupDto group = new();
    private void SelectAttributeGroup(AttributeGroupDto item)
    {
        if (item is null) return;
        group = item;
        if(item.Id == 0)
        {
            FilteredAttributes = Attributes;
        }
        else
        {
            FilteredAttributes = Attributes.Where(x => x.GroupId == item.Id).ToList();
        }  
    }
}
