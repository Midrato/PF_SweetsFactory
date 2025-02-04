using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookViewer : PassiveExplainBase
{
    void Start(){
        ResetViewer();
    }
    
    public void ResetViewer(){
        itemIcon.sprite = null;
        // 色を透明に
        itemIcon.color = new Color(0,0,0,0);
        itemName.text = "";
        // 文字色を白にリセット
        itemName.color = Color.white;
        itemRank.text = "";
        itemType.text = "";
        itemExplain.text = "";
    }

    public override void SetItemData(PassiveItemBase item)
    {
        itemIcon.color = Color.white;
        base.SetItemData(item);
    }
}
