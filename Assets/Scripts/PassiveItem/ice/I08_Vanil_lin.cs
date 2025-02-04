using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I08_Vanil_lin : PassiveItemBase
{
    // アイスを1つだけ売った時、ボーナス倍率+300%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            if(data.count == 1){
                return 3f;
            }
        }
        return 0;
    }
}
