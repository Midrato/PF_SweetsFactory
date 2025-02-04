using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A07_NumberCandle : PassiveItemBase
{
    // 残り手数が5の倍数の時にケーキを売ると、
    // ボーナス売り上げ+3000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            if(data.remainingMoves % 5 == 0){
                return 3000;
            }
        }
        return 0;
    }
}
