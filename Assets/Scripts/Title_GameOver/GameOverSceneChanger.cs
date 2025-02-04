using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameOverSceneChanger : SceneChangeBase
{
    [SerializeField] private DisplayFader fader;
    
    [SerializeField] private float fadeDuration = 2f;

    private GeneralTouchSensor tSens;

    protected override void Start()
    {
        base.Start();
        tSens = new GeneralTouchSensor();
    }

    void Update()
    {
        // スタート可能でタッチを離した瞬間に実行
        if(canSceneChange && tSens.GetTouchUp()){
            StartCoroutine(FadeOutSceneChange(fadeDuration));
        }
    }

    private IEnumerator FadeOutSceneChange(float duration){
        // BGMもフェードアウトさせる
        SoundManager.I.DOFadeOutBGM(duration);
        yield return fader.MyDOFade(1, duration).WaitForCompletion();
        ChangeScene();
    }


}
