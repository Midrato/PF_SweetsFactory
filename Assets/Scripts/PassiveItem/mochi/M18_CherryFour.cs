using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M18_CherryFour : PassiveItemBase
{
    // この手番で全種類の使用アイテムを使用したなら、
    // 桜もちの単価+1500、コンボ倍率+30%
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            // アイテムを全種類使用していないかを取得
            bool isUseAllItem = JudgeUseAllItems(data.itemUsage);

            return isUseAllItem ? 1000 : 0;
        }
        return 0;
    }

    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            bool isUseAllItem = JudgeUseAllItems(data.itemUsage);
            return isUseAllItem ? 0.3f : 0;
        }
        return 0;
    }
    
// アイテムを全種類使用しているかを取得
    private bool JudgeUseAllItems(int[] itemUsage){
        
        bool isNotUseAllItem = false;
        foreach(var usage in itemUsage){
            if(usage == 0){
                isNotUseAllItem = true;
                break;
            }
        }
        return !isNotUseAllItem;
    }
}
