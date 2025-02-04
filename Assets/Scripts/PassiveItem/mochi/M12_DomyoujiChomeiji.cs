using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M12_DomyoujiChomeiji : PassiveItemBase
{
    // 一度に売った桜もちの個数が素数個なら、
    // ボーナス売り上げ+50%
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            if(GetIsPrime(data.count)){
                return 0.5f;
            }
        }
        return 0;
    }

    // 値が素数かチェック
    private bool GetIsPrime(int num){
        if(num <= 1)return false;
        
        for(int i = num/2; i>1;i--){
            if(num % i == 0)return false;
        }
        return true;
    }
}
