using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C09_StarCutout : PassiveItemBase
{
    // 現在の年が大きいほどチョコのコンボ倍率上昇
    // (1年毎に+3%、5年目で最大値15%)
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            // 5年目が最大値
            return data.year > 5 ? 0.15f : data.year * 0.03f;
        }
        return 0;
    }
}
