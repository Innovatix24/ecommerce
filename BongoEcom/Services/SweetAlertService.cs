
using Microsoft.JSInterop;

namespace BongoEcom.Services;
public class SweetAlertService
{
    private readonly IJSRuntime _jsRuntime;

    public SweetAlertService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ShowSuccessAsync(string message)
    {
        await _jsRuntime.InvokeVoidAsync("toaster.showSuccess", message);
    }
    public async Task ShowErrorAsync(string message)
    {
        await _jsRuntime.InvokeVoidAsync("toaster.showError", message);
    }

    public async Task<bool> ConfirmAsync(string title, string text, string confirmText = "Confirm")
    {
        return await _jsRuntime.InvokeAsync<bool>("sweet.confirm", title, text, confirmText);
    }
}