using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M15_ArcanaMochi : PassiveItemBase
{
    // 夏季に桜もちの出現数に応じて桜もちの単価が強化
    // (10個から5個増えるたびに+100、60個の時最大値+1000)
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi && data.season == SeasonEnum.Summer){
            // 最大10回分強化
            var val = (data.sweetsSpawnRates[4] -10) / 5;
            val = Mathf.Clamp(val, 0, 11);

            return 100 * val;
        }
        return 0;
    }
}
