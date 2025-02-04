using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 四半期ごとの目標金額を設定するマネージャー
public class PeriodGoalManager : MonoBehaviour
{
    // 目標金額の初期値
    [SerializeField] private int goalMoneyBase = 30000;

    // 四半期ごとに増える目標金額
    [SerializeField] private float goalMoneyMagnification = 0.5f;

    // 毎期終了時投資する金額の目標金額に対する割合
    [field:SerializeField] public float investFromGoalRatio {get;private set;} = 0.4f;

    [field : Space]

    [SerializeField] private TurnManager turnMan;


    // 今期の目標金額を更新する
    public int NowGoalMoney(){
        return GetGoalMoney(turnMan.nowYear, turnMan.nowSeason);
    }

    // 前目標から、投資額を引いた額と現在の目標の差額を得る
    public int GetEarnByNext(){
        // 初回は目標値の5割
        if(turnMan.nowYear == 1 && turnMan.nowSeason == SeasonEnum.Spring)return NowGoalMoney()/2;
        // 前回の季節
        int beforeYear = turnMan.nowSeason == SeasonEnum.Spring ? turnMan.nowYear-1 : turnMan.nowYear;
        SeasonEnum beforeSeason = turnMan.nowSeason == SeasonEnum.Spring ? SeasonEnum.Winter : turnMan.nowSeason - 1;
        return (int)(NowGoalMoney() - GetGoalMoney(beforeYear, beforeSeason) * (1-investFromGoalRatio));
    }

    // 目標金額を取得する
    // 目標金額：有効数字3桁で残す
    // (基礎値) × (1+(倍率)×経た期数^(3/2))
    public int GetGoalMoney(int year, SeasonEnum season){
        // 今まで合計何四半期経てきたか
        int totalSeasons = (int)season + 4*(year-1);
        // そのままの目標値
        float calcGoalScore = goalMoneyBase * (1 + goalMoneyMagnification * Mathf.Pow(totalSeasons, 1.5f));

        // 桁数
        int digits = Mathf.FloorToInt(Mathf.Log10(calcGoalScore))+1;
        // 丸める倍率
        int roundScale = (int)Mathf.Pow(10, (digits - 3));

        // 3桁に丸めて返す
        return Mathf.RoundToInt(calcGoalScore/roundScale) * roundScale;
    }

    // 今期投資するお金を計算する
    public int CalcSubMoney(){
        return Mathf.RoundToInt(NowGoalMoney() * investFromGoalRatio);
    }
}
