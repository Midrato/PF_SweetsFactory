using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// パッシブアイテムを所持し、適用値の更新・特別な条件での適用値の譲渡を行うクラス
public class PassiveHolder : MonoBehaviour
{
    // 特別条件で発動するパッシブのためのデータ元
    [SerializeField] private TurnManager turnMan;
    [SerializeField] private MoneyManager moneyMan;

    [Space]

    // アイテムによる更新値を適用するマネージャー
    [SerializeField] private SweetsTypeManager sweetsTypeMan;
    [SerializeField] private ScoreManager scoreMan;
    [SerializeField] private DeleteSweetsBuffer deleteBuffer;
    [SerializeField] private ItemManager itemMan;
    // アイテム初期化用のデータ
    [SerializeField] private AllPassiveData allPassiveData;

    [field : Space]

    // 現在所持しているパッシブアイテム
    [field : SerializeField]public List<PassiveItemBase> passiveItems {get;private set;} = new List<PassiveItemBase>();

    // 特別条件参照用変数
    private int nowMoney;
    private int year;
    private SeasonEnum season;
    private int remainingMove;
    
    void Awake(){
        allPassiveData.SetPassiveIDs();
    }

    void Start(){
        // データを取得
        var data = DataManager.Inst.LoadPlayingData();
        passiveItems = DecodePassive(data.havingPassives);
        FindHavePassive();

        UpdateCorrectionValue();
    }

    // 所持しているパッシブリストを図鑑に登録する
    private void FindHavePassive(){
        var accumData = DataManager.Inst.LoadAccumulativeData();
        foreach(var pas in passiveItems){
            accumData.AddFindPassive(pas.itemID);
        }
        DataManager.Inst.SaveAccumData(accumData);
    }

    // 所持パッシブをエンコード
    public List<int> EncodeHavingPassives(){
        return EncodePassive(passiveItems);
    }

    // パッシブの情報をエンコードする
    public List<int> EncodePassive(List<PassiveItemBase> _passiveItems){
        var ids = new List<int>();
        foreach(var item in _passiveItems){
            ids.Add(item.itemID);
        }
        return ids;
    }

    // パッシブの内容をIDの配列からデコードする
    public List<PassiveItemBase> DecodePassive(List<int> data){
        var passiveItems = new List<PassiveItemBase>();
        foreach(var _data in data){
            var passive = Instantiate(allPassiveData.allPassiveItems[_data]);
            passive.transform.SetParent(this.transform);
            passiveItems.Add(passive.GetComponent<PassiveItemBase>());
        }
        return passiveItems;
    }

    // パッシブアイテムを追加する関数
    public void AddPassiveItem(PassiveItemBase passiveItem){
        // パッシブを実体化
        var obj = Instantiate(passiveItem.gameObject);
        obj.transform.SetParent(transform);
        
        passiveItems.Add(obj.GetComponent<PassiveItemBase>());
        // パッシブアイテムが増えたら補正値を更新する
        UpdateCorrectionValue();
    }

    // 補正値を更新し適用する
    private void UpdateCorrectionValue(){
        // それぞれの補正値
        var totalPriceDiff = GetTotalPriceDiff();
        var totalMagnificationDiff = GetTotalMagnificationDiff();
        var totalSpawnRatesDiff = GetTotalSpawnRatesDiff();

        // 各マネージャーの更新処理
        scoreMan.SetPriceAndMagnification(totalPriceDiff, totalMagnificationDiff);
        sweetsTypeMan.SetSweetsSpawnRates(totalSpawnRatesDiff);
    }

    // 各パッシブのスイーツ単体価値補正を合計し、各スイーツのリストで返す
    private int[] GetTotalPriceDiff(){
        var value = new int[5]{0,0,0,0,0};
        foreach(var passive in passiveItems){
            for(int i=0;i<value.Length;i++){
                value[i] += passive.priceDiff[i];
            }
        }
        return value;
    }

    // 各パッシブのスイーツ倍率補正を合計し、各スイーツのリストで返す
    private float[] GetTotalMagnificationDiff(){
        var value = new float[5]{0,0,0,0,0};
        foreach(var passive in passiveItems){
            for(int i=0;i<value.Length;i++){
                value[i] += passive.magnificationDiff[i];
            }
        }
        return value;
    }

