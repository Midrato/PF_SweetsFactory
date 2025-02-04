using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I11_FightingColdWithCold : PassiveItemBase
{
    // 冬季・春季にアイスの単価+500
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            if(data.season == SeasonEnum.Spring || data.season == SeasonEnum.Winter){
                return 500;
            }
        }
        return 0;
    }
}
