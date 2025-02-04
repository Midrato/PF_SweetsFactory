using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I19_LoyalCopen : PassiveItemBase
{
    // アイスを1度に消した数が少ないほど、アイスの性能強化
    // (10個から発動。1個減る毎に
    // 単価+100、ボーナス倍率+20%
    // 1個だけ消した場合さらにボーナス倍率+200%)
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            // パッシブ表示用
            if(data.count == 0)return 0;
            
            // かかる効果の強度
            int val = 11 - data.count;
            // 0未満を弾く
            val = val < 0 ? 0:val;
            return 100 * val;
        }
        return 0;
    }

    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            // かかる効果の強度
            int val = 11 - data.count;
            // 0未満を弾く
            val = val < 0 ? 0:val;

            // 消した数1なら
            if(data.count == 1){
                return 2 + val*0.2f;
            }else{
                return val*0.2f;
            }
        }
        return 0;
    }
}
