using UnityEngine;
using UnityEngine.UI;
using CAS;
using System.Collections.Generic;
using CAS.UserConsent;

public class AdsManager : MonoBehaviour
{
    IMediationManager manager;
    
    public ConsentStatus userConsent;
    public CCPAStatus userCCPAStatus;
    
    void OnEnable ()
    {

        InitAdsManager();
        // Called when the ad is displayed.
        manager.OnInterstitialAdShown += () => Debug.Log("Interstitial shown");
        // The same call as the `OnInterstitialAdShown` but with `AdMetaData` about the impression. 
        manager.OnInterstitialAdOpening += (data) => Debug.Log("Interstitial Ad " + data.ToString());
        // Called when the ad is failed to display.
        manager.OnInterstitialAdFailedToShow += (error) => Debug.LogError(error);
        // Called when the user clicks on the Ad.
        manager.OnInterstitialAdClicked += () => Debug.Log("Interstitial clicked");
        // Called when the ad is closed.
        manager.OnInterstitialAdClosed += OnInterClosed;
    }


    void OnInterClosed()
    {
        Debug.Log("Interstitial closed");
        GameManager.instance.StartGame();
    }

    void InitAdsManager()
    {
        // -- Privacy Laws (Optional):
        MobileAds.settings.userConsent = userConsent;
        MobileAds.settings.userCCPAStatus = userCCPAStatus;
        manager = MobileAds.BuildManager()
            .WithManagerIdAtIndex(0)
            .WithInitListener((success, error) =>
            {
                // CAS manager initialization done
            })
            .Initialize();
    }


    public void LoadInter()
    {
        manager.LoadAd(AdType.Interstitial);
    }

    public void ShowInter()
    {
        if (manager.IsReadyAd(AdType.Interstitial))
        {
            manager.ShowAd(AdType.Interstitial);
            int i = PlayerPrefs.GetInt("adsShown", 0);
            PlayerPrefs.SetInt("adsShown", i + 1);
            FirebaseAnalytics.instance.UpdateAdsShown(i);
        }
        else
        {
            GameManager.instance.StartGame();
        }
    }
}
