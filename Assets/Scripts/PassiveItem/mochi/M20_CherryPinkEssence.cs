using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M20_CherryPinkEssence : PassiveItemBase
{
    // タイプが桜もちのモジュール1個につき、
    // 桜もちの単価+50
    // 桜もちのボーナス倍率+5%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            int passiveCount = 0;
            foreach(var pas in data.passiveItems){
                if(pas.itemType == SweetsEnum.mochi)passiveCount++;
            }

            return passiveCount * 0.05f;
        }
        return 0;
    }

    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            int passiveCount = 0;
            foreach(var pas in data.passiveItems){
                if(pas.itemType == SweetsEnum.mochi)passiveCount++;
            }

            return passiveCount * 50;
        }
        return 0;
    }
}
