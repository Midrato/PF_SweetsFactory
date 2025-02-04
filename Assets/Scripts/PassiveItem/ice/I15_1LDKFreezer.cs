using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I15_1LDKFreezer : PassiveItemBase
{
    // アイスを1度に8個以上消した際、アイスの単価+300
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            if(data.count >= 8){
                return 300;
            }
        }
        return 0;
    }
}
