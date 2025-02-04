using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C02_FaceCacao : PassiveItemBase
{
    // 残り手数が11以上なら、チョコを消した際ボーナス倍率+20%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            if(data.remainingMoves >= 11){
                return 0.2f;
            }
        }
        return 0;
    }
}
