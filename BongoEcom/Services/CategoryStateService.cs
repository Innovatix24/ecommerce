namespace BongoEcom.Services;

public class CategoryStateService
{
    public int? SelectedCategoryId { get; private set; }
    public event Action<int>? OnChange;

    public void SetSelectedCategory(int categoryId)
    {
        SelectedCategoryId = categoryId;
        NotifyStateChanged(categoryId);
    }

    private void NotifyStateChanged(int categoryId) => OnChange?.Invoke(categoryId);
}
