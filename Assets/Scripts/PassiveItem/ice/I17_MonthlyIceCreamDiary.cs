using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I17_MonthlyIceCreamDiary : PassiveItemBase
{
    // アイスの出現数+35
    // アイスを1度に20個以上消した時、
    // アイスのコンボ倍率+40%
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            if(data.count >= 20){
                return 0.4f;
            }
        }
        return 0;
    }
}
