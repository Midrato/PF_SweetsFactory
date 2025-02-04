using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D11_OilSea : PassiveItemBase
{
    // ドーナツ以外のスイーツを消した際ボーナス売り上げ-500
    // ドーナツの単価+400
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type != SweetsEnum.donut){
            return -500;
        }
        return 0;
    }
}
