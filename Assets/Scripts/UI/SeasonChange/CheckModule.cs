using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckModule : OpenOptionBase
{
    // モジュール表示可能か制御するためのセレクター
    [SerializeField] private ItemSelector itemSelector;

    void Update(){
        optionButton.interactable = itemSelector.canReroll;
    }
    protected override void SummonOptionWindow()
    {
        if(itemSelector.canReroll){
            var window = Instantiate(optionWindow, canvasTrans, false);
            // 閉じたらリロール可能に戻す処理を仕込む
            window.GetComponent<OptContentsWindow>()?.SetOnCompleteClose(() => itemSelector.SetCanReroll(true));
            itemSelector.SetCanReroll(false);
        }
        
    }
}
