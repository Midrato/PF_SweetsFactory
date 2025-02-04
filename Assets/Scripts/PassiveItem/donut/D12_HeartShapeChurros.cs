using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D12_HeartShapeChurros : PassiveItemBase
{
    // ドーナツを連続で売った時、ドーナツのボーナス倍率+50%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            // 消した数0なら即リターン
            if(data.deletedSweets.Count == 0)return 0;
            if(data.deletedSweets[0] == SweetsEnum.donut){
                return 0.5f;
            }
        }
        return 0;
    }
}
