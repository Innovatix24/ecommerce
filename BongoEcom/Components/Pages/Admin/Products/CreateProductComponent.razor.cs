using Application.Features.Categories.DTOs;
using Application.Features.Categories.Queries;
using Application.Features.Products.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Products;

public partial class CreateProductComponent
{
    List<CategoryDto> _categories { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadCategories();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadCategories() 
    {
        var query = new GetAllCategoriesQuery();
        var result = await _mediator.Send(query);
        if(result.IsSuccess)
        {
            _categories = result.Data ?? new();
        }
    }
    private string? Validate()
    {
        if (string.IsNullOrEmpty(Command.Name))
        {
            return "Name is required";
        }
        if (Command.RegularPrice == 0)
        {
            return "Regular price is required";
        }
        if (Command.SalePrice == 0)
        {
            return "Sale price is required";
        }
        return null;
    }

    private async Task HandleSubmit()
    {
        var error = Validate();

        if (!string.IsNullOrEmpty(error))
        {
            //await UIService.ShowErrorAsync(error);
            toastService.ShowError(error);
            return;
        }

        UIService.ShowLoader("Saving...");

        Command.Attributes = Attributes;
        Command.Images = await AddImages();
        Command.CategoryId = Category.Id;
        Command.ShortDescription = await shortEditor.GetHtmlAsync();
        Command.Description = await editor.GetHtmlAsync();

        var result = await _mediator.Send(Command);
        if (result.IsSuccess)
        {
            UIService.HideLoader();
            Navigation.NavigateTo("admin/products");
        }
        else
        {
            DeleteProductImages(Command.Images);
        }
        UIService.HideLoader();
    }

    private async Task<List<ProductImageDto>> AddImages()
    {
        List<ProductImageDto> productImages = new List<ProductImageDto>();
        foreach (var item in uploadedImages)
        {
            var extension = Path.GetExtension(item.Name);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var relativePath = Path.Combine("images", fileName);
            var absolutePath = Path.Combine(env.WebRootPath, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

            using (var fileStream = new FileStream(absolutePath, FileMode.Create))
            {
                await item.OpenReadStream(maxAllowedSize: 10_000_000).CopyToAsync(fileStream);
            }

            productImages.Add(new ProductImageDto
            {
                Url = $"/{relativePath.Replace("\\", "/")}", // For web use
                Tag = "",  
                DisplayOrder = (byte)(uploadedImages.IndexOf(item) + 1),
                IsPrimary = false
            });
        }
        return productImages;
    }

    public void DeleteProductImages(List<ProductImageDto> productImages)
    {
        foreach (var image in productImages)
        {
            if (string.IsNullOrWhiteSpace(image.Url))
                continue;

            string relativePath = image.Url.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            string fullPath = Path.Combine(env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }

    private async Task OpenAddAttributeModal()
    {
        DialogParameters parameters = new()
        {
            Title = $"Add Attribute",
            PrimaryAction = "Yes",
            PrimaryActionEnabled = false,
            SecondaryAction = "No",
            Width = "450px",
            TrapFocus = false,
            Modal = false,
            PreventScroll = true
        };

        var item = new ProductAttributeDto()
        {
            Name = "",
        };

        IDialogReference dialog = await dialogService.ShowDialogAsync<AddProductAttributeComponent>(item, parameters);
        DialogResult? result = await dialog.Result;

        if (result?.Data is ProductAttributeDto content)
        {
            Attributes.Add(content);
        }
    }
}
