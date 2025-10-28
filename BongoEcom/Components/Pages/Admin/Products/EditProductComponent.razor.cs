using Application.Features.Categories.DTOs;
using Application.Features.Categories.Queries;
using Application.Features.Products.Commands;
using Application.Features.Products.DTOs;
using Application.Features.Products.Queries;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Products;

public partial class EditProductComponent
{
    List<CategoryDto> _categories { get; set; } = new();

    bool _isLoading = true;
    private string shortDescription = "";
    private string longDescription = "";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            UIService.ShowLoader();

            await LoadCategories();
            await LoadProduct();

            UIService.HideLoader();

            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    ProductDto Product = new();
    
    private async Task LoadProduct()
    {
        var query = new GetProductDetailsByIdQuery((short)ProductId);
        var response = await _mediator.Send(query);
        if (response.IsSuccess) 
        {
            Product = response.Data ?? new();
            Attributes = Product.Attributes;

            Command.ProductId = Product.Id;
            Command.Name = Product.Name;
            Command.NameBangla = Product.NameBangla;
            Command.Tag = Product.Tag;
            Command.ShortDescription = Product.ShortDescription;
            Command.Description = Product.LongDescription;
            Command.SalePrice = Product.SalePrice;
            Command.RegularPrice = Product.RegularPrice;
            Command.CategoryId = Product.CategoryId;
            Command.InStock = Product.InStock;
            Command.Specifications = Product.Specifications;

            //await shortEditor.SetHtmlAsync(Product.ShortDescription);
            //await editor.SetHtmlAsync(Product.LongDescription);

            //ImageInputRef.SetImages(Product.Images);

            var category = _categories.Where(x=> x.Id == Product.CategoryId).FirstOrDefault();
            if (category != null) 
            {
                Category = category;
            }
        } 
    }

    private async Task LoadCategories()
    {
        var query = new GetAllCategoriesQuery();
        var result = await _mediator.Send(query);
        if (result.IsSuccess)
        {
            _categories = result.Data ?? new();
        }
    }

    private async Task HandleSubmit()
    {
        Command.Images = await AddImages();
        Command.CategoryId = Category.Id;
        Command.Attributes = Attributes;

        UIService.ShowLoader("Updating...");
        var result = await _mediator.Send(Command);
        UIService.HideLoader();

        if (result.IsSuccess)
        {
            await UIService.ShowSuccessAsync("Updated Successfully");
            Navigation.NavigateTo("admin/products");
        }
        else
        {
            DeleteProductImages(Command.Images);
        }
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

    List<ProductAttributeDto> Attributes = new();

    private async void DeleteAttribute(ProductAttributeDto attribute)
    {
        var confirmed = await UIService.ShowConfirmationAsync("Delete", "Are you sure to delete?");
        if (confirmed)
        {
            var command = new DeleteProductAttributeCommand(attribute.Id);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                Attributes.Remove(attribute);
                StateHasChanged();
            }
        }
    }

    private async Task AddAttribute()
    {
        await OpenAttributeModal();
    }
    private async Task EditAttribute(ProductAttributeDto item)
    {
        await OpenAttributeModal(item);
    }


    //private async Task OpenAttributeModal(ProductAttributeDto? item = null, string title = "Add Attribute")
    //{
    //    DialogParameters parameters = new()
    //    {
    //        Title = item == null? "Add attribute" : "Update attribute",
    //        PrimaryAction = "Yes",
    //        PrimaryActionEnabled = false,
    //        SecondaryAction = "No",
    //        Width = "450px",
    //        TrapFocus = false,
    //        Modal = false,
    //        PreventScroll = true
    //    };

    //    if(item is null)
    //    {
    //        item = new ProductAttributeDto()
    //        {
    //            Name = "",
    //        };
    //    }

    //    IDialogReference dialog = await dialogService.ShowDialogAsync<AddProductAttributeComponent>(item, parameters);
    //    DialogResult? result = await dialog.Result;

    //    if (result?.Data is ProductAttributeDto content)
    //    {
    //        var command = new AddProductAttributeCommand()
    //        {
    //            Id = content.Id,
    //            AttributeId = content.AttributeId,
    //            ProductId = Product.Id,
    //            Values = content.Values,
    //        };
    //        var response = await _mediator.Send(command);
    //        if (response.IsSuccess)
    //        {
    //            if(item.Id == 0)
    //            {
    //                content.Id = response.Data;
    //                Attributes.Add(content);
    //            }
    //            else
    //            {
    //                item.Values = content.Values;
    //            }
    //        }
    //    }
    //}

    private async Task OpenAttributeModal(ProductAttributeDto? item = null)
    {
        DialogParameters parameters = new()
        {
            Title = item == null ? $"Add Attribute" : "Update attribute",
            PrimaryAction = "Yes",
            PrimaryActionEnabled = false,
            SecondaryAction = "No",
            Width = "450px",
            TrapFocus = false,
            Modal = false,
            PreventScroll = true
        };

        if (item == null)
        {
            item = new ProductAttributeDto()
            {
                Name = "",
            };
        }

        IDialogReference dialog = await dialogService.ShowDialogAsync<AddProductAttributeComponent>(item, parameters);
        DialogResult? result = await dialog.Result;

        if (result?.Data is ProductAttributeDto content)
        {
            Attributes.Add(content);
        }
    }
}
