namespace BongoEcom.Services;

public class ProductFilterService
{
    public event Action<string>? SearchHandler;

    public void HandleSearch(string stringKey)
    {
        NotifyHandleSearch(stringKey);
    }

    private void NotifyHandleSearch(string stringKey) => SearchHandler?.Invoke(stringKey);
}
