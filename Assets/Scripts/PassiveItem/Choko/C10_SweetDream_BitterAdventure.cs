using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C10_SweetDream_BitterAdventure : PassiveItemBase
{
    // 所持している全モジュール数5個ごとに
    // チョコにボーナス売上+2000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            return 2000 * (int)(data.passiveItems.Count / 5);
        }
        return 0;
    }
}
