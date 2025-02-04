using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D13_CoffeeCupTopology : PassiveItemBase
{
    // ドーナツの出現数+10
    // 一度にドーナツを10個以上売った時、ドーナツの単価+200
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            if(data.count >= 10){
                return 200;
            }
        }
        return 0;
    }
}
