using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A17_SuperGoldenChocolate : PassiveItemBase
{
    // ケーキ出現数-5
    // ケーキを1つだけ消したとき、ケーキ単価+1500、
    // ケーキのボーナス倍率+500%
    // 残り手数がちょうど10回の時に、ケーキを1つだけ消すと効果が強化され、
    // ケーキ単価+2000、
    // ケーキのボーナス倍率+1000%

    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            if(data.count == 1){
                return data.remainingMoves == 10 ? 2000 : 1500;
            }
        }
        return 0;
    }

    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            if(data.count == 1){
                return data.remainingMoves == 10 ? 10 : 5;;
            }
        }
        return 0;
    }
}
