using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I07_WinningNumber111 : PassiveItemBase
{
    // アイスを売った際、30%の確率でボーナス倍率+100%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            if(data.activeRandom){
                float rand = Random.Range(0f, 1f);
                if(rand <= 0.3f)return 1;
            }
        }
        return 0;
    }
}
