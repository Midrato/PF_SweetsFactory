using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C13_RichMilkXMilk : PassiveItemBase
{
    // チョコの単価+100
    // チョコを売った直後にケーキもしくはアイスを売るとき
    // ボーナス倍率+30%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake || data.type == SweetsEnum.ice){
            // 消した数0なら即リターン
            if(data.deletedSweets.Count == 0)return 0;
            if(data.deletedSweets[0] == SweetsEnum.choko){
                return 0.3f;
            }
        }
        return 0;
    }
}
