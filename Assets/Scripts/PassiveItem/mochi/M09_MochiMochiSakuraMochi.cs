using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M09_MochiMochiSakuraMochi : PassiveItemBase
{
    // 現在の所持金が少ないほど桜もちのコンボ倍率上昇
    // (100万アマから発動、
    // 10万アマ少なくなるたびにコンボ倍率+2%
    // 10万アマ以下の時に最大値+20%)
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            // 100万より大きいならリターン
            if(data.nowMoney > 1000000)return 0;
            int val = (1000000 - data.nowMoney) / 100000 + 1;
            return val * 0.02f;
        }
        return 0;
    }
}
