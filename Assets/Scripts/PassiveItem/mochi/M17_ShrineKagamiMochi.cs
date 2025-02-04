using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M17_ShrineKagamiMochi : PassiveItemBase
{
    // 桜もちにかかる、
    // 条件付きのボーナス倍率・ボーナス売り上げが2倍になる
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            float otherBonusTotal = 0;
            // 他のパッシブの合計値を得る
            foreach(var pas in data.passiveItems){
                if(pas == this)continue;
                otherBonusTotal += pas.BonusMagnification(data);
            }
            return otherBonusTotal;
        }
        return 0;
    }

    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            int otherBonusTotal = 0;
            // 他のパッシブの合計値を得る
            foreach(var pas in data.passiveItems){
                if(pas == this)continue;
                otherBonusTotal += pas.BonusPrice(data);
            }
            return otherBonusTotal;
        }
        return 0;
    }
}
