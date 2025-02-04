using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C16_Marriage : PassiveItemBase
{
    // チョコの出現数と他のスイーツの出現数の和の
    // 差が小さいほどボーナス倍率上昇
    // (最大+100%、差が1あるごとに効果値-4%)
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            // 他アイテムの出現数をカウント
            int otherSweetsCount = 0;
            for(int i=0;i<5;i++){
                if(i==1)continue;
                otherSweetsCount += data.sweetsSpawnRates[i];
            }
            // 差の絶対値をとる
            int diff = Mathf.Abs(data.sweetsSpawnRates[1] - otherSweetsCount);
            float value = 1 - diff*0.04f;
            // 適用値が0未満なら0に
            return value < 0 ? 0 : value;
        }
        return 0;
    }
}
