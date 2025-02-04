using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class M06_CherryBlossomLeafPocket : PassiveItemBase
{
    // 全スイーツの出現数の合計が150以上で発動
    // 桜もちのコンボ倍率+15%
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            // 出現数の合計をチェック
            if(data.sweetsSpawnRates.Sum() >= 150){
                return 0.15f;
            }
        }
        return 0;
    }
}
