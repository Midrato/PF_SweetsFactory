using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PassiveViewer : PassiveViewerBase
{

    // メニュー遷移可能にするためのメニューチェンジャー
    [HideInInspector] public MenuChanger menuChanger;

    public void setMenuChanger(MenuChanger _menuChanger){
        menuChanger = _menuChanger;
    }

    protected override void OnEndView(){
        menuChanger.canChangeMenu = true;
    }

}
