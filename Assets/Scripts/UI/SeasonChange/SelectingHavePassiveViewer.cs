using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingHavePassiveViewer : PassiveViewerBase
{
    // 閲覧終了時に操作可能とするアクションを保存する
    public Action endViewAct;
    protected override void OnEndView()
    {
        endViewAct?.Invoke();
        base.OnEndView();
    }
}
