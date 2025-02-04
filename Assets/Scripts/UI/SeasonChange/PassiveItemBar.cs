using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PassiveItemBar : PassiveExplainBase
{
    // パッシブ出現時の動作時間・ディレイ
    public float showDuration {get; private set;}= 0.8f;
    public float showDelay {get; private set;}= 0.4f;

    private Button button;  // 選択のボタン
    [Space]
    // 非選択・選択時のウィンドウスプライト
    private Image windowImage;
    [SerializeField] private Sprite[] selectSprites = new Sprite[2];
    [Space]
    // 現在選ばれているか
    public bool isSelect = false;

    // タッチする動作
    private Tween txtMove;
    // 自分のトランスフォーム
    private RectTransform myTrans;
    // デフォルトのスケール
    private Vector3 defaultScale;
    
    void Awake(){
        myTrans = GetComponent<RectTransform>();
        defaultScale = myTrans.localScale;

        windowImage = GetComponent<Image>();
        button = GetComponent<Button>();

        // ボタンが押されたら動かす動きを仕込む
        button.onClick.AddListener(SelectMove);
    }
    void Update(){
        // 選択状況でスプライトを設定
        if(!isSelect){
            windowImage.sprite = selectSprites[0];
        }else{
            windowImage.sprite = selectSprites[1];
        }
    }
    
    
    // パッシブアイテムに応じてそれをセットする
    public override void SetItemData(PassiveItemBase item){
        base.SetItemData(item);

        // 準備が出来たから入場する
        GetComponent<RectTransform>().DOAnchorPosX(0, showDuration).SetEase(Ease.OutExpo).SetDelay(showDelay);
    }

    // buttonにアクションを仕込む
    public void SetButtonAction(UnityAction targEvent){
        if (targEvent == null){
            Debug.LogError("targEvent is null!");
            return;
        }
        button.onClick.AddListener(targEvent);
    }

    // ボタンが押されたときに少しボタンを動かす
    public void SelectMove(){
        // SEを鳴らす
        SoundManager.I.PlaySE("button1");
        // Tween中ならキル
        txtMove.Kill();
        txtMove = myTrans.DOScale(1.2f, 0.1f).SetLoops(2, LoopType.Yoyo);
        // キルされた際スケールを戻す
        txtMove.OnKill(() => myTrans.localScale = defaultScale);
    }

    public void SetButtonInteractable(bool val){
        button.interactable = val;
    }

}
