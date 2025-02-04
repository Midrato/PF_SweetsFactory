using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenOptionBase : MonoBehaviour
{
    // 親にする用キャンバス
    [SerializeField] protected Transform canvasTrans;
    // オプションメニューのプレハブ
    [SerializeField] protected GameObject optionWindow;

    // オプションを呼び出すボタン
    protected Button optionButton;

    void Start(){
        // 親キャンバスが設定されていないならメインキャンバスを探す
        if(canvasTrans == null)canvasTrans = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        optionButton = GetComponent<Button>();
        optionButton.onClick.AddListener(SummonOptionWindow);
    }

    // オプション画面を召喚する
    protected virtual void SummonOptionWindow(){
        // ウィンドウ呼び出し
        var window = Instantiate(optionWindow, canvasTrans, false);
    }
}
