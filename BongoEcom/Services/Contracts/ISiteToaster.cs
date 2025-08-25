using Blazored.Toast.Services;
using System.Collections.Concurrent;

namespace BongoEcom.Services.Contracts;

public class ToastMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public ToastLevel Level { get; set; } = ToastLevel.Info;
    public DateTime Timestamp { get; } = DateTime.Now;
    public int Duration { get; set; } = 3000;
    public bool IsVisible { get; set; } = true;
}

public enum ToastLevel
{
    Info,
    Success,
    Warning,
    Error
}

public interface ISiteToaster
{
    event Action<ToastMessage> OnToastAdded;
    event Action OnToastsUpdated;

    void ShowInfo(string message, string title = "Information");
    void ShowSuccess(string message, string title = "Success");
    void ShowWarning(string message, string title = "Warning");
    void ShowError(string message, string title = "Error");
    void RemoveToast(Guid toastId);
    void ClearAll();
    IEnumerable<ToastMessage> GetToasts();
}

public class SiteToaster : ISiteToaster
{
    private readonly ConcurrentDictionary<Guid, ToastMessage> _toasts = new();
    private readonly int _maxToasts = 5;
    private readonly List<Action> _onToastsUpdatedCallbacks = new();

    public event Action<ToastMessage> OnToastAdded;
    public event Action OnToastsUpdated;

    // Method to safely invoke events on UI thread
    private async void SafeInvokeToastsUpdated()
    {
        if (OnToastsUpdated != null)
        {
            foreach (var handler in OnToastsUpdated.GetInvocationList().Cast<Action>())
            {
                try
                {
                    handler.Invoke();
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("Dispatcher"))
                {
                    // If we're not on the UI thread, this will fail gracefully
                    // The component should handle this by using InvokeAsync
                }
            }
        }
    }

    public void ShowInfo(string message, string title = "Information")
    {
        ShowToast(message, title, ToastLevel.Info);
    }

    public void ShowSuccess(string message, string title = "Success")
    {
        ShowToast(message, title, ToastLevel.Success);
    }

    public void ShowWarning(string message, string title = "Warning")
    {
        ShowToast(message, title, ToastLevel.Warning);
    }

    public void ShowError(string message, string title = "Error")
    {
        ShowToast(message, title, ToastLevel.Error);
    }

    private void ShowToast(string message, string title, ToastLevel level)
    {
        var toast = new ToastMessage
        {
            Title = title,
            Message = message,
            Level = level
        };

        // Remove oldest toast if we've reached the maximum
        if (_toasts.Count >= _maxToasts)
        {
            var oldestToast = _toasts.Values.OrderBy(t => t.Timestamp).First();
            _toasts.TryRemove(oldestToast.Id, out _);
        }

        _toasts[toast.Id] = toast;
        OnToastAdded?.Invoke(toast);
        SafeInvokeToastsUpdated();
    }

    public void RemoveToast(Guid toastId)
    {
        _toasts.TryRemove(toastId, out _);
        SafeInvokeToastsUpdated();
    }

    public void ClearAll()
    {
        _toasts.Clear();
        SafeInvokeToastsUpdated();
    }

    public IEnumerable<ToastMessage> GetToasts()
    {
        return _toasts.Values.OrderByDescending(t => t.Timestamp);
    }

    public void Dispose()
    {
        _toasts.Clear();
    }
}