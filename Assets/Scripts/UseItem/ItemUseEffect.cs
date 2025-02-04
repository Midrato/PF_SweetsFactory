using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemUseEffect : MonoBehaviour
{
    // アイコン表示のトランスフォーム
    [SerializeField] private RectTransform iconTrans;
    // アイコンの画像表示コンポーネント
    [SerializeField] private Image iconImage;
    // 画面暗転用
    [SerializeField] private DisplayFader dispFader;

    // アイテム使用時のアニメーション
    [SerializeField] private GameObject itemUseAnim;

    [Space]
    // 背景のフェード時間
    [SerializeField] private float fadeDuration = 0.1f;
    // アイコン入退場の時間
    [SerializeField] private float exitDuration = 0.1f;
    // 使用演出を見せる時間
    [SerializeField] private float showDuration = 0.4f;
    // 使用演出中の移動距離
    [SerializeField] private int showMoveDistance = 100;

    void Awake(){
        // 表示位置を右にセット
        iconTrans.anchoredPosition += ScreenSizeGetter.I.GetTrueScreenSize().x * Vector2.right;
        itemUseAnim.SetActive(false);
    }

    public IEnumerator StartEffect(Sprite itemIcon){
        

        // アイテム発動SEを流す
        SoundManager.I.PlaySE("useItem");
        
        iconImage.sprite = itemIcon;
        
        // エフェクトの総時間
        float totalTime = fadeDuration*2 + exitDuration*2 + showDuration;
        itemUseAnim.SetActive(true);

        var animator = itemUseAnim.GetComponent<Animator>();
        animator.speed = 1 / totalTime;
        // アニメーション開始
        animator.SetBool("StartAnim", true);

        float screenWidth = ScreenSizeGetter.I.GetTrueScreenSize().x;

        // 演出のシークエンス
        Sequence effectSeq = DOTween.Sequence();
        
        // 背景をフェード
        effectSeq.Append(dispFader.MyDOFade(0.5f, fadeDuration));
        // アイコンの登場
        effectSeq.Append(iconTrans.DOAnchorPosX(-(screenWidth - showMoveDistance/2), exitDuration).SetEase(Ease.InSine).SetRelative());
        // アイコン見せつけ
        effectSeq.Append(iconTrans.DOAnchorPosX(-showMoveDistance, showDuration).SetEase(Ease.Linear).SetRelative());
        // アイコン退場
        effectSeq.Append(iconTrans.DOAnchorPosX(-(screenWidth - showMoveDistance/2), exitDuration).SetEase(Ease.OutSine).SetRelative());
        // 背景フェードアウト
        effectSeq.Append(dispFader.MyDOFade(0f, fadeDuration));

        yield return effectSeq.WaitForCompletion();
        
    }

}
