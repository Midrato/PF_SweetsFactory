using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D20_YellowEssence : PassiveItemBase
{
    // タイプがドーナツのモジュール1個につき、
    // ドーナツ以外の単価-15
    // ドーナツの単価+100
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        int donutPassiveCount = 0;
        foreach(var pas in data.passiveItems){
            if(pas.itemType == SweetsEnum.donut)donutPassiveCount++;
        }

        if(data.type == SweetsEnum.donut){
            return donutPassiveCount * 75;
        }else{
            return donutPassiveCount * -15;
        }
    }
}
