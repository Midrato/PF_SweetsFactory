using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I06_PolishingChocoMint : PassiveItemBase
{
    // チョコを売った次の手番にアイスのボーナス売り上げ+1000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            // 消した数0なら即リターン
            if(data.deletedSweets.Count == 0)return 0;
            
            if(data.deletedSweets[0] == SweetsEnum.choko){
                return 1000;
            }
        }
        return 0;
    }
}
