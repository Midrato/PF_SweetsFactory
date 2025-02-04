using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I10_OasisLookMidsummer : PassiveItemBase
{
    // 夏季・秋季にアイスのコンボ倍率+15%
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            if(data.season == SeasonEnum.Summer || data.season == SeasonEnum.Autumn){
                return 0.15f;
            }
        }
        return 0;
    }
}
