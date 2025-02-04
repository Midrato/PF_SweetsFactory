using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DisplayConfirm : PassiveExplainBase
{
    // パッシブアイコンの収まる所の座標点取得用
    [SerializeField] private RectTransform iconHolderCenter;
    // ゲーム続行ボタン
    [SerializeField] private Button continueButton;
    private CanvasGroup buttonCanvGp;

    // 各表示テキストを一時的に保持する
    private string[] textTmp = new string[2];
    private RectTransform[] textTrans = new RectTransform[2];
    private Vector2[] textScale = new Vector2[2];

    // Tween待ち時間
    private float iconDuration = 1f;
    private float textDuration = 0.5f;
    private float textDistanceTime = 0.3f;

    void Start(){
        buttonCanvGp = continueButton.GetComponent<CanvasGroup>();
        // ボタンを初期は触れられないように
        continueButton.interactable = false;
        buttonCanvGp.alpha = 0;
    }


    public override void SetItemData(PassiveItemBase item)
    {
        base.SetItemData(item);

        SetTmp();
    }

    // テキストを一時的に保管し一旦テキスト表示をしない
    private void SetTmp(){
        textTrans[0] = itemRank.GetComponent<RectTransform>();
        textTrans[1] = itemType.GetComponent<RectTransform>();

        textTmp[0] = itemName.text;
        textScale[0] = textTrans[0].localScale;
        textTmp[1] = itemExplain.text;
        textScale[1] = textTrans[1].localScale;

        itemName.text = "";
        textTrans[0].localScale = Vector3.zero;
        textTrans[1].localScale = Vector3.zero;
        itemExplain.text = "";
    }

    public IEnumerator StartConfirm(){
        var confirmSeq = DOTween.Sequence();
        // アイコンをホルダーの中に収める(モジュールが工場設備に取りつけられるイメージ)
        confirmSeq.Append(itemIcon.GetComponent<RectTransform>().DOAnchorPos(iconHolderCenter.anchoredPosition, iconDuration).SetEase(Ease.InOutQuart));
        // 説明文がだんだん現れる
        confirmSeq.Append(itemName.GetComponent<MyDOText>().DOAddText(textTmp[0], textDuration).SetEase(Ease.OutCubic));
        confirmSeq.Join(textTrans[0].DOScale(textScale[0], textDuration).SetEase(Ease.OutCubic).SetDelay(textDistanceTime));
        confirmSeq.Join(textTrans[1].DOScale(textScale[1], textDuration).SetEase(Ease.OutBack).SetDelay(textDistanceTime*2));
        // 説明文は長いので表示時間2倍
        confirmSeq.Join(itemExplain.GetComponent<MyDOText>().DOAddText(textTmp[1], textDuration*2).SetDelay(textDistanceTime*3));

        // モジュール紹介後にボタン表示
        confirmSeq.Append(buttonCanvGp.DOFade(1, textDuration).SetDelay(textDuration));

        // 表示終了したらコルーチン終了
        yield return confirmSeq.WaitForCompletion();
        // ボタンの有効化を忘れず
        continueButton.interactable = true;
    }

    // 確認の終了まで待つ
    public IEnumerator WaitForTouchButton(){
        yield return StartCoroutine(continueButton.GetComponent<ButtonWaiter>().WaitToTouchButton());

        // ウィンドウを消す
        var endConfirmSeq = DOTween.Sequence();
        endConfirmSeq.Append(buttonCanvGp.DOFade(0, 0.2f));
        endConfirmSeq.Join(GetComponent<RectTransform>().DOScale(Vector3.zero, 0.3f).SetEase(Ease.InQuad).SetDelay(0.2f));
        
        yield return endConfirmSeq.WaitForCompletion();
    }
}
