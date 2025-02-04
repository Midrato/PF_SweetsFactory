using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M05_HoldOnFoodColoring : PassiveItemBase
{
    // この手番にアイテムを使用した場合、桜もちの単価+300
    public override int SpecialSweetsPrice(PassiveRequireData data)
    {
        if(data.type == SweetsEnum.mochi){
            // アイテムを使ったか確認
            bool isUseItem = false;
            foreach(var usage in data.itemUsage){
                if(usage > 0){
                    isUseItem = true;
                    break;
                }
            }

            return isUseItem ? 300 : 0;
        }
        return 0;
    }
}
