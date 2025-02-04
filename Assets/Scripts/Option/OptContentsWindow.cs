using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptContentsWindow : OptionWindowBase
{
    // ひとつ前のウィンドウを落とす関数
    private Action fallBeforeWindow = () => { };
    private Action onCompleteClose = () => { };

    public void SetFallBeforeWindow(Action action){
        fallBeforeWindow = action;
    }

    public void SetOnCompleteClose(Action action){
        onCompleteClose = action;
    }

    protected override void BeforeClose()
    {
        // ひとつ前のウィンドウを落とす関数を実行
        fallBeforeWindow.Invoke();
    }

    protected override void _OnCompleteClose()
    {
        onCompleteClose.Invoke();
        base._OnCompleteClose();
    }
}
