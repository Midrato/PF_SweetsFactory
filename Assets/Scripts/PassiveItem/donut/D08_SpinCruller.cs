using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D08_SpinCruller : PassiveItemBase
{
    // ドーナツを1度に5個以上消したとき、ボーナス売り上げ+2000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            if(data.count >= 5){
                return 2000;
            }
        }
        return 0;
    }
}
