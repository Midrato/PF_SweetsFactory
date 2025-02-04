using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveReroller : MonoBehaviour
{
    // リロール用ボタン
    private Button rerollButton;
    // リロール回数表示テキスト
    [SerializeField] private TextMeshProUGUI dispText;

    // リロール可能な回数
    [SerializeField] private int rerollCount = 1;
    // 広告を見てリロール出来る回数
    [SerializeField] private int adRerollCount = 0;

    // 現在の残リロール回数を保存する
    private int remainingReroll;
    private int remainingAdReroll;

    // アイテム抽出を行うコンポーネント
    [SerializeField] private ItemSelector itemSelector;

    // リロール回数を使い終わった後に切り替えるモジュール受取拒否機能
    [SerializeField] private PassiveSelCanceler canceler;
    
    void Awake(){
        remainingReroll = rerollCount;
        remainingAdReroll = adRerollCount;
        // リロール回数に応じてキャンセル機能の無効化を行う
        if(remainingReroll > 0 || remainingAdReroll > 0)canceler.enabled = false;
        else canceler.enabled = true;
    }
    
    void Start()
    {
        rerollButton = GetComponent<Button>();
        rerollButton.onClick.AddListener(RerollButton);

        SetState();
    }

    void Update(){
        // ボタンの入力可否を操作の可否で設定
        rerollButton.interactable = itemSelector.canReroll;
    }

    /// <summary>
    /// 残リロール回数をみてリロールを行う
    /// </summary>
    private void RerollButton(){
        // 通常リロールの回数が残っているなら
        if(remainingReroll > 0){
            if(itemSelector.RerollPassive()){
                // リロール出来たら回数-1
                remainingReroll--;
                SetState();
            }
        }else if(remainingAdReroll > 0){
            if(itemSelector.canReroll){
                // 広告を見せる
                //ShowAd();
            }
        }
    }

    /*
    protected override void AdsShowCompleted()
    {
        // リロール広告を見た後の処理
        itemSelector.RerollPassive();
        remainingAdReroll--;
        SetState();
    }
    */

    private void SetState(){
        if(remainingReroll > 0){
            dispText.text = $"リロール\n残り{remainingReroll.ToString()}回";
        }else if(remainingAdReroll > 0){
            dispText.text = $"広告を見て\nリロール";
        }else{
            // リロール不能, 受け取り拒否機能に切り替え
            dispText.text = "モジュール\n受取拒否";
            // キャンセル機能を有効に
            canceler.enabled = true;
        }
    }
}
