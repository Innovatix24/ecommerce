using Microsoft.JSInterop;

namespace BongoEcom.Services;

public class JSHelper
{
    private readonly IJSRuntime jSRuntime;

    public JSHelper(IJSRuntime jSRuntime)
    {
        this.jSRuntime = jSRuntime;
    }

    public async Task Focus(string elementId)
    {
        await jSRuntime.InvokeVoidAsync("jsfunction.focusElement", elementId);
    }
    public async Task ClearTextBoxValue(string elementId)
    {
        await jSRuntime.InvokeVoidAsync("clearTextBoxValue", elementId);
    }
    public async Task Focus(string elementId, string value)
    {
        await jSRuntime.InvokeVoidAsync("testFunctions.focusToSearchTextBox", elementId, value);
    }

    public async Task<ComponentLocation> GetComponentLocation(string componentId)
    {
        var result = await jSRuntime.InvokeAsync<ComponentLocation>("testFunctions.getComponentLocation", componentId);
        return result;
    }
    public async Task<object?> GetComponentLocation2(string componentId)
    {
        var result = await jSRuntime.InvokeAsync<object?>("testFunctions.getComponentLocation", componentId);
        return result;
    }

    public async Task SetText(string componentId, object value)
    {
        await jSRuntime.InvokeVoidAsync("setText", componentId, value.ToString());
    }
}
