using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A19_BirthDaySharedByThree : PassiveItemBase
{
    // 残り手数が3の倍数の時にケーキを3個消すと、
    // ボーナス売り上げ+(10000×現在の年)(10年目で最大値)

    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            if(data.remainingMoves % 3 == 0 && data.count == 3){
                // 10年目に最大値を取るように
                return (data.year > 10) ? 13000 * 10 : 13000 * data.year;
            }
        }
        return 0;
    }
}
