using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M19_SpringStormNerikiri : PassiveItemBase
{
    // 各季節で次のモジュールを所持しているなら効果発動
    // 春季:"桜一色団子"，"咲き誇る桜の木の下"
    // 夏季:"つぶやきあん"，"アルカナ・モチ"
    // 秋季:"うららか葉隠れ"，"大楓::唐紅"
    // 冬季:"ねばねばもち米"，"ホーリーソルト"
    // 桜もちのコンボ倍率+40%
    // 桜もちの出現数+15
    // 全季節で発動するなら，代わりに桜もちのコンボ倍率+60%
    // 桜もちの出現数+30

    public override float SpecialSweetsMagnification(PassiveRequireData data)
    {
        // 各条件アイテムを保持しているかを取得
        bool[] havePassives = GetHaveItems(data.passiveItems);

        // 全季節条件満たさないか
        bool isNotHaveAll = false;
        foreach(var isHave in havePassives){
            if(!isHave){
                isNotHaveAll = true;
                break;
            }
        }
        if(!isNotHaveAll)return 0.6f;

        // 各季節条件
        if(data.season == SeasonEnum.Spring && havePassives[0] && havePassives[1])return 0.4f;
        if(data.season == SeasonEnum.Summer && havePassives[2] && havePassives[3])return 0.4f;
        if(data.season == SeasonEnum.Autumn && havePassives[4] && havePassives[5])return 0.4f;
        if(data.season == SeasonEnum.Winter && havePassives[6] && havePassives[7])return 0.4f;
    
        return 0;
    }

    public override int[] SpecialSpawnRates(PassiveRequireData data)
    {
        // 各条件アイテムを保持しているかを取得
        bool[] havePassives = GetHaveItems(data.passiveItems);

        // 全季節条件満たさないか
        bool isNotHaveAll = false;
        foreach(var isHave in havePassives){
            if(!isHave){
                isNotHaveAll = true;
                break;
            }
        }
        if(!isNotHaveAll)return new int[5]{0,0,0,0,30};

        // 各季節条件
        if(data.season == SeasonEnum.Spring && havePassives[0] && havePassives[1])return new int[5]{0,0,0,0,15};
        if(data.season == SeasonEnum.Summer && havePassives[2] && havePassives[3])return new int[5]{0,0,0,0,15};
        if(data.season == SeasonEnum.Autumn && havePassives[4] && havePassives[5])return new int[5]{0,0,0,0,15};
        if(data.season == SeasonEnum.Winter && havePassives[6] && havePassives[7])return new int[5]{0,0,0,0,15};
    
        return new int[5];
    }

    // 各条件アイテムを保持しているかを取得
    private bool[] GetHaveItems(List<PassiveItemBase> havePassives){
        bool[] _have = new bool[8]{false, false, false, false, false, false, false, false};
        foreach(var pas in havePassives){
            if(pas.itemName == "桜一色団子" && !_have[0])_have[0] = true;
            if(pas.itemName == "咲き誇る桜の木の下" && !_have[1])_have[1] = true;
            if(pas.itemName == "つぶやきあん" && !_have[2])_have[2] = true;
            if(pas.itemName == "アルカナ・モチ" && !_have[3])_have[3] = true;
            if(pas.itemName == "うららか葉隠れ" && !_have[4])_have[4] = true;
            if(pas.itemName == "大楓::唐紅" && !_have[5])_have[5] = true;
            if(pas.itemName == "ねばねばもち米" && !_have[6])_have[6] = true;
            if(pas.itemName == "ホーリーソルト" && !_have[7])_have[7] = true;
        }
        return _have;
    }
}
