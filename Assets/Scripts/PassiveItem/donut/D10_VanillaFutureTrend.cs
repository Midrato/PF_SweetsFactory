using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D10_VanillaFutureTrend : PassiveItemBase
{
    // 年が経過するほどドーナツの単価上昇
    // さらに年が経過すると、ドーナツの単価減少
    // (1年目から1年ごとに+160, 5年目の時に最大値800
    // 6年目から1年ごとに補正値-100, 10年目の時に最小値300)
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            int year = data.year;
            int val = 0;
            // 年が5年以下
            if(year <= 5){
                val = year * 160;
            }else{
                val = 800;
                // 10年目で効果は一定となる
                year = year > 10 ? 10 : year;
                val -= 100 * (year-5);
            }
            return val;
        }
        return 0;
    }
}
