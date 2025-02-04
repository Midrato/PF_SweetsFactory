using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A15_ChocoSymphony : PassiveItemBase
{
    // "ケーキの単価+200アマ、チョコの出現数10個毎にさらに
    // ケーキの単価+100(最大合計+700)"

    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            int val = data.sweetsSpawnRates[1] / 10;
            // 効果強化は5回まで
            val = (val > 5)?5:val;
            return val*100;
        }
        return 0;
    }

}
