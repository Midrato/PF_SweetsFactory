using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionMaster : OptionWindowBase
{
    // フェードイン・アウト用フェーダ
    [SerializeField] protected DisplayFader fader;

    // 操作可能とするためのメニューチェンジャー
    private MenuChanger menuChanger;
    
    protected override void Start()
    {
        menuChanger = GameObject.Find("MenuChanger").GetComponent<MenuChanger>();
        base.Start();
        // フェードインする
        fader.MyDOFade(0.6f, fallTime);
    }

    protected override void BeforeClose()
    {
        // フェードアウトする
        fader.MyDOFade(0, upTime);
        base.BeforeClose();
    }

    protected override void _OnCompleteClose()
    {
        menuChanger.canChangeMenu = true;
        base._OnCompleteClose();
    }

}
