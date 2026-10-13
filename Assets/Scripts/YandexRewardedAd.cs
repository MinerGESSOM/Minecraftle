using System;
using TMPro;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexRewardedAd : MonoBehaviour
{
    private RewardedAdLoader rewardedAdLoader;
    private RewardedAd rewardedAd;

    [SerializeField] AdvBonus AdvBonus;

    public int Mn = 1;

    private void Awake()
    {
        rewardedAdLoader = new RewardedAdLoader();
        rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
        rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    void Start()
    {
        LoadRewardedAd();
    }

    public void Show()
    {
        ShowRewardedAd();
    }


    public void LoadRewardedAd()
    {
        // Замените на реальный AdUnitId: "R-M-XXXXXX-Y"
        string adUnitId = "R-M-20200250-3";
        AdRequestConfiguration adRequestConfiguration = 
            new AdRequestConfiguration.Builder(adUnitId).Build();
        
        rewardedAdLoader.LoadAd(adRequestConfiguration);
    }

    private void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
    {
        rewardedAd = args.RewardedAd;
        
        rewardedAd.OnAdShown += HandleAdShown;
        rewardedAd.OnAdFailedToShow += HandleAdFailedToShow;
        rewardedAd.OnAdDismissed += HandleAdDismissed;
        rewardedAd.OnAdImpression += HandleImpression;
        rewardedAd.OnAdClicked += HandleAdClicked;
        rewardedAd.OnRewarded += HandleRewarded;

        Debug.Log("Rewarded ad loaded successfully");
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log($"Rewarded ad failed to load: {args.Message}");
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad is not ready");
        }
    }

    private void HandleAdShown(object sender, EventArgs args)
    {
        Debug.Log("Rewarded ad shown");
    }

    private void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        Debug.Log($"Rewarded ad failed to show: {args.Message}");
    }

    private void HandleAdDismissed(object sender, EventArgs args)
    {
        Debug.Log("Rewarded ad dismissed");
        DestroyRewardedAd();
    }

    private void HandleImpression(object sender, ImpressionData impressionData)
    {
        Debug.Log("Impression recorded");
    }

    private void HandleAdClicked(object sender, EventArgs args)
    {
        Debug.Log("Ad clicked");
    }

    private void HandleRewarded(object sender, Reward args)
    {
        // Пользователь просмотрел рекламу и заслужил награду
        // args.type — тип награды, args.amount — количество
        Debug.Log($"User rewarded: {args.amount} {args.type}");
        // Выдайте награду здесь

        CraftManager.Instance.MnX = Mn;
        if(Mn == 2)
        {
            AdvBonus.tickX2 = 180f;
            AdvBonus.ActiveX2 = true;
        }
        else
        {
            AdvBonus.tickX3 = 120f;
            AdvBonus.ActiveX3 = true;
        }
    }

    private void DestroyRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
    }

    private void OnDestroy()
    {
        DestroyRewardedAd();
    }
}