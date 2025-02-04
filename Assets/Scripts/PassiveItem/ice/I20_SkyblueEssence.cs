using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I20_SkyblueEssence : PassiveItemBase
{
    // タイプがアイスのモジュールの1個つき、
    // アイスの出現数+1、単価+20、コンボ倍率+0.5%
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            // アイスのパッシブの個数をカウント
            int icePassiveCount = 0;
            foreach(var pas in data.passiveItems){
                if(pas.itemType == SweetsEnum.ice)icePassiveCount++;
            }
            return 20 * icePassiveCount;
        }
        return 0;
    }

    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            // アイスのパッシブの個数をカウント
            int icePassiveCount = 0;
            foreach(var pas in data.passiveItems){
                if(pas.itemType == SweetsEnum.ice)icePassiveCount++;
            }
            return 0.005f * icePassiveCount;
        }
        return 0;
    }

    public override int[] SpecialSpawnRates(PassiveRequireData data)
    {
        // アイスのパッシブの個数をカウント
        int icePassiveCount = 0;
        foreach(var pas in data.passiveItems){
            if(pas.itemType == SweetsEnum.ice)icePassiveCount++;
        }

        return new int[5]{0, 0, icePassiveCount, 0, 0};
    }
}
