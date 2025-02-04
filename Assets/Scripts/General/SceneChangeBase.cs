using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeBase : MonoBehaviour
{
    // シーンチェンジできるか
    public bool canSceneChange = false;

    // 遷移先のシーン名
    [SerializeField] private string SceneName;

    protected virtual void Start()
    {
        canSceneChange =  true;
    }

    // シーンチェンジする
    public virtual bool ChangeScene(){
        if(canSceneChange){
            // ロード出来たら全Tweenをキル
            SceneManager.sceneLoaded += (Scene, LoadSceneMode) => DOTween.KillAll();
            
            SceneManager.LoadScene(SceneName);
            return true;
        }else{
            return false;
        }
    }
}
