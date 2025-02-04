using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M01_StickyGlutinousRice : PassiveItemBase
{
    // 桜もちを6個以上まとめて売ると、ボーナス倍率+25%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            if(data.count >= 6){
                return 0.25f;
            }
        }
        return 0;
    }
}
