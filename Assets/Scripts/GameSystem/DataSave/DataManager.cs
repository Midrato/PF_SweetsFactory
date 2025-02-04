using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // 様々な場所からロードを行うためシングルトンを採用
    static public DataManager Inst;

    // 保存先のファイル名たち
    private static string playingDataFilepath;
    private static string accumulativeDataFilepath;
    private static string optionDataFilepath;

    private readonly string playingDataName = "/PlayingData.json";
    private readonly string accumulativeDataName = "/AccumulativeData.json";
    private readonly string optionDataName = "/OptionData.json";

    // 一時的な回のデータの保存用
    [SerializeField] private ScriptableEndData currentEndData;

    // 初回スタート時のデータ
    [SerializeField] private PlayingData awakeData;


    void Awake(){
        SingleTon();

        playingDataFilepath = Application.persistentDataPath + playingDataName;
        accumulativeDataFilepath = Application.persistentDataPath + accumulativeDataName;
        optionDataFilepath = Application.persistentDataPath + optionDataName;

        // セーブが無いなら初期データ生成
        if(!File.Exists(playingDataFilepath)){
            SaveAwakeData();
        }
    }

    protected virtual void SingleTon(){
        if(Inst == null){
            Inst = this;
        }else{
            Destroy(this);
        }
    }

    /// <summary>
    /// プレイデータをセーブ
    /// </summary>
    public void SavePlayingData(PlayingData saveData){

        string json = JsonUtility.ToJson(saveData);
        // 上書き設定でファイルを開く
        StreamWriter writer = new StreamWriter(playingDataFilepath, false);
        writer.WriteLine(json);
        writer.Close();
        Debug.Log("セーブ完了");
    }

    /// <summary>
    /// スコアの累計データをセーブ
    /// </summary>
    /// <param name="isGameOverSave">ゲームオーバー時の記録保存処理かどうか</param>
    public void SaveAccumulativeData(EndPlayData saveData, AccumCondition condition){

        // 現セーブデータを吸い出し、それを編集
        AccumulativeData data;
        if(File.Exists(accumulativeDataFilepath)){
            data = LoadAccumulativeData();
        }else{
            Debug.Log("累計データ新規作成");
            data = new AccumulativeData();
        }

        // ゲームオーバー時のセーブ処理ならば
        switch(condition){
            case AccumCondition.GameOver : 
                // データをランキングに加え、順位を得る
                int ranking = data.AddRanking(saveData);
                // スクリプタブルオブジェクトにこの回のセーブデータを収める
                currentEndData.SetData(saveData, ranking);
                break;
            case AccumCondition.NoMemGameOver :
                currentEndData.SetData(saveData, data.leaveRankNum);
                break;
            case AccumCondition.Clear :
                saveData.isClear = true;
                data.AddRanking(saveData);
                break;
        }

        SaveAccumData(data);
    }

    public void SaveAccumData(AccumulativeData saveData){
        string json = JsonUtility.ToJson(saveData);
        Debug.Log(json);
        // 上書き設定でファイルを開く
        StreamWriter writer = new StreamWriter(accumulativeDataFilepath, false);
        writer.WriteLine(json);
        writer.Close();

        Debug.Log("累計データセーブ完了");
    }

    /// <summary>
    /// プレイデータをロード
    /// </summary>
    /// <returns></returns>
    public PlayingData LoadPlayingData(){
        // データが存在するかチェック
        if(!File.Exists(playingDataFilepath)){
            // 新規セーブデータ作成
            SavePlayingData(new PlayingData());
        }

        var reader = new StreamReader(playingDataFilepath);
        string json = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<PlayingData>(json);
    }

    /// <summary>
    /// 累計データをロード
    /// </summary>
    /// <returns></returns>
    public AccumulativeData LoadAccumulativeData(){
        // データが存在するかチェック
        if(!File.Exists(accumulativeDataFilepath)){
            SaveAccumulativeData(new EndPlayData(), AccumCondition.General);
        }

        var reader = new StreamReader(accumulativeDataFilepath);
        string json = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<AccumulativeData>(json);
    }

    /// <summary>
    /// セーブデータを削除する
    /// </summary>
    public void ClearData(){
        File.Delete(playingDataFilepath);
    }

    private void SaveAwakeData(){
        string json = JsonUtility.ToJson(awakeData);
        // 上書き設定でファイルを開く
        StreamWriter writer = new StreamWriter(playingDataFilepath, false);
        writer.WriteLine(json);
        writer.Close();
        Debug.Log("初期セーブ完了");
    }

    public void SaveOptionData(OptionData data){
        string json = JsonUtility.ToJson(data);
        // 上書き設定でファイルを開く
        StreamWriter writer = new StreamWriter(optionDataFilepath, false);
        writer.WriteLine(json);
        writer.Close();
    }

    public OptionData LoadOptionData(){
        // データが存在するかチェック
        if(!File.Exists(optionDataFilepath)){
            SaveOptionData(new OptionData());
        }

        var reader = new StreamReader(optionDataFilepath);
        string json = reader.ReadToEnd();
        reader.Close();

        var data = JsonUtility.FromJson<OptionData>(json);
        data.CheckValue();

        return data;
    }
}
