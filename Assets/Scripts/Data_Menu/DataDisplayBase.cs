using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataDisplayBase : MonoBehaviour
{
    // データセットテキスト
    [SerializeField] private TextMeshProUGUI yearSeasonText;
    [SerializeField] private TextMeshProUGUI nowMoneyText;
    [SerializeField] private TextMeshProUGUI totalMoneyText;

    // 情報取得用のコンポーネントたち
    [SerializeField] private TurnManager turnMan;
    [SerializeField] private MoneyManager moneyMan;
    
    void OnEnable(){
        SetDatas();
    }

    // 累計データをセットする
    private void SetDatas(){
        // 年・季節について
        var nowSeason = turnMan.nowSeason;
        var nowSeasonColor = TurnManager.GetSeasonColor(nowSeason);
        var _yearSeasonText = turnMan.nowYear + "年目 " + TurnManager.GetSeasonString(nowSeason);
        yearSeasonText.text = _yearSeasonText;

        // 所持金について
        nowMoneyText.text = moneyMan.nowMoney.ToString("N0") + "<size=50%>アマ</size>";
        totalMoneyText.text = moneyMan.totalMoney.ToString("N0") + "<size=50%>アマ</size>";
    }
}
