using Application.Features.Attributes.DTOs;
using Application.Features.Categories.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Attributes;

public partial class AttributeFormModal : IDialogContentComponent<AttributeDto>
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public AttributeDto? Content { get; set; } = new();
}
