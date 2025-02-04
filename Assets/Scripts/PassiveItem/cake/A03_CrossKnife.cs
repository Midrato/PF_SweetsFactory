using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A03_CrossKnife : PassiveItemBase
{
    // ケーキを1度に4つ以上売ったらケーキ倍率+10%
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake && data.count >= 4){
            return 0.1f;
        }else{
            return 0;
        }
    }
}
