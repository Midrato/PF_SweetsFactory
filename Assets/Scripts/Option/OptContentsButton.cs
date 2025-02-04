using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptContentsButton : OpenOptionBase
{   
    // ひとつ前のメニュー
    [SerializeField] private OptionWindowBase beforeWindow;

    protected override void SummonOptionWindow()
    {
        // ひとつ前のメニューが選択可能状態にあるなら
        if(beforeWindow.isCanControlOption){
            // ウィンドウ呼び出し
            var window = Instantiate(optionWindow);
            window.transform.SetParent(canvasTrans, false);
            // ひとつ前のメニューの落下動作をセットする
            window.GetComponent<OptContentsWindow>().SetFallBeforeWindow(beforeWindow.FallOptionWindow);

            // ひとつ前のメニューを上に送る
            beforeWindow.UpOptionWindow();
        }
        
    }
}
