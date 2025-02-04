using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Starter : PassiveItemBase
{
    public override float BonusMagnification(PassiveRequireData data)
    {
        // 5個以上スイーツを繋いだらボーナス倍率30%
        if(data.count >= 5){return 0.3f;}
        return 0;
    }
}
