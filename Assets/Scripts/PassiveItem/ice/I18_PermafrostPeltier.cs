using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I18_PermafrostPeltier : PassiveItemBase
{
    // アイス以外のスイーツの単価-100、出現数-5
    // アイスの単価+800
    // アイスを消した際ボーナス倍率+50%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            return 0.5f;
        }
        return 0;
    }
}
