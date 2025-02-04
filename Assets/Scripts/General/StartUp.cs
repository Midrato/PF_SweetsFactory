using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private int targetFPS = 60;

    // スタート演出用のフェーダー
    [SerializeField] private DisplayFader dispFader;
    // 場面遷移用のスターター
    [SerializeField] private ClickSceneChanger starter;
    // 文字を点滅させる
    [SerializeField] private BlinkObject blink;

    void Awake()
    {
        Application.targetFrameRate = targetFPS;
    }

    void Start(){
        starter.canSceneChange = false;
        dispFader.MyDOFade(0, 0.8f).SetEase(Ease.InExpo).OnComplete(() => {
            starter.canSceneChange = true;
            blink.StartBlink();
            SoundManager.I.PlayBGM("Title");
        });
    }
}
