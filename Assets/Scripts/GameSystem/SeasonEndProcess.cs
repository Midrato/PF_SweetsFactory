using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SeasonEndProcess : MonoBehaviour
{   
    // 残り操作数表示のターンマネージャー
    [SerializeField] private TurnManager turnMan;
    // 今期の目標取得用マネージャー
    [SerializeField] private PeriodGoalManager goalMan;
    // 収集金額取得用のマネージャー
    [SerializeField] private MoneyManager moneyMan;
    // 目標金額を得るためのマネージャー
    [SerializeField] private PeriodGoalManager goalMoneyMan;
    // 操作不可を設定するためのマネージャ
    [SerializeField] private PuzzleManager puzzleMan;

    // パッシブの保持を行うホルダー
    [SerializeField] private PassiveHolder holder;
    // パッシブを抽選する
    [SerializeField] private PassiveLottery lottery;
    

    // 今シーズンの結果を表示するプレハブ
    [SerializeField] private GameObject seasonResultWindow;
    [SerializeField] private Transform canvasTransform;
    // シーズン遷移画面のプレハブ
    [SerializeField] private GameObject seasonChangeWindow;

    // ゲームオーバー画面遷移用
    [SerializeField] private GoGameOver goGameOver;

    // メニュー遷移可否設定用
    [SerializeField] private MenuChanger menuChanger;

    // シーズン終了処理を行っているか
    public bool isSeasonEnding = false;

    // Update is called once per frame
    void Update()
    {
        if(turnMan.remainingMoves == 0 && !isSeasonEnding){
            EndSeason();
        }
    }

    // シーズンを終わらせる
    public void EndSeason(){
        isSeasonEnding = true;
        // パズルを操作できない状態にする
        puzzleMan.SetCannotPuzzle(true);
        // メニュー変更も出来ない状態にする
        menuChanger.canChangeMenu = false;

        int resultMoney = moneyMan.nowMoney;
        int goalMoney = goalMan.NowGoalMoney();

        // シーズン終了処理画面のオブジェクト生成
        var endWindow = Instantiate(seasonResultWindow);
        var resultDisplayer = endWindow.GetComponent<ResultDisplayer>();

        endWindow.transform.SetParent(canvasTransform, false);

        // リザルトウィンドウ用にシーズン終了マネージャー自身をセット
        resultDisplayer.SetSeasonEndProcess(GetComponent<SeasonEndProcess>());
        // リザルトを表示させる
        StartCoroutine(resultDisplayer.ShowResult(turnMan,resultMoney, goalMoney));


        if(resultMoney >= goalMoney){
            Debug.Log("Success!");
        }else{
            Debug.Log("Fail...");
        }

    }

    // シーズン終了後、目標達成時の選択。遷移する画面を呼び出す(成功ボタン押下時)
    public void Success_CallSeasonChangeWindow(){
        // ウィンドウを召喚し、コンポーネント取得などをする
        var windowObject = Instantiate(seasonChangeWindow);
        windowObject.transform.SetParent(canvasTransform, false);

        var _seasonChange = windowObject.GetComponent<SeasonChangeWindow>();
        // コンポーネントをセットし、動作を始める
        _seasonChange.SetComponent(turnMan, goalMoneyMan, holder, lottery, GetComponent<SeasonEndProcess>());
        StartCoroutine(_seasonChange.StartSeasonChange());

        // リザルト画面のBGMを流す
        SoundManager.I.PlayBGM("Result");
    }

    // シーズン終了後、失敗時に呼び出す
    public void Fail_CallGameOverWindow(){
        StartCoroutine(goGameOver.ChangeToFailScene());
    }

    // シーズン終了後、広告を見る際呼び出す
    public void AD_CallAD(){

    }

    // 次のシーズンを開始する
    public void StartNextSeason(){
        // 目標金額の一定割合を投資として、所持金から引く
        int subMoney = goalMoneyMan.CalcSubMoney();
        moneyMan.SubMoney(subMoney);
        // 次のターンへ
        turnMan.GoNextSeason();
        isSeasonEnding = false;
        // パズルも可能に
        puzzleMan.SetCannotPuzzle(false);
        // メニュー変更も可能に
        menuChanger.canChangeMenu = true;
    }
}
