using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I12_HatTrickBar : PassiveItemBase
{
    // アイスを3連続以上で売った時、コンボ倍率+15%
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.ice){
            // 直近三回で、アイス以外を消したか
            bool isDelOther = false;
            for(int i=0;i<Mathf.Min(3, data.deletedSweets.Count);i++){
                if(data.deletedSweets[i] != SweetsEnum.ice){
                    isDelOther = true;
                    break;
                }
            }
            
            return !isDelOther ? 0.15f : 0;
        }
        return 0;
    }
}
