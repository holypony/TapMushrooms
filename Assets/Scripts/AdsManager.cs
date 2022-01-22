using System;
using UnityEngine;
using GoogleMobileAds.Api;
public class AdsManager : MonoBehaviour
{
    
    private InterstitialAd interstitial;
    [SerializeField] private bool TestAds = true;
    
    
    public static AdsManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject); 
    }
    
    public void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }
    
    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string TESTadUnitId = "ca-app-pub-3940256099942544/1033173712";
        string adUnitId = "ca-app-pub-5214300113420107/1956980315";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        this.interstitial = TestAds ? new InterstitialAd(TESTadUnitId) : new InterstitialAd(adUnitId);
        
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        GameManager.instance.StartGame();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("Interstitial failed to load: ");
        // Handle the ad failed to load event.
    }

    public void ShowInter()
    {
        if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        }
        else
        {
            GameManager.instance.StartGame();
        }
    }
    
}

