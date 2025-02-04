using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I13_AntarcticCold : PassiveItemBase
{
    // アイスの出現数+15
    // アイスの出現数が20個から10個増える毎に、
    // アイスのコンボ倍率+5%(最大25%)、
    // その他のスイーツのコンボ倍率-2%(最小-10%)
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        int iceCount = (data.sweetsSpawnRates[2] - 30) / 10;
        // 性能強化は5回まで
        iceCount = iceCount > 6 ? 5 : iceCount;
        // アイスとそれ以外で分岐
        switch(data.type){
            case SweetsEnum.ice :
                return iceCount * 0.05f;
            default :
                return iceCount * -0.02f;
        }
    }
}
