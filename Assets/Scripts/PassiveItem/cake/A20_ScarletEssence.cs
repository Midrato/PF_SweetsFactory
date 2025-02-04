using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A20_ScarletEssence : PassiveItemBase
{
    // タイプがケーキのモジュール1つにつき、ケーキの単価+80
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            var counter = 0;
            foreach(var pass in data.passiveItems){
                if(pass.itemType == SweetsEnum.cake){
                    counter++;
                }
            return counter * 80;
            }
        }
        return 0;
        
    }
}
