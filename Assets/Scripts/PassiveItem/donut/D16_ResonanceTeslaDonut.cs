using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D16_ResonanceTeslaDonut : PassiveItemBase
{
    // ドーナツと他のいずれかのスイーツの出現数が同じなら、
    // それぞれの単価+200、コンボ倍率+10%
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            // ドーナツ：他に同じ数のスイーツがあるか
            int donutCount = data.sweetsSpawnRates[3];
            for(int i=0;i<5;i++){
                if(i==3)continue;
                if(data.sweetsSpawnRates[i] == donutCount){
                    return 200;
                }
            }
        }else{
            // その他：ドーナツと同じ数か
            if(data.sweetsSpawnRates[(int)data.type] == data.sweetsSpawnRates[3]){
                return 200;
            }
        }
        return 0;
    }

    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            // ドーナツ：他に同じ数のスイーツがあるか
            int donutCount = data.sweetsSpawnRates[3];
            for(int i=0;i<5;i++){
                if(i==3)continue;
                if(data.sweetsSpawnRates[i] == donutCount){
                    return 0.1f;
                }
            }
        }else{
            // その他：ドーナツと同じ数か
            if(data.sweetsSpawnRates[(int)data.type] == data.sweetsSpawnRates[3]){
                return 0.1f;
            }
        }
        return 0;
    }
}
