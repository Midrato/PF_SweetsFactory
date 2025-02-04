using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A02_ColorfulCandle : PassiveItemBase
{
    // ケーキを消した際ボーナス30%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            return 0.3f;
        }
        return 0;
    }
}
