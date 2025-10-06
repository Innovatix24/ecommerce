using Application.Features.Attributes.DTOs;
using Application.Features.Attributes.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Attributes;

public partial class AttributeGroupFormModal : IDialogContentComponent<AttributeGroupDto>
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public AttributeGroupDto? Content { get; set; } = new();
}
