using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I03_FrozenCider : PassiveItemBase
{
    // アイスを消した際、ボーナス売り上げ+1000アマ
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            return 1000;
        }
        return 0;
    }
}
