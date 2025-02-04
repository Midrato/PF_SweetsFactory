using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M07_BrightHagakure : PassiveItemBase
{
    // 残り手数が偶数の時、桜もちのボーナス売り上げ+2000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            if(data.remainingMoves % 2 == 0){
                return 2000;
            }
        }
        return 0;
    }
}
