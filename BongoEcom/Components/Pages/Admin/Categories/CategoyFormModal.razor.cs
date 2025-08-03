using Application.Features.Categories.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Categories;

public partial class CategoyFormModal : IDialogContentComponent
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public CategoryDto? Content { get; set; } = new();

    private async Task CancelAsync()
    {
        await Dialog.CloseAsync();
    }
}
