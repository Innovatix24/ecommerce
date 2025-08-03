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
}
