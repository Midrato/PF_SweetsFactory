using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D02_HollowedOutBread : PassiveItemBase
{
    // ドーナツのボーナス倍率+20%
    //"業務用穴あけパンチ"を所持しているなら、さらにボーナス
    //倍率+20%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.donut){
            float val = 0.2f;
            
            foreach(var pas in data.passiveItems){
                if(pas.itemName == "業務用穴あけパンチ"){
                    val += 0.2f;
                    break;
                }
            }
            return val;
        }
        return 0;
    }
}
