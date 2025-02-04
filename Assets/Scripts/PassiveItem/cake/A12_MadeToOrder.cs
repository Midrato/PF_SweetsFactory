using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A12_MadeToOrder : PassiveItemBase
{
    // ケーキを3個以下一度に消したらボーナス売り上げ+5000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake && data.count <= 3 && data.count > 0){
            return 5000;
        }else{
            return 0;
        }
    }
}
