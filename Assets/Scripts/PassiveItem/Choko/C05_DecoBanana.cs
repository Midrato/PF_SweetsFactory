using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C05_DecoBanana : PassiveItemBase
{
    // この手番でアイテムを使用した場合、
    // チョコのボーナス売上+4000
    public override int BonusPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.choko){
            var _isUseItem = false;
            // アイテムを使用しているかを検索
            foreach(var usage in data.itemUsage){
                if(usage > 0){
                    _isUseItem = true;
                    break;
                }
            }
            return _isUseItem ? 4000 : 0;
        }
        return 0;
    }
}
