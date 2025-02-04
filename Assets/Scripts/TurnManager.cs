using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 四半期を表す列挙体
public enum SeasonEnum{
    Spring,
    Summer,
    Autumn,
    Winter,
    None
}

public class TurnManager : MonoBehaviour
{
    // 現在の年
    [field:SerializeField]public int nowYear {get;private set;} = 1;
    // 現在の四半期(春夏秋冬順)
    [field:SerializeField]public SeasonEnum nowSeason {get;private set;} = SeasonEnum.Spring;
    // 1四半期の操作可能数
    [SerializeField] private int movesPerSeason = 20;
    // この四半期の残り操作回数
    [field:SerializeField]public int remainingMoves {get;private set;}

    // 季節に応じたBGMを流す
    [SerializeField]private SeasonBGMChanger bgmChanger;

    void Awake(){
        nowYear = 1;
        nowSeason = SeasonEnum.Spring;
        remainingMoves = movesPerSeason;
    }

    void Start(){
        Debug.Log(System.DateTime.Now.Month);
        // データをロード
        var data = DataManager.Inst.LoadPlayingData();
        nowYear = data.nowYear;
        nowSeason = data.nowSeason;
        remainingMoves = data.remainingMoves;

        bgmChanger.PlaySeasonBGM(nowSeason);
    }

    // 残り手数を引く
    public void SubRemainingMoves(){
        remainingMoves--;
    }

    // 次の四半期に移行
    public void GoNextSeason(){
        nowSeason++;
        if(nowSeason == SeasonEnum.None){
            nowYear++;
            nowSeason = SeasonEnum.Spring;
        }
        // 操作回数も復活
        remainingMoves = movesPerSeason;

        // BGMも変更し流す
        bgmChanger.PlaySeasonBGM(nowSeason);
    }

    // 季節の列挙体から文字列に変換
    static public string GetSeasonString(SeasonEnum season){
        string seasonText = "";

        switch(season){
            case SeasonEnum.Spring :
                seasonText = "春季";
                break;
            case SeasonEnum.Summer:
                seasonText = "夏季";
                break;
            case SeasonEnum.Autumn:
                seasonText = "秋季";
                break;
            case SeasonEnum.Winter:
                seasonText = "冬季";
                break;
            default:
                seasonText = "";
                break;
        }
        return seasonText;
    }

    // 季節の列挙体から色に変換
    static public Color GetSeasonColor(SeasonEnum season){
        Color seasonColor;

        switch(season){
            case SeasonEnum.Spring :
                seasonColor = new Color(1f, 0.76f, 0.93f);
                break;
            case SeasonEnum.Summer:
                seasonColor = new Color(0.82f, 1f, 0.76f);
                break;
            case SeasonEnum.Autumn:
                seasonColor = new Color(1f, 0.64f, 0.35f);
                break;
            case SeasonEnum.Winter:
                seasonColor = new Color(0.6f, 0.77f, 1f);
                break;
            default:
                seasonColor = Color.white;
                break;
        }
        return seasonColor;
    }

    // 次のシーズンが何年目かを返す
    public int GetNextYear(){
        if(nowSeason == SeasonEnum.Winter){
            return nowYear + 1;
        }else{
            return nowYear;
        }
    }
}
