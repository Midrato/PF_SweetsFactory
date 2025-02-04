using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C04_DoubleBlackChocolate : PassiveItemBase
{
    // チョコの排出数+10、
    // チョコの単価+150
    // チョコのコンボ倍率-5%
    // あるモジュールと組み合わせると...?
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            bool haveWhiteChoco = false;
            // 所持パッシブから抽出ホワイトチョコを持っているか確認
            foreach(var pas in data.passiveItems){
                haveWhiteChoco = pas.itemName == "抽出ホワイトチョコ";
            }

            return haveWhiteChoco ? 0 : -0.05f;
        }
        return 0;
    }
}
