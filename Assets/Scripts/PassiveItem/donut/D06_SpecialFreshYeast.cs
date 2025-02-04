using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D06_SpecialFreshYeast : PassiveItemBase
{
    // ドーナツのボーナス倍率+70%
    // 他のスイーツのボーナス倍率-10%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            return 0.7f;
        }else{
            return -0.1f;
        }
    }
}
