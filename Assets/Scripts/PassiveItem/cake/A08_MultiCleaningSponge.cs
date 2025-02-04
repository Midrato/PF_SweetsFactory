using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A08_MultiCleaningSponge : PassiveItemBase
{
    // ケーキのボーナス倍率(現在の年×15%)(5年目に最大値+75%)
    public override float BonusMagnification (PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            // 現在の年
            var _year = data.year;
            return (_year > 5) ? 0.75f : 0.15f * _year;
        }
        return 0;
    }

}
