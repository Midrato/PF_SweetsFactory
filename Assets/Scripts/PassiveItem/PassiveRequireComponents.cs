using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// パッシブの計算に必要なもののクラス
public class PassiveRequireData
{
    /// <summary>
    /// 消したスイーツのタイプ
    /// </summary>
    public SweetsEnum type;

    /// <summary>
    /// スイーツを一度に消した数
    /// </summary>
    public int count;

    /// <summary>
    /// 現在の所持金
    /// </summary>
    public int nowMoney;

    /// <summary>
    /// 現在の年
    /// </summary>
    public int year;

    /// <summary>
    /// 現在の季節
    /// </summary>
    public SeasonEnum season;

    /// <summary>
    /// 残り操作可能回数
    /// </summary>
    public int remainingMoves;

    /// <summary>
    /// 各スイーツの出現率の配列
    /// </summary>
    public int[] sweetsSpawnRates;

    /// <summary>
    /// 所持しているパッシブアイテムリスト
    /// </summary>
    public List<PassiveItemBase> passiveItems;

    /// <summary>
    /// 直近で削除したアイテムリスト
    /// </summary>
    public List<SweetsEnum> deletedSweets;

    /// <summary>
    /// この手番でそれぞれアイテムを使用したか
    /// </summary>
    public int[] itemUsage = new int[3];

    /// <summary>
    /// パッシブのランダム性をオンにするか(定常値抽出用)
    /// </summary>
    public bool activeRandom;

    public PassiveRequireData(
        SweetsEnum type,
        int count,
        int nowMoney,
        int year,
        SeasonEnum season,
        int remainingMoves,
        int[] sweetsSpawnRates,
        List<PassiveItemBase> passiveItems,
        List<SweetsEnum> deletedSweets,
        int[] itemUsage,
        bool activeRandom = true
    ){
        this.type = type;
        this.count = count;
        this.nowMoney = nowMoney;
        this.year = year;
        this.season = season;
        this.remainingMoves = remainingMoves;
        this.sweetsSpawnRates = sweetsSpawnRates;
        this.passiveItems = passiveItems;
        this.deletedSweets = deletedSweets;
        this.itemUsage = itemUsage;
        this.activeRandom = activeRandom;
    }
}
