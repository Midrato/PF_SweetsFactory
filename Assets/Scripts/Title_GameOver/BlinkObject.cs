using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class BlinkObject : MonoBehaviour
{
    private CanvasGroup myCanvGp;
    private Tween fadeTween;

    [SerializeField] private Ease blinkEase;

    void Start()
    {
        myCanvGp = GetComponent<CanvasGroup>();

        // 最初は0
        myCanvGp.alpha = 0;

        // シーンロード時にこのTweenをkillする
        SceneManager.sceneLoaded += (Scene, LoadSceneMode) => fadeTween?.Kill();
    }

    public void StartBlink(){
        fadeTween = myCanvGp.DOFade(1, 1f).SetEase(blinkEase).SetLoops(-1, LoopType.Yoyo);
    }
}
