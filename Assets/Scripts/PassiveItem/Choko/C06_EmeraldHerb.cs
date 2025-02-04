using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C06_EmeraldHerb : PassiveItemBase
{
    // 夏季にチョコを消した際、ボーナス倍率+50%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            if(data.season == SeasonEnum.Summer){
                return 0.5f;
            }
        }
        return 0;
    }
}
