using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A11_ConflictingDesires : PassiveItemBase
{
    // 夏季以外シーズンでケーキのボーナス倍率+50%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            if(data.season != SeasonEnum.Summer){
                return 0.5f;
            }
        }
        return 0;
    }
}
