using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_NowMoney : UITextBase
{
    [SerializeField] private MoneyManager moneyMan;

    public override void UpdateContent()
    {
        myText.text = "現在所持金 : " + moneyMan.nowMoney.ToString("N0") + "アマ";
    }
}
