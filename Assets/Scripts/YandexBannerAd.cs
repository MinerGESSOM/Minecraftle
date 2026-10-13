using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;
using System;

public class YandexBannerAd : MonoBehaviour
{
    private Banner banner;


    void Start()
    {
        RequestStickyBanner();
    }
    private int GetScreenWidthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }

    public void RequestStickyBanner()
    {
        // Замените на реальный AdUnitId: "R-M-XXXXXX-Y"
        string adUnitId = "R-M-20200250-1";

        BannerAdSize bannerMaxSize = BannerAdSize.StickySize(GetScreenWidthDp());
        banner = new Banner(adUnitId, bannerMaxSize, AdPosition.BottomCenter);

        banner.OnAdLoaded += HandleAdLoaded;
        banner.OnAdFailedToLoad += HandleAdFailedToLoad;

        AdRequest request = new AdRequest.Builder().Build();
        banner.LoadAd(request);
    }

    private void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Banner loaded successfully");
    }

    private void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        Debug.Log($"Banner failed to load: {args.Message}");
    }

    private void OnDestroy()
    {
        if (banner != null)
        {
            banner.Destroy();
            banner = null;
        }
    }
}