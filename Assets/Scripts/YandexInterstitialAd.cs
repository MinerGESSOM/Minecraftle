using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexInterstitialAd : MonoBehaviour
{
    private InterstitialAdLoader interstitialAdLoader;
    private Interstitial interstitial;

    float tick = 0;
    bool active = true;

    private void Awake()
    {
        interstitialAdLoader = new InterstitialAdLoader();
        interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
        interstitialAdLoader.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
    }

    void Start()
    {
        LoadInterstitial();
    }

    void Update()
    {
        if (!active)
        {
            tick += Time.deltaTime;
            if(tick >= 120)
            {
                tick = 0;
                active = true;
            }
        }
    }
    public void Show()
    {
        if (active)
        {
            ShowInterstitial();
            active = false;
        }
    }

    public void LoadInterstitial()
    {
        // Замените на реальный AdUnitId: "R-M-XXXXXX-Y"
        string adUnitId = "R-M-20200250-2";
        AdRequestConfiguration adRequestConfiguration = 
            new AdRequestConfiguration.Builder(adUnitId).Build();
        
        interstitialAdLoader.LoadAd(adRequestConfiguration);
    }

    private void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        interstitial = args.Interstitial;
        
        // Подписка на события показа
        interstitial.OnAdShown += HandleInterstitialShown;
        interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
        interstitial.OnAdDismissed += HandleInterstitialDismissed;
        interstitial.OnAdImpression += HandleImpression;
        interstitial.OnAdClicked += HandleAdClicked;

        Debug.Log("Interstitial loaded successfully");
    }

    private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log($"Interstitial failed to load: {args.Message}");
        // НЕ пытайтесь загружать новую рекламу здесь — это может привести к блокировке
    }

    public void ShowInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("Interstitial is not ready yet");
        }
    }

    private void HandleInterstitialShown(object sender, EventArgs args)
    {
        Debug.Log("Interstitial shown");
    }

    private void HandleInterstitialFailedToShow(object sender, AdFailureEventArgs args)
    {
        Debug.Log($"Interstitial failed to show: {args.Message}");
    }

    private void HandleInterstitialDismissed(object sender, EventArgs args)
    {
        Debug.Log("Interstitial dismissed");
        DestroyInterstitial();
    }

    private void HandleImpression(object sender, ImpressionData impressionData)
    {
        Debug.Log("Impression recorded");
    }

    private void HandleAdClicked(object sender, EventArgs args)
    {
        Debug.Log("Ad clicked");
    }

    private void DestroyInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }
    }

    private void OnDestroy()
    {
        DestroyInterstitial();
    }
}