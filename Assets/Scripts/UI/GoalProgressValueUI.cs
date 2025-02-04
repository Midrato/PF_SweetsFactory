using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalProgressValueUI : UITextBase
{
    [SerializeField] private MoneyManager moneyMan;
    [SerializeField] private PeriodGoalManager goalMan;

    protected override void Start()
    {
        base.Start();
        ChangedContent();
    }
    
    public override void UpdateContent()
    {
        myText.text = CalcProgressByPercentage() + "%";
    }

    // 所持金が目標達成(100%超え)なら文字色を変える
    public override void ChangedContent()
    {   
        var percentage = CalcProgressByPercentage();
        if(percentage >= 100){
            myText.color = Color.yellow;
        }else{
            myText.color = Color.white;
        }
    }

    // 所持金が目標金額の何%かを計算する
    private int CalcProgressByPercentage(){
        int nowMoney = moneyMan.nowMoney;
        int goalMoney = goalMan.NowGoalMoney();

        float ratio = (float)nowMoney / goalMoney;
        return (int)(ratio*100);
    }
}
