using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonUI : UITextBase
{
    [SerializeField] private TurnManager turnMan;

    public override void UpdateContent()
    {
        myText.text = TurnManager.GetSeasonString(turnMan.nowSeason);
    }
}
