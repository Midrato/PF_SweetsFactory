using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 使用アイテムの配列
    [SerializeField] private UseItemBase[] useItems = new UseItemBase[3];

    // 各アイテムの所持数
    public int[] HaveItemNumber = new int[3]{0,0,0}; 

    // この手番での各アイテム使用状況
    public int[] itemUsage = new int[3];
    
    // アイテム使用可否判別用メニュー管理コンポーネント
    public MenuChanger menuChanger;

    // アイテム使用演出用ゲームオブジェクト
    [SerializeField] private GameObject itemUseEffecter;
    // アイテム演出のためのキャンバス
    [SerializeField] private RectTransform canvas;

    [SerializeField] private PuzzleManager puzzleMan;

    void Start(){
        // 各アイテムに自身のマネージャーを与える
        foreach(var items in useItems){
            items.itemManager = this;
        }
        // セーブデータを取得
        var data = DataManager.Inst.LoadPlayingData();
        HaveItemNumber = data.havingItemsNum;
        itemUsage = data.thisTimeItemUsage;
    }

    // 特定のアイテムの数を+1
    public void AddUseItem(int index){
        HaveItemNumber[index]++;
    }

    public IEnumerator ItemUseEffect(Sprite usedItemsIcon){
        // メニュー変更・パズルを不可に
        menuChanger.canChangeMenu = false;
        puzzleMan.SetCannotPuzzle(true);
        // エフェクトオブジェクトを生成し、キャンバスに描画
        var _effecter = Instantiate(itemUseEffecter);
        _effecter.GetComponent<RectTransform>().SetParent(canvas, false);

        // 使用演出を起動
        yield return StartCoroutine(_effecter.GetComponent<ItemUseEffect>().StartEffect(usedItemsIcon));
        Destroy(_effecter);

        // メニュー変更・パズルを可能に
        menuChanger.canChangeMenu = true;
        puzzleMan.SetCannotPuzzle(false);
    }

    // アイテム使用状況をリセットする
    public void resetItemUsage(){
        for(int i=0;i<itemUsage.Length;i++){
            itemUsage[i] = 0;
        }
        //MainSceneDataManager.MCInst.SaveNowPlayingData();
        // リセット時に各アイテムの処理を行う
        foreach(var item in useItems){
            item.OnTurnEnd();
        }
    }
}
