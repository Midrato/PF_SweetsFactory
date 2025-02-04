using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A16_LonelyCake : PassiveItemBase
{
    // ケーキの出現数-5
    // ケーキの出現数が30個から5個少なくなるたびに
    // ケーキの単価+250, 5個以下の時に最大値+1250)
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        // ケーキでないかつケーキ排出量30未満
        if(data.type == SweetsEnum.cake && data.sweetsSpawnRates[0] < 30){
            int tmpMagni = (30 - data.sweetsSpawnRates[0]) / 5;
            if(tmpMagni > 5)tmpMagni = 5;   // 最大5回分

            return tmpMagni * 250;
        }else{
            return 0;
        }
    }
}
