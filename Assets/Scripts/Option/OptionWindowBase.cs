using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindowBase : MonoBehaviour
{
    [SerializeField] private RectTransform optionWindowTrans;
    private Vector2 firstPos;

    /// <summary>
    /// オプションメニューを操作出来るか
    /// </summary>
    public bool isCanControlOption = false;

    [SerializeField] protected float fallTime = 0.5f;
    [SerializeField] protected float upTime = 0.4f;

    // 設定画面から離脱するボタン。背景タッチも含める
    [SerializeField] private List<Button> cancelButtons = new List<Button>();
    
    protected virtual void Start()
    {
        firstPos = optionWindowTrans.anchoredPosition;

        // 画面表示
        FallOptionWindow();
        // ボタンにキャンセル処理を仕組む
        SetFunctionToButton();
    }

    public virtual void FallOptionWindow(){
        // 初期位置を画面上に
        optionWindowTrans.anchoredPosition = firstPos + new Vector2(0, ScreenSizeGetter.I.GetTrueScreenSize().y);

        // 画面を落とし、落としきったらオプション操作可能に
        optionWindowTrans.DOAnchorPos(firstPos, fallTime).SetEase(Ease.OutBack).OnComplete(() => isCanControlOption = true);
    }

    public Tween UpOptionWindow(){
        isCanControlOption = false;
        Tween val = optionWindowTrans.DOAnchorPos(firstPos+ new Vector2(0, ScreenSizeGetter.I.GetTrueScreenSize().y), upTime).SetEase(Ease.InBack);
        return val;
    }

    // オプション画面を閉じる
    public virtual void CloseWindow(){
        if(isCanControlOption){
            // クローズ前の処理
            BeforeClose();
            // ボタンを押せないように
            SetIsButtonInteractable(false);
            // 画面を上にスクロール完了と共に、メニューチェンジ可能とし、クローズ完了時処理を呼び出す
            UpOptionWindow().OnComplete(_OnCompleteClose);
        }
    }

    /// <summary>
    /// クローズ前に実行される処理
    /// </summary>
    protected virtual void BeforeClose(){}
    /// <summary>
    /// クローズ完了時に実行される処理
    /// </summary>
    protected virtual void _OnCompleteClose(){
        Destroy(this.gameObject);
    }

    // ボタンにオプション画面を閉じる関数を仕組む
    private void SetFunctionToButton(){
        foreach(var button in cancelButtons){
            button.onClick.AddListener(CloseWindow);
        }
    }

    private void SetIsButtonInteractable(bool value){
        foreach(var button in cancelButtons){
            button.interactable = value;
        }
    }
    
}
