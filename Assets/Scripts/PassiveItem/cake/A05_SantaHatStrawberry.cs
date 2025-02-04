using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A05_SantaHatStrawberry : PassiveItemBase
{
    // 冬季にケーキのコンボ倍率+20%
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.season == SeasonEnum.Winter && data.type == SweetsEnum.cake) return 0.2f;
        else return 0;
    }
}
