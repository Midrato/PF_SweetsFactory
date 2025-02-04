using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBookDataInit : MonoBehaviour
{
    [SerializeField] private AllPassiveData passiveData;
    void Start()
    {
        var accumData = DataManager.Inst.LoadAccumulativeData();
        int allItemNum = passiveData.allPassiveItems.Count;
        if(accumData.foundPassives.Count < allItemNum){
            // 足りないリスト要素だけ要素を足す
            var addList = new List<bool>();
            for(int i=0;i<allItemNum - accumData.foundPassives.Count;i++)addList.Add(false);
            accumData.foundPassives.AddRange(addList);
            DataManager.Inst.SaveAccumData(accumData);
        }
    }
}
