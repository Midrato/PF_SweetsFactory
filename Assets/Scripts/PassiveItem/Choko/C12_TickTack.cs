using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C12_TickTack : PassiveItemBase
{
    // チョコを連続で消すとチョコのコンボ倍率+5%(最大50%)
    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            int continuous = 0;
            // 連続で何回消したのか確認
            for(int i=0;i<Mathf.Min(10, data.deletedSweets.Count);i++){
                if(data.deletedSweets[i] == SweetsEnum.choko){
                    continuous++;
                }else{
                    break;
                }
            }
            return continuous * 0.05f;
        }
        return 0;
    }
}
