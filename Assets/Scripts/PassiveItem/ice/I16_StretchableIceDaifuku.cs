using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I16_StretchableIceDaifuku : PassiveItemBase
{
    // 残り手数が少ないほどアイスのボーナス倍率上昇
    // (残り10手から発動。1手少なくなるたびに+5%
    // 残り1手の時に最大値+50%)
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            // 10手から発動、パッシブ効果の強度
            int val = 11 - data.remainingMoves;
            // 0未満は弾く
            val = val < 0 ? 0:val;
            return val * 0.05f;
        }
        return 0;
    }
}
