using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C20_BrownEssence : PassiveItemBase
{
    // タイプがチョコのモジュール1つにつきチョコの出現数+4
    public override int[] SpecialSpawnRates(PassiveRequireData data)
    {
        int chocoPassives = 0;
        foreach(var pas in data.passiveItems){
            if(pas.itemType == SweetsEnum.choko){
                chocoPassives++;
            }
        }
        return new int[5]{0, chocoPassives*4, 0, 0, 0};
    }
}
