namespace BongoEcom.Services;

public class ProductFilterService
{
    public event Func<string, Task>? SearchHandler;

    public async Task HandleSearch(string stringKey)
    {
        await NotifyHandleSearch(stringKey);
    }

    private async Task NotifyHandleSearch(string stringKey)
    {
        await SearchHandler?.Invoke(stringKey);
    }
}
