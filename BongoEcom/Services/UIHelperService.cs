using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Icons =  Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace BongoEcom.Services;

public class UIHelperService
{
    IDialogService DialogService;
    IJSRuntime JS;
    IWebHostEnvironment _env;
    public UIHelperService(IDialogService dialogService, IJSRuntime js, IWebHostEnvironment env)
    {
        this.DialogService = dialogService;
        JS = js;
        _env = env;
    }

    public async Task ShowSuccessAsync(string message)
    {
        var dialog = await DialogService.ShowSuccessAsync(message, "Success", "Ok");
        var result = await dialog.Result;
    }

    public async Task ShowWarningAsync(string warning)
    {
        var dialog = await DialogService.ShowWarningAsync(warning, null, "Stop!");
        var result = await dialog.Result;
    }

    public async Task ShowErrorAsync(string message)
    {
        var dialog = await DialogService.ShowErrorAsync(message, null, "Ok");
        var result = await dialog.Result;
    }

    public async Task ShowInformationAsync()
    {
        var dialog = await DialogService.ShowInfoAsync("This is a message", null, "Confirm");
        var result = await dialog.Result;
    }

    public async Task<bool> ShowConfirmationAsync(string title = "Eyes on you", string message = "Are you sure?")
    {
        var dialog = await DialogService.ShowConfirmationAsync(message, "Yes", "No", title);
        var result = await dialog.Result;
        return !result.Cancelled;
    }

    public async Task ShowMessageBoxLongAsync()
    {
        var dialog = await DialogService.ShowInfoAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", null, "Excepteur");
        var result = await dialog.Result;
    }

    public async Task ShowMessageBoxAsync(string title = "My title", string message = "Hello World")
    {
        var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
        {
            Content = new()
            {
                Title = title,
                MarkupMessage = new MarkupString(message),
                Icon = new Icons.Regular.Size24.Games(),
                IconColor = Color.Success,
            },
            PrimaryAction = "OK",
            SecondaryAction = "Cancel",
            Width = "300px",
        });
        var result = await dialog.Result;
    }

    public async void ShowLoader(string message = "Loading...")
    {
        await JS.InvokeVoidAsync("loader.show", message);
    }

    public async void HideLoader()
    {
        await JS.InvokeVoidAsync("loader.hide");
    }

    public async void SuccessToastMessage(string message = "")
    {
        await JS.InvokeVoidAsync("toaster.showSuccess", message);
    }

    public async void ErrorToastMessage(string message = "")
    {
        await JS.InvokeVoidAsync("toaster.showError", message);
    }

    public async Task SweetConfirm(string message = "")
    {
        await JS.InvokeVoidAsync("toaster.showError", message);
    }

    public byte[] GetImageBytes(string relativePath)
    {
        var cleanPath = relativePath.TrimStart('~', '/');
        var physicalPath = Path.Combine(_env.WebRootPath, cleanPath);
        var bytes = File.ReadAllBytes(physicalPath);
        return bytes;
    }
}
