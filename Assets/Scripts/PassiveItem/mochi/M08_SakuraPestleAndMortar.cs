using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M08_SakuraPestleAndMortar : PassiveItemBase
{
    // 桜もちの出現数+10
    // 桜もちの出現数が10の倍数の時、桜もちのボーナス倍率+30%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            if(data.sweetsSpawnRates[4] % 10 == 0){
                return 0.3f;
            }
        }
        return 0;
    }
}
