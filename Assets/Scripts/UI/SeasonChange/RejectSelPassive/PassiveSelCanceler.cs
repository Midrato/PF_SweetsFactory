using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSelCanceler : OpenOptionBase
{
    // 操作可能状況取得用
    [SerializeField] private ItemSelector selector;


    protected override void SummonOptionWindow()
    {
        if(selector.canReroll){
            // パッシブ選択画面を一旦操作不能に
            selector.SetCanReroll(false);

            var window = Instantiate(optionWindow, canvasTrans, false);
            var rejectCheck = window.GetComponent<PasRejectCheckWindow>();
            // パッシブ受け取り拒否確認画面に必要なセレクター、パッシブ受け取り拒否動作を設定
            rejectCheck.SetItemSelector(selector);
            rejectCheck.SetConfilmAction(() => {
                selector.SetCanReroll(true);
                selector.isRejectSelection = true;
                Debug.Log("受取拒否！");
            });
        }
        
    }
}
