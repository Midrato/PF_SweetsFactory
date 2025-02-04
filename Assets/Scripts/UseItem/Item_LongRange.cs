using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Item_LongRange : UseItemBase
{
    // アイテムを使用すると、選択可能領域が上下左右1マス増える

    [SerializeField] private ItemManager itemMan;
    [SerializeField] private LongRangeSetter longRangeSetter;
    public bool canUse = true;

    [SerializeField] private Image frameIm;
    [SerializeField] private List<Color> frameColors;

    protected override void Start()
    {
        base.Start();
        // アイテムを使用した状態のデータをロードしたなら、アイテム効果適用
        var data = DataManager.Inst.LoadPlayingData();
        if(data.thisTimeItemUsage[itemNumber] > 0)longRangeSetter.AddFunc(setLongRange, false);
        UpdateFrameColor();
    }


    protected override void _UseItem()
    {
        if(!canUse){
            cannotUseText.DispCannotUseItem("効果によって使用出来ません！");
            return;
        }
        if(longRangeSetter.AddFunc(setLongRange, false)){
            // 長距離選択可能に
            StartCoroutine(UsedItem());
            UpdateFrameColor();
        }else{
            cannotUseText.DispCannotUseItem("使用しても効果がありません!");
        }
    }

    // パズル遠距離接続可能を示す関数
    private bool setLongRange(){
        return true;
    }

    private void UpdateFrameColor(){
        // このアイテムを使った回数によって枠の色を変更
        int useTimes = itemMan.itemUsage[itemNumber];
        frameIm.color = frameColors[Mathf.Clamp(useTimes, 0, frameColors.Count)];
    }

    public override void OnTurnEnd()
    {
        UpdateFrameColor();
    }
}
