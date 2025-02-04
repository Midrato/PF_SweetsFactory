using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D18_ReincarnationOuroboros : PassiveItemBase
{
    // ドーナツ以外のスイーツを連続で消すと、
    // 1回につき次の全スイーツのコンボ倍率-3%(最低-30%)
    // ドーナツを連続で消すと、
    // 1回につき次のドーナツのコンボ倍率+20%
    // (最大100%)
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        // 消した数0なら即リターン
        if(data.deletedSweets.Count == 0)return 0;
        // 直近に消したもので分岐
        if(data.deletedSweets[0] == SweetsEnum.donut){
            // ドーナツのコンボ倍率強化
            var donutDelCount = 0;
            foreach(var del in data.deletedSweets){
                if(del == SweetsEnum.donut){
                    donutDelCount++;
                }else{
                    break;
                }
            }
            // 最大5に制限
            donutDelCount = Mathf.Clamp(donutDelCount, 0, 6);
            return donutDelCount * 0.2f;
        }else{
            // 全スイーツ弱化
            var otherCount = 0;
            foreach(var del in data.deletedSweets){
                if(del != SweetsEnum.donut){
                    otherCount++;
                }else{
                    break;
                }
            }
            // 最大10に制限
            otherCount = Mathf.Clamp(otherCount, 0, 11);
            return otherCount * -0.03f;
        }
    }
}
