using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A18_WholeCakeHall : PassiveItemBase
{
    // ケーキの単価+1000，ケーキのコンボ倍率-10%
    // ケーキにかかる総ボーナス倍率が3倍
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            if(data.remainingMoves % 2 == 0){
                // このパッシブ以外の総ボーナスを求める
                float totalMagni = 0;
                foreach(var pas in data.passiveItems){
                    if(pas == this)continue;
                    totalMagni += pas.BonusMagnification(data);
                }
                return totalMagni*2;
            }
        }
        return 0;
    }
}
