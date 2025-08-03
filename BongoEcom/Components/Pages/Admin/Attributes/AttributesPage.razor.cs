using Application.Features.Attributes.Commands;
using Application.Features.Attributes.DTOs;
using Application.Features.Attributes.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BongoEcom.Components.Pages.Admin.Attributes;

public partial class AttributesPage
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<AttributeDto> Attributes = new();
    private bool IsLoading = true;
    private string Message = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadAttributesAsync();
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
            Message = result.Error ?? "Failed to load categories.";
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

    private void EditAttribute(short id)
    {
        Message = $"Edit logic for category ID {id} would go here.";
    }
}
