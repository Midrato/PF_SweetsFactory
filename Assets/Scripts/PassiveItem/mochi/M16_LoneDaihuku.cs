using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M16_LoneDaihuku : PassiveItemBase
{
    // 毎回5%の確率で桜もちのコンボ倍率+100%
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            if(data.activeRandom){
                // 乱数生成
                float rand = Random.Range(0f, 1f);
                if(rand <= 0.05f)return 1;
            }
        }
        return 0;
    }
}
