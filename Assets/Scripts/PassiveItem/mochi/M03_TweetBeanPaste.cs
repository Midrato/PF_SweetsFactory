using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M03_TweetBeanPaste : PassiveItemBase
{
    // 直近に桜もちを消している場合、
    // 桜もちのボーナス倍率+25%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            // 消した数0なら即リターン
            if(data.deletedSweets.Count == 0)return 0;
            if(data.deletedSweets[0] == SweetsEnum.mochi){
                return 0.25f;
            }
        }
        return 0;
    }
}
