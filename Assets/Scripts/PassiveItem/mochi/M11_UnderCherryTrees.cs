using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M11_UnderCherryTrees : PassiveItemBase
{
    // 春季に桜もち以外のスイーツの単価-40
    // 桜もちの単価+400, ボーナス倍率+40%,
    // ボーナス売り上げ+4000
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi && data.season == SeasonEnum.Spring){
            return 0.4f;
        }
        return 0;
    }

    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi && data.season == SeasonEnum.Spring){
            return 4000;
        }
        return 0;
    }

    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.season == SeasonEnum.Spring){
            if(data.type == SweetsEnum.mochi){
                return 400;
            }else{
                return -40;
            }
        }
        return 0;
    }
}
