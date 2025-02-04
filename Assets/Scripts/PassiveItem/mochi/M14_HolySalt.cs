using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M14_HolySalt : PassiveItemBase
{
    // 冬季に桜もちを一度に10個以上消したら、
    // 桜もちのコンボ倍率+20%、ボーナス売り上げ+4000
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi && data.season == SeasonEnum.Winter && data.count >= 10){
            return 0.2f;
        }
        return 0;
    }
    
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi && data.season == SeasonEnum.Winter && data.count >= 10){
            return 4000;
        }
        return 0;
    }
}
