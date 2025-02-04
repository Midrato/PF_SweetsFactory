using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemBase : MonoBehaviour
{
    // アイテム検索用のID
    [HideInInspector]public int itemID = 0;
    // アイテム名
    [field:SerializeField]public string itemName {get;protected set;}
    // アイテムの対象タイプ(汎用はNONE)
    [field:SerializeField]public SweetsEnum itemType {get;protected set;}
    // アイテムのレア度
    [field:SerializeField, Range(1, 3)]public int itemRank {get;protected set;}
    // アイテムの効果説明文
    [field:TextArea(1,3)]
    [field:SerializeField]public string itemExplanation {get;protected set;}
    // アイテムのアイコン
    [field:SerializeField]public Sprite itemSprite {get;protected set;}

    [field:Space]

    // 各スイーツの単体価値/倍率/出現率の差分
    [field:SerializeField]public int[] priceDiff {get;protected set;} = new int[5];
    [field:SerializeField]public float[] magnificationDiff {get;protected set;} = new float[5];
    [field:SerializeField]public int[] spawnRatesDiff {get;protected set;} = new int[5];


    /// <summary>
    /// 特別な状況で""コンボ倍率""を付与するオーバーライド前提関数
    /// </summary>
    /// <param name="data">計算に必要なデータ</param>
    /// <returns></returns>
    public virtual float SpecialSweetsMagnification(PassiveRequireData data){
        return 0f;
    }

    /// <summary>
    /// 特別な状況で""ボーナス倍率(スイーツ数によらない固定倍率ボーナス)""を付与するオーバーライド前提関数
    /// </summary>
    /// <param name="data">計算に必要なデータ</param>
    /// <returns></returns>
    public virtual float BonusMagnification(PassiveRequireData data){
        return 0f;
    }

    /// <summary>
    /// 特別な状況で""スイーツ単体価値""に値を付与するオーバーライド前提関数
    /// </summary>
    /// <param name="data">計算に必要なデータ</param>
    /// <returns></returns>
    public virtual int SpecialSweetsPrice(PassiveRequireData data){
        return 0;
    }

    /// <summary>
    /// 特別な状況で""ボーナス売り上げ(スイーツ数によらない固定値ボーナス)""に値を付与するオーバーライド前提関数
    /// </summary>
    /// <param name="data">計算に必要なデータ</param>
    /// <returns></returns>
    public virtual int BonusPrice(PassiveRequireData data){
        return 0;
    }

    /// <summary>
    /// 特別な状況で""スイーツ出現数""に値を付与するオーバーライド前提関数
    /// </summary>
    /// <param name="data">計算に必要なデータ</param>
    /// <returns></returns>
    public virtual int[] SpecialSpawnRates(PassiveRequireData data){
        return new int[5]{0, 0, 0, 0, 0};
    }

}
