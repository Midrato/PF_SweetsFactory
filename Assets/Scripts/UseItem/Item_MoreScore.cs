using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_MoreScore : UseItemBase
{
    [SerializeField] private ItemManager itemMan;
    [SerializeField] private ScoreManager scoreMan;
    [SerializeField] private TurnManager turnMan;

    [SerializeField] private Image frameIm;
    [SerializeField] private List<Color> frameColors;

    protected override void Start()
    {
        base.Start();
        // アイテムを使用した状態のデータをロードしたなら、アイテム効果適用
        var data = DataManager.Inst.LoadPlayingData();
        scoreMan.SetUseItemCount(data.thisTimeItemUsage[itemNumber]);
        UpdateFrameColor();
    }

    protected override void _UseItem()
    {
        if(turnMan.remainingMoves <= 1){
            cannotUseText.DispCannotUseItem("残り手数が足りません!");
        }else{
            // アイテム使用倍率を適用
            if(scoreMan.SetUseItemCount(itemMan.itemUsage[itemNumber] + 1)){
                // 手番を-1
                turnMan.SubRemainingMoves();
                StartCoroutine(UsedItem());
                UpdateFrameColor();
            }else{
                cannotUseText.DispCannotUseItem("1度に" + scoreMan.itemUseLimit + "個までしか使えません!");
            }
        }
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
