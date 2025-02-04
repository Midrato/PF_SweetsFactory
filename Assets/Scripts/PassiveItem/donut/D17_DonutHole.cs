using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D17_DonutHole : PassiveItemBase
{
    // 残り操作回数が4の倍数の時にドーナツを操作時、
    // 1つ飛ばしの位置に連鎖可能となり、さらにドーナツのコンボ倍率+10%
    // アイテム"メビウスグミ"が使用不能になる

    private TurnManager turnMan;
    private PuzzleManager puzzleMan;
    private Item_LongRange longRangeItem;

    void Start(){
        // これのみの処理だからFind使用
        turnMan = GameObject.Find("GameManager").GetComponent<TurnManager>();
        puzzleMan = GameObject.Find("Grid").GetComponent<PuzzleManager>();
        longRangeItem = GameObject.Find("Item3").GetComponent<Item_LongRange>();
        // メビウスグミ使用不能に
        longRangeItem.canUse = false;
        puzzleMan.longRangeSetter.AddFunc(SetCanLongRange, true);
    }

    private bool SetCanLongRange(){
        // 残り操作回数が4の倍数か
        if(turnMan.remainingMoves % 4 == 0){
            // タッチしたものがドーナツか
            if(puzzleMan.connectMans[0]?.sweetsType == SweetsEnum.donut){
                //Debug.Log("ドーナツホールです!");
                return true;
            }
        }
        return false;
    }

    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut && data.remainingMoves % 4 == 0){
            return 0.1f;
        }
        return 0;
    }
}
