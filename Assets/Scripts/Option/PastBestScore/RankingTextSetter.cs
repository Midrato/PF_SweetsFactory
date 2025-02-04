using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingTextSetter : MonoBehaviour
{
    // 各種記録のテキスト
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI lastYearText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI recordedDateText;
    [SerializeField] private TextMeshProUGUI isClearText;

    // 順位によった文字カラー(3位まで設定)
    [SerializeField] private List<Color> rankColor = new List<Color>();

    public void SetRankingText(int rank, int lastYear, SeasonEnum lastSeason, int totalScore, string recordedDate, bool isClear){
        // 順位テキストの設定
        rankText.text = rank + "位";
        rankText.color = GetRankColor(rank);
        // 最終期テキストの設定
        lastYearText.text = $"{lastYear}年目 {TurnManager.GetSeasonString(lastSeason)}";
        // 総スコアテキストの設定
        totalScoreText.text = $"{totalScore:N0}アマ";
        // 記録日テキストの設定
        recordedDateText.text = recordedDate;
        // クリアデータかどうかの設定
        isClearText.gameObject.SetActive(isClear);

        Debug.Log(rank + "位をセットしたよ！");
    }

    // 順位ごとに良さげな色を付ける
    private Color GetRankColor(int rank){
        // このランクに色が設定されていたら
        if(rankColor.Count >= rank){
            return rankColor[rank-1];
        }else{
            // 設定されていないなら白
            return Color.white;
        }

    }
}
