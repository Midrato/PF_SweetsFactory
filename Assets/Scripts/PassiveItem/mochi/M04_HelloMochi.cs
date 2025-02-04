using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M04_HelloMochi : PassiveItemBase
{
    // 直近4回の操作で桜もちを消していない時、
    // 桜もちのコンボ倍率+10%
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            // 過去4回でもちを消したかチェック
            bool isDelMochi = false;
            for(int i=0;i<Mathf.Min(4, data.deletedSweets.Count);i++){
                if(data.deletedSweets[i] == SweetsEnum.mochi){
                    isDelMochi = true;
                    break;
                }
            }

            return !isDelMochi ? 0.1f : 0;
        }
        return 0;
    }
}
