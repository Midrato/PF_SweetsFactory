using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A06_MysteryCream : PassiveItemBase
{
    // ケーキ以外のスイーツの単価-50、
    // ケーキのボーナス売り上げ+30%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake)return 0.3f;
        else return 0;
    }
}
