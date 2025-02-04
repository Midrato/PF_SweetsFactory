using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D07_InstrumentsLandolt : PassiveItemBase
{
    // ドーナツの所持数が多いとドーナツのボーナス売り上げ増加
    // (0個から5個増える毎に+400, 50個の時に最大値+4000)
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            int val = data.sweetsSpawnRates[3] / 5;
            // 強化は10回まで
            val = val > 10 ? 10 : val;
            return 400 * val;
        }
        return 0;
    }
}
