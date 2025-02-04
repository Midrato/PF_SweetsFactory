using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ResetBoard : UseItemBase
{
    [SerializeField] private PuzzleManager puzzleMan;

    public override void UseItem()
    {
        if(itemManager.menuChanger.nowMenu != MenuEnum.Factory || !itemManager.menuChanger.canChangeMenu){
            return;
        }else if(itemManager.HaveItemNumber[itemNumber] <= 0){
            cannotUseText?.DispCannotUseItem("アイテムを所持していません!");
            return;
        }else{
            StartCoroutine(SpecialUseItem());
            Debug.Log("UseItem!");
        }
    }

    private IEnumerator SpecialUseItem(){
        yield return StartCoroutine(UsedItem());
        StartCoroutine(puzzleMan.Item_ResetBoard());
    }

    protected override IEnumerator UsedItem()
    {
        itemManager.HaveItemNumber[itemNumber]--;
        itemManager.itemUsage[itemNumber]++;
        // 使用時セーブをなくす(盤面確定後にセーブしたいため)
        // MainSceneDataManager.MCInst.SaveNowPlayingData();
        yield return StartCoroutine(itemManager.ItemUseEffect(itemIcon.sprite));
    }
}
