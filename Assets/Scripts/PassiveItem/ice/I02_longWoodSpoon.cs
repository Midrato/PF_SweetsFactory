using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I02_longWoodSpoon : PassiveItemBase
{
    // アイスを1度に8個以上売った際、ボーナス売上+2000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            if(data.count >= 8){
                return 2000;
            }
        }
        return 0;
    }
}
