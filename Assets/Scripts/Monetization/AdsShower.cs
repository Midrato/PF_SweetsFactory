using UnityEngine;
//using UnityEngine.Advertisements;
/*
public class AdsShower : MonoBehaviour, IUnityAdsShowListener
{
    public void ShowAd(){
        if(AdsInitializer.inst.adLoaded){
            Advertisement.Show(AdsInitializer.inst.adRewardId, this);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("広告見せエラー！" + error.ToString() + " - " + message);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId == AdsInitializer.inst.adRewardId){
            switch(showCompletionState){
                case UnityAdsShowCompletionState.COMPLETED :
                    Debug.Log("広告報酬！！");
                    AdsShowCompleted();
                    break;
                case UnityAdsShowCompletionState.SKIPPED :
                    Debug.Log("スキップ!");
                    AdsShowSkipped();
                    break;
            }
        }        
    }

    /// <summary>
    /// 広告をちゃんと見た後の処理
    /// </summary>
    protected virtual void AdsShowCompleted(){}

    /// <summary>
    /// 広告をスキップした後の処理
    /// </summary>
    protected virtual void AdsShowSkipped(){}
}
*/