using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C17_SnowOfChocola : PassiveItemBase
{
    // チョコの出現数+50
    // チョコを1度に20個以上消すとボーナス倍率+100%、
    // チョコの単価+(チョコの出現数×3)
    // チョコのコンボ倍率+(チョコの出現数÷10)%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            if(data.count >= 20){
                return 1;
            }
        }
        return 0;
    }

    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            // チョコ出現数の3倍の単価をプラス
            return data.sweetsSpawnRates[1] * 3;
        }
        return 0;
    }

    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            // チョコ出現数の10分の1(=1000分の1)の値をコンボ倍率にプラス
            return data.sweetsSpawnRates[1] / 1000f;
        }
        return 0;
    }
}
