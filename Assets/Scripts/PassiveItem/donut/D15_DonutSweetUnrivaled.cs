using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D15_DonutSweetUnrivaled : PassiveItemBase
{
    // ドーナツ以外のいずれかのスイーツの出現数が
    // ドーナツの出現数より多かったら
    // ドーナツのボーナス倍率-30%
    // ドーナツの出現数が一番多いなら、
    // ドーナツのボーナス倍率+100%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            int donutCount = data.sweetsSpawnRates[3];
            bool moreThanDonut = false;

            // より多いなので、ドーナツは自然と弾かれるからok
            foreach(var count in data.sweetsSpawnRates){
                if(donutCount < count){
                    moreThanDonut = true;
                    break;
                }
            }

            if(moreThanDonut)return 1;
            else return -0.3f;
        }
        return 0;
    }
}
