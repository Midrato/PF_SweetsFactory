using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M13_YoungChocoPaste : PassiveItemBase
{
    // 桜もちの出現数+20
    // 残り手数が15回以上なら、桜もちのボーナス倍率+50%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            if(data.remainingMoves >= 15){
                return 0.5f;
            }
        }
        return 0;
    }
}
