using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidGameId;
    [SerializeField] string iOSGameId;

    [SerializeField] bool testMode = true;

    [SerializeField] string androidAdUnitId;
    [SerializeField] string iOSAdUnitId;

    public Canvas rewardCanvas;

    string gameId;
    string adUnitId;

    public static AdManager Instance;

    void Start()
    {
        rewardCanvas.enabled = false;
    }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InitaliseAds();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void InitaliseAds()
    {
#if UNITY_IOS
        gameId = iOSGameId;
        adUnitId = iOSAdUnitId;
#elif UNITY_ANDROID
        gameId = androidGameId;
        adUnitId = androidAdUnitId;
#elif UNITY_EDITOR
        gameId = androidGameId;
        adUnitId = androidAdUnitId;
#endif

        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }

    }
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        // Display Ad
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("OnUnityAdsShowComplete called. Placement ID: " + placementId + ", Completion State: " + showCompletionState);
        // Player earns credits when ad has finished
        GameManager.Instance.AddRewardCredits();
        // Reward canvas text is displayed
        rewardCanvas.enabled = true;
        Invoke("DisableRewardsCanvas", 5f);
    }

    public void ShowAd()
    {
        Advertisement.Load(adUnitId, this);
    }

    void DisableRewardsCanvas()
    {
        // Reward canvas text is disabled
        rewardCanvas.enabled = false;
    }
}
