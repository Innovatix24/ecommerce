using Application.Features.Site.Banners;

namespace BongoEcom.Components.Pages.Admin.Site.Banners;
public partial class BannerFormComponent
{
    BannerDto banner { get; set; } = new BannerDto();

    private async Task HandleSubmit()
    {
        var error = Validate();
        if (!string.IsNullOrEmpty(error))
        {
            tostService.ShowError(error);
            return;
        }

        var command = new AddEditBannerCommand
        {
            Id = (byte)BannerId,
            Title = banner.Title,
            Description = banner.Description,
            ImageUrl = await GetImageUrl(),
            TargetUrl = banner.TargetUrl,
            DisplayOrder = banner.DisplayOrder,
            IsActive = banner.IsActive,
        };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {

        }
    }

    private string? Validate()
    {
        if (string.IsNullOrEmpty(banner.Title))
        {
            return "Title is required";
        }
        if (uploadedImages.Count == 0)
        {
            return "Image is required is required";
        }
        return null;
    }

    private async Task<string> GetImageUrl()
    {
        if (uploadedImages.Count == 0) return "";

        var item = uploadedImages.First();
        var extension = Path.GetExtension(item.Name);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var relativePath = Path.Combine("images", fileName);
        var absolutePath = Path.Combine(env.WebRootPath, relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

        using (var fileStream = new FileStream(absolutePath, FileMode.Create))
        {
            await item.OpenReadStream(maxAllowedSize: 10_000_000).CopyToAsync(fileStream);
        }

        return $"/{relativePath.Replace("\\", "/")}";
    }
}
