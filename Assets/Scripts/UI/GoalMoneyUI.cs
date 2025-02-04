using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMoneyUI : UITextBase
{
    [SerializeField] private PeriodGoalManager goalMan;

    public override void UpdateContent()
    {
        myText.text = "目標金額：" + goalMan.NowGoalMoney().ToString("N0") + "アマ";
    }
}
