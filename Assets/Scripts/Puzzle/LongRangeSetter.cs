using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 遠距離選択有効化の関数, 操作後にリセットするか毎回適用するかを設定可
public class LongRangeFunction{
    public Func<bool> setFunc;
    public bool isEveryTime = false;

    public LongRangeFunction(
        Func<bool> setFunc,
        bool isEveryTime
    ){
        this.setFunc = setFunc;
        this.isEveryTime = isEveryTime;
    }
}

public class LongRangeSetter : MonoBehaviour
{
    // 遠距離選択する関数たち
    public List<LongRangeFunction> longRangeFuncs = new List<LongRangeFunction>();
    
    public bool GetCanLongRange(){
        foreach(var action in longRangeFuncs){
            if(action.setFunc() == true){
                return true;
            }
        }
        return false;
    }

    // 操作後にリセットするものはリセット
    public void AfterOperation(){
        for(int i=longRangeFuncs.Count-1;i>=0;i--){
            if(!longRangeFuncs[i].isEveryTime){
                longRangeFuncs.RemoveAt(i);
            }
        }
    }

    // リストに関数を追加、既に追加されているなら失敗,falseを返す
    public bool AddFunc(Func<bool>func, bool isEveryTime){
        // 既に追加されているものか
        foreach(var action in longRangeFuncs){
            if(func == action.setFunc){
                return false;
            }
        }
        longRangeFuncs.Add(new LongRangeFunction(func, isEveryTime));
        return true;
    }

    
}
