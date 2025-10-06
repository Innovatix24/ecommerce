using Application.Features.Attributes.DTOs;
using Application.Features.Attributes.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Attributes;

public partial class AttributeFormModal : IDialogContentComponent<AttributeDto>
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public AttributeDto? Content { get; set; } = new();


    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadAttributeGroupsAsync();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    bool IsLoading = false;
    List<AttributeGroupDto> Groups = new List<AttributeGroupDto>();
    private async Task LoadAttributeGroupsAsync()
    {
        IsLoading = true;
        var result = await _mediator.Send(new GetAttributeGroups());
        if (result.IsSuccess)
        {
            Groups = result.Data ?? [];
            if(Content is not null)
            {
                group = Groups.FirstOrDefault(x => x.Id == Content.GroupId) ?? new();
            }
        }
        IsLoading = false;
    }

    AttributeGroupDto group = new();
    private void SelectAttributeGroup(AttributeGroupDto item)
    {
        if (item is null) return;
        group = item;
        if (Content != null)
        {
            Content.GroupId = item.Id;
        }
    }
}
