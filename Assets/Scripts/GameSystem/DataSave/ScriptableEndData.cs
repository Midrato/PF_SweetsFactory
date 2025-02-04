using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スクリプタブルオブジェクトの成績データ
/// </summary>
[CreateAssetMenu(fileName = "EndPlayData", menuName = "ScriptableObject/EndPlayData")]
public class ScriptableEndData : ScriptableObject
{
    // 今回の記録が何位だったか
    public int ranking = 1;

    public EndPlayData data = new EndPlayData();
    
    public void SetData(EndPlayData _data, int ranking){
        this.ranking = ranking;
        this.data.lastYear = _data.lastYear;
        this.data.lastSeason = _data.lastSeason;
        this.data.totalSales = _data.totalSales;
    }
}