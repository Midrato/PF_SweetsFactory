using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C19_TrueHeartWhite : PassiveItemBase
{
    // 春期にチョコのコンボ倍率+30%
    // "シークレット・ヴァレンタイン"を
    // 所持しているなら全期でこの効果を適用
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            bool haveScV = false;
            foreach(var pas in data.passiveItems){
                if(pas.itemName == "シークレット・ヴァレンタイン"){
                    haveScV = true;
                    break;
                }
            }

            if(data.season == SeasonEnum.Spring || haveScV){
                return 0.3f;
            }
        }
        return 0;
    }
}
