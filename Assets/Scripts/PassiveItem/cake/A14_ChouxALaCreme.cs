using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A14_ChouxALaCreme : PassiveItemBase
{
    // ケーキ出現数+10
    // ケーキのボーナス倍率+50%, 単価-100
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            return 0.5f;
        }
        return 0;
    }
}
