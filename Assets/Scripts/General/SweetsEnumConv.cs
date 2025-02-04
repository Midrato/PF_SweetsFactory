using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsEnumConv : MonoBehaviour
{
    public static string SweetsToEnglish(SweetsEnum type){
        string val = "";

        switch(type){
            case SweetsEnum.cake :
                val = "Cake";
                break;
            case SweetsEnum.choko:
                val = "Chocolate";
                break;
            case SweetsEnum.donut:
                val = "Donut";
                break;
            case SweetsEnum.ice:
                val = "Ice cream";
                break;
            case SweetsEnum.mochi:
                val = "Sakura mochi";
                break;
            case SweetsEnum.None:
                val = "General";
                break;
            default:
                val = "";
                break;
        }
        return val;
    }

    public static string SweetsToJapanese(SweetsEnum type){
        string val = "";

        switch(type){
            case SweetsEnum.cake :
                val = "ケーキ";
                break;
            case SweetsEnum.choko:
                val = "チョコレート";
                break;
            case SweetsEnum.donut:
                val = "ドーナツ";
                break;
            case SweetsEnum.ice:
                val = "アイス";
                break;
            case SweetsEnum.mochi:
                val = "桜もち";
                break;
            case SweetsEnum.None:
                val = "一般";
                break;
            default:
                val = "";
                break;
        }
        return val;
    }

    public static Color SweetsToColor(SweetsEnum type){
        Color val = Color.white;

        switch(type){
            case SweetsEnum.cake :
                val = new Color(1f, 0.18f, 0.20f);
                break;
            case SweetsEnum.choko:
                val = new Color(0.95f, 0.56f, 0.11f);
                break;
            case SweetsEnum.donut:
                val = new Color(1f, 1f, 0.3f);
                break;
            case SweetsEnum.ice:
                val = new Color(0.47f, 0.92f, 1f);
                break;
            case SweetsEnum.mochi:
                val = new Color(0.26f, 0.80f, 0.26f);
                break;
            default:
                break;
        }
        return val;
    }
}
