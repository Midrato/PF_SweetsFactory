using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A09_BigCutRollCake : PassiveItemBase
{
    // ケーキを1度に4個以下一度に売ったらケーキ単価+300アマ
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            if(data.count<=4  && data.count > 0){
                return 300;
            }
        }
        return 0;
    }
}
