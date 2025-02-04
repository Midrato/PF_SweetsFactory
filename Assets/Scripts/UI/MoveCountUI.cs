using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCountUI : UITextBase
{
    [SerializeField] private TurnManager turnMan;

    protected override void Start()
    {
        base.Start();
        ChangedContent();
    }

    public override void UpdateContent()
    {
        myText.text = "残り" + turnMan.remainingMoves + "手";
    }

    public override void ChangedContent()
    {
        // 残り手数が少ないなら色を変化させる
        if(turnMan.remainingMoves <= 3){
            myText.color = new Color(0.98f, 0.54f, 0.44f);
        }else{
            myText.color = Color.white;
        }
    }
}
