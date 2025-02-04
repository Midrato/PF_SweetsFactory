using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPassiveData : MonoBehaviour
{   
    // アイテムの最大レア度
    [field:SerializeField]public int maxRare {get;private set;} = 3;

    // 全パッシブ(トッピングモジュール)のアイテムデータ
    [field:SerializeField]public List<PassiveItemBase> allPassiveItems {get;private set;}

    public void SetPassiveIDs(){
        // パッシブのIDの設定
        for(int i=0;i<allPassiveItems.Count;i++){
            allPassiveItems[i].itemID = i;
        }
    }

    /*// 全パッシブアイテムデータを返す
    public List<PassiveItemBase> GetPassiveItems(){
        var _passiveItems = new List<PassiveItemBase>();
        foreach(var obj in passiveItemsObject){
            _passiveItems.Add(obj.GetComponent<PassiveItemBase>());
        }

        return _passiveItems;
    }
    */
}
