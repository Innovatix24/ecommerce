using Application.Features.Site.Banners;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace BongoEcom.Components.Pages.Web.Home;

public partial class BannerCarousel : IAsyncDisposable
{
    [Parameter]
    public List<BannerDto> Banners { get; set; } = new();

    [Parameter]
    public bool AutoPlay { get; set; } = true;

    [Parameter]
    public int AutoPlayInterval { get; set; } = 5000; // 5 seconds

    [Parameter]
    public bool ShowArrows { get; set; } = true;

    [Parameter]
    public bool ShowIndicators { get; set; } = true;

    [Parameter]
    public bool ShowPlayPause { get; set; } = true;

    [Parameter]
    public string Height { get; set; } = "400px";

    private ElementReference carouselRef;
    private BannerDto CurrentBanner => Banners.Count > 0 ? Banners[CurrentIndex] : null;
    private int CurrentIndex { get; set; } = 0;
    private Timer autoPlayTimer;
    private bool IsAutoPlaying { get; set; }
    private DotNetObjectReference<BannerCarousel> dotNetHelper;

    protected override void OnInitialized()
    {
        dotNetHelper = DotNetObjectReference.Create(this);

        if (AutoPlay && Banners.Count > 1)
        {
            StartAutoPlay();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("bannerCarousel.init", carouselRef, dotNetHelper);
        }
    }

    private void StartAutoPlay()
    {
        autoPlayTimer?.Dispose();

        autoPlayTimer = new Timer(AutoPlayInterval);
        autoPlayTimer.Elapsed += (sender, e) => InvokeAsync(NextBanner);
        autoPlayTimer.AutoReset = true;
        autoPlayTimer.Start();

        IsAutoPlaying = true;
    }

    private void StopAutoPlay()
    {
        autoPlayTimer?.Stop();
        IsAutoPlaying = false;
    }

    private void ToggleAutoPlay()
    {
        if (IsAutoPlaying)
        {
            StopAutoPlay();
        }
        else
        {
            StartAutoPlay();
        }
    }

    public void NextBanner()
    {
        if (Banners.Count == 0) return;

        CurrentIndex = (CurrentIndex + 1) % Banners.Count;
        StateHasChanged();
    }

    public void PrevBanner()
    {
        if (Banners.Count == 0) return;

        CurrentIndex = (CurrentIndex - 1 + Banners.Count) % Banners.Count;
        StateHasChanged();
    }

    public void GoToBanner(int index)
    {
        if (index >= 0 && index < Banners.Count)
        {
            CurrentIndex = index;
            StateHasChanged();
        }
    }

    [JSInvokable]
    public void HandleSwipe(string direction)
    {
        if (direction == "left")
        {
            NextBanner();
        }
        else if (direction == "right")
        {
            PrevBanner();
        }
    }

    public async ValueTask DisposeAsync()
    {
        autoPlayTimer?.Dispose();
        dotNetHelper?.Dispose();

        try
        {
            await JSRuntime.InvokeVoidAsync("bannerCarousel.dispose", carouselRef);
        }
        catch
        {
            // JavaScript might be unavailable during disposal
        }
    }
}