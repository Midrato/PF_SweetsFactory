using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I14_SuperHardCupIce : PassiveItemBase
{
    // 現在の年が大きいほどアイスのボーナス倍率上昇
    // (1年毎に+20%、5年目に最大値+100%)
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            int yearCount = data.year;
            // 5年目まで
            yearCount = yearCount > 6 ? 5 : yearCount;
            return yearCount * 0.2f;
        }
        return 0;
    }
}
