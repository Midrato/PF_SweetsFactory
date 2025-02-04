using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenOptionMenu : OpenOptionBase
{
    [SerializeField] private MenuChanger menuChanger;

    // オプション画面を召喚する
    protected override void SummonOptionWindow(){
        // 開いているメニューがデータメニューなら
        if(menuChanger.nowMenu == MenuEnum.Data && menuChanger.canChangeMenu){
            // ウィンドウ呼び出し
            var window = Instantiate(optionWindow);
            window.transform.SetParent(canvasTrans, false);
            // メニュー遷移不能に
            menuChanger.canChangeMenu = false;
        }
    }
}
