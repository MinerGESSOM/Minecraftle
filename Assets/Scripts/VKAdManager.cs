using System;
using System.Collections.Generic;
using UnityEngine;

public class VKAdManager : MonoBehaviour
{
    [Header("VK Bridge")]
    public VkBridgeController bridge;

    // Кулдаун между показами interstitial (в секундах), чтобы не спамить
    [SerializeField] private float interstitialCooldown = 90f;
    private float lastInterstitialTime = -999f;

    private const string AdFormatInterstitial = "interstitial";
    private const string AdFormatReward = "reward";
    private const string AdFormatPreloader = "preloader";
    public GameObject S;
    // ============================================================
    // INTERSTITIAL — межстраничная реклама
    // ============================================================
    /// <summary>
    /// Показать межстраничную рекламу. Вызывать между уровнями, при
    /// переходе в меню и т.п. onComplete вызовется в любом случае —
    /// и при успешном показе, и при ошибке/пропуске.
    /// </summary>
    public void ShowInterstitial(Action onComplete = null)
    {
        if (Time.time - lastInterstitialTime < interstitialCooldown)
        {
            Debug.Log("[VKAd] Interstitial on cooldown, skipping.");
            onComplete?.Invoke();
            return;
        }

        var args = new Dictionary<string, string>
        {
            { "ad_format", AdFormatInterstitial }
        };

        lastInterstitialTime = Time.time;

        bridge.Send("VKWebAppShowNativeAds", args, callback =>
        {
            Debug.Log("[VKAd] Interstitial closed. Response: " + callback);
            onComplete?.Invoke();
        });
    }

    // ============================================================
    // REWARDED — реклама за вознаграждение
    // ============================================================
    /// <summary>
    /// Показать рекламу за награду. onRewardEarned(true) — если
    /// пользователь досмотрел, false — если закрыл/ошибка.
    /// </summary>
    public void ShowRewarded(Action<bool> onRewardEarned)
    {
        var args = new Dictionary<string, string>
        {
            { "ad_format", AdFormatReward }
        };

        bridge.Send("VKWebAppShowNativeAds", args, callback =>
        {
            Debug.Log("[VKAd] Rewarded closed. Response: " + callback);

            // VK возвращает {"result":true} при успешном досмотре
            bool success = callback != null && callback.Contains("\"result\":true");

            if (success)
                Debug.Log("[VKAd] Reward granted.");
            else
                Debug.Log("[VKAd] Reward not granted.");

            onRewardEarned?.Invoke(success);
        });
    }




    public void OnLevelCompleted()
{
    // Показываем рекламу, а после её закрытия загружаем следующий уровень
    ShowInterstitial(() =>
    {
        CraftManager.Instance.StartMenuButton();
        S.SetActive(true);
    });
}
}