    // 各パッシブのスイーツ出現率補正を合計し、各スイーツのリストで返す
    private int[] GetTotalSpawnRatesDiff(){
        var value = new int[5]{0,0,0,0,0};
        var tmpData = new PassiveRequireData(
                SweetsEnum.None,
                0,
                nowMoney,
                year,
                season,
                remainingMove,
                sweetsTypeMan.sweetsSpawnRates,
                passiveItems,
                deleteBuffer.deleteSweetsBuffer,
                itemMan.itemUsage
            );
        foreach(var passive in passiveItems){
            var tmpSpawnRates = passive.SpecialSpawnRates(tmpData);
            for(int i=0;i<value.Length;i++){
                value[i] += passive.spawnRatesDiff[i];
                value[i] += tmpSpawnRates[i];
            }
        }
        return value;
    }

    // 現在の状況を取得する関数
    private void UpdateConditions(){
        nowMoney = moneyMan.nowMoney;
        year = turnMan.nowYear;
        season = turnMan.nowSeason;
        remainingMove = turnMan.remainingMoves;
    }

    // 参照用アイテム使用状況・アイテム出現数のリセット
    public void resetCondition(){
        itemMan.resetItemUsage();
        sweetsTypeMan.SetSweetsSpawnRates(GetTotalSpawnRatesDiff());
    }

    //--------------------------------------------------
    // ここから下 特別補正値を返す関数(ほとんど同一)
    //--------------------------------------------------

    public int GetSpecialSweetsPrice(SweetsEnum type, int count, bool activeRand = true){
        // 最初に参照する変数を更新
        UpdateConditions();
        int value = 0;
        // 必要なデータの作成
        var tmpData = new PassiveRequireData(
            type,
            count,
            nowMoney,
            year,
            season,
            remainingMove,
            sweetsTypeMan.sweetsSpawnRates,
            passiveItems,
            deleteBuffer.deleteSweetsBuffer,
            itemMan.itemUsage,
            activeRand
        );
        foreach(var passive in passiveItems){
            // 各関数の特別な処理を実行し、加算していく
            value += passive.SpecialSweetsPrice(tmpData);
        }
        return value;
    }

    public int GetBonusPrice(SweetsEnum type, int count, bool activeRand = true){
        // 最初に参照する変数を更新
        UpdateConditions();
        int value = 0;
        // 必要なデータの作成
        var tmpData = new PassiveRequireData(
            type,
            count,
            nowMoney,
            year,
            season,
            remainingMove,
            sweetsTypeMan.sweetsSpawnRates,
            passiveItems,
            deleteBuffer.deleteSweetsBuffer,
            itemMan.itemUsage,
            activeRand
        );
        foreach(var passive in passiveItems){
            value += passive.BonusPrice(tmpData);
        }
        return value;
    }

    public float GetSpecialSweetsMagnification(SweetsEnum type, int count, bool activeRand = true){
        // 最初に参照する変数を更新
        UpdateConditions();
        float value = 0;
        // 必要なデータの作成
        var tmpData = new PassiveRequireData(
            type,
            count,
            nowMoney,
            year,
            season,
            remainingMove,
            sweetsTypeMan.sweetsSpawnRates,
            passiveItems,
            deleteBuffer.deleteSweetsBuffer,
            itemMan.itemUsage,
            activeRand
        );
        foreach(var passive in passiveItems){
            value += passive.SpecialSweetsMagnification(tmpData);
        }
        return value;
    }

    public float GetBonusMagnification(SweetsEnum type, int count, bool activeRand = true){
        // 最初に参照する変数を更新
        UpdateConditions();
        float value = 0;
        // 必要なデータの作成
        var tmpData = new PassiveRequireData(
            type,
            count,
            nowMoney,
            year,
            season,
            remainingMove,
            sweetsTypeMan.sweetsSpawnRates,
            passiveItems,
            deleteBuffer.deleteSweetsBuffer,
            itemMan.itemUsage,
            activeRand
        );
        foreach(var passive in passiveItems){
            value += passive.BonusMagnification(tmpData);
        }
        return value;
    }

}
