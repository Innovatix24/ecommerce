using Application.Features.Attributes.Queries;
using Application.Features.Products.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Products;

public partial class AddProductAttributeComponent : IDialogContentComponent
{
    private string Name = "";
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public ProductAttributeDto? Content { get; set; }

    protected override async Task OnInitializedAsync()
    {
        
    }

    bool IsLoading = false;
    private async Task LoadAttributesAsync()
    {
        IsLoading = true;
        var result = await _mediator.Send(new GetAttributesQuery());
        if (result.IsSuccess)
        {
            Attributes = result.Data ?? [];
        }

        IsLoading = false;
    }

    List<AttributeGroupDto> Groups = new List<AttributeGroupDto>();
    private async Task LoadAttributeGroupsAsync()
    {
        IsLoading = true;
        var result = await _mediator.Send(new GetAttributeGroups());
        if (result.IsSuccess)
        {
            Groups = result.Data ?? [];
        }
        IsLoading = false;
    }

    AttributeGroupDto group = new();
    private void SelectAttributeGroup(AttributeGroupDto item)
    {
        group = item;
        FilteredAttributes = Attributes.Where(x=> x.GroupId == item.Id).ToList();
    }

    public bool? AreAllTypesVisible { get; set; }
}
