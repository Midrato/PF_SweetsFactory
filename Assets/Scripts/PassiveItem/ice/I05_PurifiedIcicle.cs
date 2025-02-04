using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I05_PurifiedIcicle : PassiveItemBase
{
    // アイスを一度に5個以上売ると、
    // ボーナス倍率+(アイスの出現数)%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            if(data.count >= 5){
                return data.sweetsSpawnRates[2]/100f;
            }
        }
        return 0;
    }
}
