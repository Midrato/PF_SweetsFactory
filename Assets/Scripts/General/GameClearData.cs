using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearData
{
    // 最後の年
    public int finalYear {get;private set;}
    // 最後の四半期
    public SeasonEnum finalSeason {get;private set;}
    // 稼いだ総額
    public int totalAmount {get;private set;}

    // 最終データを保存する
    public void SetFinalData(int finalYear, SeasonEnum finalSeason, int totalAmount){
        this.finalYear = finalYear;
        this.finalSeason = finalSeason;
        this.totalAmount = totalAmount;
    }

}
