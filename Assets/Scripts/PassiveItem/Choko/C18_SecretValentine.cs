using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C18_SecretValentine : PassiveItemBase
{
    // 春期にチョコの単価+600
    // "トゥルーハート・ホワイト"を所持しているなら
    // 全期でこの効果を適用
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            bool haveTHW = false;
            foreach(var pas in data.passiveItems){
                if(pas.itemName == "トゥルーハート・ホワイト"){
                    haveTHW = true;
                    break;
                }
            }

            if(data.season == SeasonEnum.Spring || haveTHW){
                return 600;
            }
        }
        return 0;
    }
}
