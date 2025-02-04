using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class YearUI : UITextBase
{
    [SerializeField] private TurnManager turnMan;

    public override void UpdateContent()
    {
        myText.text = turnMan.nowYear + "年目";
    }
}
