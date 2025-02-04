using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C01_LivingChocolate : PassiveItemBase
{
    // チョコを1度に5個以上売るとボーナス売り上げ+1500アマ
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            if(data.count >= 5){
                return 1500;
            }
        }
        return 0;
    }
}
