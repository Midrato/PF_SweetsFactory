using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M10_Kaede_BrightRed : PassiveItemBase
{
    // 秋季に桜もちの単価+400，ボーナス倍率+50%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            if(data.season == SeasonEnum.Autumn){
                return 0.5f;
            }
        }
        return 0;
    }

    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            if(data.season == SeasonEnum.Autumn){
                return 400;
            }
        }
        return 0;
    }
}
