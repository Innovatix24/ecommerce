
using Application.Features.Attributes.Commands;
using Application.Features.Attributes.DTOs;
using Application.Features.Attributes.Queries;
using Microsoft.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Attributes;

public partial class AttributeFormPageComponent
{
    [Parameter] public int AttributeId { get; set; }

    public AttributeDto Content { get; set; } = new();

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadAttributeDetails(AttributeId);
            await LoadAttributeGroupsAsync();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadAttributeDetails(int AttributeId)
    {
        var response = await _mediator.Send(new GetAttributeDetailsByIdQuery((short)AttributeId));
        if(response.IsSuccess)
        {
            Content = response.Data ?? new();
        }
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
            if (Content is not null)
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

    private async Task HandleValidSubmit()
    {
        

        if(Content.Id == 0)
        {
            var command = new CreateAttributeCommand
            {
                GroupId = group.Id,
                Name = Content.Name,
                Values = Content.Values
            };
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                await UIService.ShowSuccessAsync("Saved successfully");
            }
        }
        else
        {
            var command = new UpdateAttributeCommand
            {
                Id = Content.Id,
                GroupId = group.Id,
                Name = Content.Name,
                Values = Content.Values
            };
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                await UIService.ShowSuccessAsync("Updated successfully");
            }
        }
    }
}
