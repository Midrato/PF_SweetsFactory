using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccumCondition{
    General,
    GameOver,
    NoMemGameOver,
    Clear
}

public class MainSceneDataManager : DataManager
{
    static public MainSceneDataManager MCInst;

    // データのセーブに必要なマネージャー類
    [SerializeField] private TurnManager turnMan;
    [SerializeField] private MoneyManager moneyMan;
    [SerializeField] private PassiveHolder passiveHolder;
    [SerializeField] private ItemManager itemMan;
    [SerializeField] private PuzzleManager puzzleMan;

    // 最後にセーブする年
    [field:SerializeField]public int lastYearToSave {get;private set;} = 10;

    // 派生クラスとしてシングルトン登録
    protected override void SingleTon()
    {
        if(MCInst == null){
            Inst = this;
            MCInst = this;
        }else{
            Destroy(this);
        }
    }

    /// <summary>
    /// 現在のプレイデータをセーブ
    /// </summary>
    public void SaveNowPlayingData(){
        var lData = LoadPlayingData();
        // マネージャーからセーブデータに必要な情報を抽出
        var data = new PlayingData();
        data.SetPlayingData(
            turnMan.nowYear,
            turnMan.nowSeason,
            turnMan.remainingMoves,
            moneyMan.nowMoney,
            moneyMan.totalMoney,
            passiveHolder.EncodeHavingPassives(),
            itemMan.HaveItemNumber,
            itemMan.itemUsage,
            puzzleMan.EncodeBoard(),
            lData.lotteryPassiveLists
        );

        SavePlayingData(data);
    }

    public void SaveLotteryPassives(List<int> modulesId){
        var data = LoadPlayingData();
        // 現在の抽選モジュールリストを取得
        var lotteryList = data.lotteryPassiveLists;
        // モジュールリストを加える
        lotteryList.AddRange(modulesId);
        // データのモジュールリストを上書き
        data.lotteryPassiveLists = lotteryList;
        SavePlayingData(data);
    }

    // index番目の長さcountの抽選リストを取得
    public List<int> LoadLotteryPassives(int index, int count){
        var data = LoadPlayingData();
        // リストの長さが要求に満たなかったらnullを返す
        if(data.lotteryPassiveLists.Count < index*count + count)return null;
        return data.lotteryPassiveLists.GetRange(index*count, count);
    }

    /// <summary>
    /// 抽出パッシブリストをリセット
    /// </summary>
    public void ResetLotteryPassiveList(){
        var data = LoadPlayingData();
        data.lotteryPassiveLists = new List<int>();
        SavePlayingData(data);
    }

    /// <summary>
    /// 累計データをセーブ
    /// </summary>
    /// <param name="isGameOverSave">ゲームオーバー時の記録保存処理かどうか</param>
    /// <param name="isClear">ゲームクリア(最終四季を乗り越えたか)</param>
    public void SaveNowAccumulativeData(bool isGameOverSave, bool isClear=false){
        // セーブの種類を演算
        var condition = new AccumCondition();
        if(isClear)condition = AccumCondition.Clear;
        else if(isGameOverSave){
            // セーブする最後の年を越しているならセーブしない
            if(turnMan.nowYear > lastYearToSave)condition = AccumCondition.NoMemGameOver;
            else condition = AccumCondition.GameOver;
        }else condition = AccumCondition.General;

        var dateGetter = new DayStringGetter();

        // 現在のデータから終了データ生成
        var endPlayData = new EndPlayData();
        endPlayData.SetEndPlayData(turnMan.nowYear, turnMan.nowSeason, moneyMan.totalMoney, dateGetter.GetNowDayString());

        SaveAccumulativeData(endPlayData, condition);
    }

    /// <summary>
    /// 取得したパッシブアイテムをセーブする
    /// </summary>
    /// <param name="item"></param>
    public void SaveFindPassive(PassiveItemBase item){
        var pastData = LoadAccumulativeData();
        pastData.AddFindPassive(item.itemID);
        SaveAccumData(pastData);
    }

}
