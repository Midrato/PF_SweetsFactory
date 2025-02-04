using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtoEffecter : MonoBehaviour
{
    private Button myButton;
    private RectTransform myRectTrans;
    // ボタンの揺れの大きさ
    [SerializeField]private float emphasisMagni = 0.3f;
    // ボタンを揺らす時間
    private float tweenTime = 0.4f;

    // スケール初期値
    private Vector3 defaultScale;
    private Tween nowTween;

    [SerializeField] private bool enableSE = true;

    void Start()
    {
        myButton = GetComponent<Button>();
        myRectTrans = GetComponent<RectTransform>();

        defaultScale = myRectTrans.localScale;

        // クリック時の動作をボタンのデリゲートに追加
        myButton.onClick.AddListener(EffectButton);
    }

    private void EffectButton(){
        // 初期化
        nowTween.Kill();

        // ボタンをバウンスさせる　
        nowTween = myRectTrans.DOPunchScale(Vector3.one * emphasisMagni, tweenTime, 10);
        nowTween.OnKill(() => myRectTrans.localScale = defaultScale);
        if(enableSE){
            SoundManager.I.PlaySE("button1");
        }
    }

    private void OnDestroy(){
        nowTween.Kill();
    }
}
