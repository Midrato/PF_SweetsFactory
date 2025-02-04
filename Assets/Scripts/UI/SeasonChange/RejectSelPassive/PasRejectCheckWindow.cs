using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PasRejectCheckWindow : OptionWindowBase
{
    // 選択キャンセルボタン
    [SerializeField] private Button confilmButton;

    private ItemSelector selector;

    public void SetConfilmAction(UnityAction act){
        confilmButton.onClick.AddListener(act);
    }

    public void SetItemSelector(ItemSelector _selector){
        selector = _selector;
    }

    protected override void BeforeClose()
    {
        selector?.SetCanReroll(true);
    }
}
