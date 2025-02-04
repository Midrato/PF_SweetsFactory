using UnityEngine;
//using UnityEngine.Advertisements;

/*
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener
{
    static public AdsInitializer inst;
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    private string adAndroidRewardId = "Rewarded_Android";
    private string adIOSRewardId = "Rewarded_IOS";
    public string adRewardId {get;private set;}

    public bool adLoaded {get; private set;} = false;


    void Awake()
    {
        // 1つしか要らないからシングルトン
        if(inst == null){
            inst = this;
            InitializeAds();
        }else{
            Destroy(this);
        }
        
    }

    public void InitializeAds()
    {
    #if UNITY_IOS
            _gameId = _iOSGameId;
            adRewardId = adIOSRewardId;
    #elif UNITY_ANDROID
            _gameId = _androidGameId;
            adRewardId = adAndroidRewardId;
    #elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
            adRewardId = adAndroidRewardId;
    #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        Advertisement.Load(adRewardId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        adLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Unity Ads Load Failed: {error.ToString()} - {message}");
    }
}
*/