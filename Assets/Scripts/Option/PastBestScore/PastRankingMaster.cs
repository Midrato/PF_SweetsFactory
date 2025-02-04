using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastRankingMaster : ObjectSpacer
{   
    // ランキングのテキストのプレハブ
    [SerializeField] private GameObject rankingObject;

    // 記録が無いことを示すテキスト
    [SerializeField] private GameObject noDataText;


    // 累計データを元にスコアを生成
    protected override List<RectTransform> InstObjects()
    {
        var data = DataManager.Inst.LoadAccumulativeData();
        // データがなければデータが無いことを示して終了
        if(data.gradeRanking.Count == 0){
            noDataText.SetActive(true);
            return new List<RectTransform>();
        }else{noDataText.SetActive(false);}

        // 返り値用トランスフォームリスト
        var transforms = new List<RectTransform>();
        // 現在辿っているデータの順位
        int nowRank = 0;
        // ランキングデータのオブジェクトを作成していく
        foreach(var ranking in data.gradeRanking){
            // 順位加算
            nowRank++;

            var obj = Instantiate(rankingObject);
            var objTrans = obj.GetComponent<RectTransform>();
            objTrans.SetParent(contentsTrans);
            // ランキングのテキストを設定
            obj.GetComponent<RankingTextSetter>().SetRankingText(nowRank, ranking.lastYear, ranking.lastSeason, ranking.totalSales, ranking.recordedDay, ranking.isClear);

            transforms.Add(objTrans);
        }
        return transforms;
    }

}
