using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D14_DevilsFrench : PassiveItemBase
{
    // 現在の所持金が多いほどドーナツの単価上昇
    // (所持金10万毎に+50, 100万円以上の時最大値+500)
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            // 効果の強度(最大10段階)
            int val = data.nowMoney / 100000;
            val = Mathf.Clamp(val, 0, 11);
            return val * 50;
        }
        return 0;
    }
}
