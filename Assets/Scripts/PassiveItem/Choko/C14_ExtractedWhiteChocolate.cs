using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C14_ExtractedWhiteChocolate : PassiveItemBase
{
    // チョコのコンボ倍率+5%
    // "重塗るブラックチョコ"を所持しているとき、
    // "重塗るブラックチョコ"のデメリット効果を消し、
    // さらにチョコを消した際ボーナス売り上げ+3000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            foreach(var pas in data.passiveItems){
                if(pas.itemName == "重塗るブラックチョコ"){
                    return 3000;
                }
            }
        }
        return 0;
    }
}
