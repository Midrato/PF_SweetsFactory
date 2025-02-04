using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A13_ShareThePie : PassiveItemBase
{
    // "現在の所持金に応じてケーキのボーナス倍率強化
    // (基礎値0%, 所持金10万毎に+7.5%, 100万で最大値+75%)"
    public override float BonusMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.cake){
            var _money = data.nowMoney;
            int bufNum = _money / 100000;
            // 補正は10回まで
            bufNum = (bufNum > 10)?10:bufNum;
            return bufNum * 0.075f;
        }
        return 0;
    }
}
