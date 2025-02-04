using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ResultEnum{
    Success,
    Fail,
    FailAD
}

public class SeasonChangeWindow : MonoBehaviour
{
    // ターン管理用
    private TurnManager turnMan;
    // 四半期毎の目標金額を取得する
    private PeriodGoalManager goalMoneyMan;

    // 画面上のテキスト
    [SerializeField] private TextMeshProUGUI headText;
    private MyDOText DOHeadText;

    [Header("シーズン変化演出用")]

    // シーズンを表す地球の画像のオブジェクト
    [SerializeField]private GameObject seasonEarth;
    private RectTransform seasonEarthTrans;
    // 季節変化前/後を収めるためのテキスト
    [SerializeField] private TextMeshProUGUI[] seasonText = new TextMeshProUGUI[2];
    private RectTransform[] seasonTextTrans = new RectTransform[2];

    [Space]
    [Header("目標値・投資値表示用")]
    // 目標値・投資量表示のテキスト
    [SerializeField] private TextMeshProUGUI[] moneyChangeText = new TextMeshProUGUI[4];
    private RectTransform[] moneyChangeTextTrans = new RectTransform[4];

    [Space]
    [Header("パッシブ選択画面用")]
    // 必要なパッシブアイテムコンポーネント
    [SerializeField] private ItemSelector itemSelector;
    private PassiveHolder passiveHolder;
    private PassiveLottery passiveLottery;
    
    [Space]
    [Header("入手パッシブ確認画面用")]
    // アイテム確認ウィンドウオブジェクト
    [SerializeField] private GameObject itemConfirmWindow;
    // パッシブ選択で入手したアイテム
    private PassiveItemBase gotPassiveItem = null;
    
    [Space]
    [Header("シーズンチェンジ終了処理用")]
    private SeasonEndProcess seasonEndProcess;

    [Space]
    [Header("最終シーズンクリア時の保存画面表示用")]
    [SerializeField] private GameObject finalMemWindow;
    
    
    // フェードイン/アウトするフェーダー
    private DisplayFader dispFader;

    // Tween用パラメーター
    private float headTxtDuration = 0.6f;

    // アニメーション中か
    private bool isAnimating = false;
    // 倍速時の再生速度
    private float animHighSpeed = 2f;
    private GeneralTouchSensor tSens;

    void Awake(){
        DOHeadText = headText.GetComponent<MyDOText>();
        tSens = new GeneralTouchSensor();
    }

        void Update(){
        // タッチ中は速度animHighSpeed倍
        if(tSens.GetTouch() && isAnimating){
            Time.timeScale = animHighSpeed;
        }else Time.timeScale = 1;
    }

    // 必要なコンポーネントをセットしてもらう関数
    public void SetComponent(TurnManager _turnMan, PeriodGoalManager _goalMoneyMan, PassiveHolder _holder, PassiveLottery _lottery, SeasonEndProcess _seasonEndProcess){
        turnMan = _turnMan;
        goalMoneyMan = _goalMoneyMan;
        passiveHolder = _holder;
        passiveLottery = _lottery;
        seasonEndProcess = _seasonEndProcess;
    }

    // シーズン変化演出を開始する
    public IEnumerator StartSeasonChange(){
        
        // 演出前の背景フェードイン
        yield return StartCoroutine(StartSesCh());
        if(turnMan.nowYear == MainSceneDataManager.MCInst.lastYearToSave && turnMan.nowSeason == SeasonEnum.Winter){
            yield return StartCoroutine(FinalSeasonMem());
        }
        // 最初の次シーズン表記
        yield return StartCoroutine(SeasonChange());
        // 次の目標金額表示
        yield return StartCoroutine(DispNextGoal());
        // パッシブアイテム選択画面の表示
        yield return StartCoroutine(SelectPassiveItem());
        if(gotPassiveItem != null){
            yield return StartCoroutine(ConfirmPassiveItem());
        }
        yield return StartCoroutine(EndSeasonChange());
        // 全処理が終了したなら破壊
        Destroy(this.gameObject);
    }

    private IEnumerator StartSesCh(){
        // 各表示物の初期設定
        seasonText[0].color = new Color(seasonText[0].color.r, seasonText[0].color.g, seasonText[0].color.b, 0);
        headText.text = "";
        // シーズン遷移背景をフェードイン
        // アニメーション開始
        isAnimating = true;
        dispFader = GetComponent<DisplayFader>();
        yield return dispFader.MyDOFade(1, 0.7f).WaitForCompletion();
    }

    private IEnumerator FinalSeasonMem(){
        // 大事な演出のためスキップ不可
        isAnimating = false;
        // 最終結果の表示を行う
        var memWin = Instantiate(finalMemWindow, transform, false);
        var _memWin = memWin.GetComponent<FSMemWindow>();
        yield return StartCoroutine(_memWin.ShowResult());
        Destroy(memWin);
    }

    public IEnumerator SeasonChange(){
        /// 各テキスト・地球の角度変更
        /// 全体フェードイン→地球が下から上に(画像の中央が画面下くらい)→季節テキストを左から右へ送る
        /// →地球を下に下げ、遷移後テキストを少し大きくする
        /// →テキストを消す 
        
        // 準備
        PrepareSeasonChange();
        // アニメーション開始
        isAnimating = true;

        var SCSeq = DOTween.Sequence();
        SCSeq.Append(DOHeadText.DOAddText(turnMan.nowYear + "年目", 1.2f));
        SCSeq.Join(seasonText[0].DOFade(1, 1f).SetEase(Ease.OutQuad));
        // 画面上に季節地球が登場
        SCSeq.Append(seasonEarthTrans.DOAnchorPosY(0, 0.6f).SetEase(Ease.OutBack));

        yield return SCSeq.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);

        // 二つのテキストオブジェクトの横軸距離
        float textDistance = seasonTextTrans[0].anchoredPosition.x - seasonTextTrans[1].anchoredPosition.x;

        var seasonRotate = DOTween.Sequence();        
        seasonRotate.Join(seasonEarthTrans.DOLocalRotate(Vector3.back * 90, 1f));
        seasonRotate.Join(seasonTextTrans[0].DOAnchorPosX(textDistance, 1f));
        seasonRotate.Join(seasonTextTrans[1].DOAnchorPosX(textDistance, 1f));

        // tween設定
        seasonRotate.SetEase(Ease.InOutExpo).SetRelative();
        
        // 次の年に移る文字の表示
        if(turnMan.nowSeason == SeasonEnum.Winter){
            var headTrans = headText.GetComponent<RectTransform>();
            var beforeScale = headTrans.localScale;
            // 0.3秒後、地球が回るくらいに一瞬巨大化させ、その瞬間に年表記を変更
            seasonRotate.Join(headTrans.DOScale(beforeScale * 1.5f, 0.3f).SetEase(Ease.OutCubic).SetDelay(0.3f).OnStart(() => headText.text = turnMan.GetNextYear() + "年目").OnComplete(() => {
                headTrans.DOScale(beforeScale, 0.2f).SetEase(Ease.OutCubic);
            }));
        }

        yield return seasonRotate.WaitForCompletion();

        // 地球を下げて、文字を少し大きく
        var finalSeasonEffect = DOTween.Sequence();
        finalSeasonEffect.Join(seasonEarthTrans.DOAnchorPosY(-seasonEarthTrans.sizeDelta.y, 0.7f).SetEase(Ease.InBack));
        finalSeasonEffect.Join(seasonTextTrans[1].DOScale(1.5f * seasonTextTrans[1].localScale, 0.4f).SetEase(Ease.InBack).SetDelay(0.4f));
        // 文字のフェードアウト
        finalSeasonEffect.Append(seasonText[1].DOFade(0, 1f).SetDelay(1f));
        finalSeasonEffect.Join(DOHeadText.DORemove(headTxtDuration));

        yield return finalSeasonEffect.WaitForCompletion();
        // 使い終わったオブジェクトを破壊
        Destroy(seasonEarth);
        Destroy(seasonText[0].gameObject);
        Destroy(seasonText[1].gameObject);
    }

    // シーズン遷移演出の準備
    private void PrepareSeasonChange(){
        // 必要なコンポーネント取得
        seasonEarthTrans = seasonEarth.GetComponent<RectTransform>();
        seasonTextTrans[0] = seasonText[0].GetComponent<RectTransform>();
        seasonTextTrans[1] = seasonText[1].GetComponent<RectTransform>();

        // 次のシーズン
        var nextSeason = (SeasonEnum)((int)turnMan.nowSeason+1);
        var nextYear = turnMan.nowYear;

        if(nextSeason == SeasonEnum.None){
            nextSeason = SeasonEnum.Spring;
            nextYear++;
        }
        Debug.Log(TurnManager.GetSeasonString(nextSeason));

        // テキスト類の設定
        seasonText[0].text = TurnManager.GetSeasonString(turnMan.nowSeason);
        // 最初は透明
        var sesCol = TurnManager.GetSeasonColor(turnMan.nowSeason);
        seasonText[0].color = new Color(sesCol.r, sesCol.g, sesCol.b, 0);
        seasonText[1].text = TurnManager.GetSeasonString(nextSeason);
        seasonText[1].color = TurnManager.GetSeasonColor(nextSeason);

        // 季節地球を現在のシーズンに傾ける
        seasonEarth.transform.rotation = Quaternion.AngleAxis(90 * (int)turnMan.nowSeason, Vector3.back);
    }
    
    private IEnumerator DispNextGoal(){
        /// 上のテキスト拡大登場→元の目標金額がフェードインし、値が増えていって次の目標金額へ変化する
        /// 真ん中のテキスト拡大登場→減る金額が0の状態でフェードイン、カウントアップし減る金額へ
        /// 最後に全部フェードアウト
        
        // 準備兼変更前のスケール取得
        var beforeScale = PrepareDispNextGoal();
        
        // 次のシーズンの季節・年
        var nextSeason = (SeasonEnum)((int)turnMan.nowSeason+1);
        var nextYear = turnMan.nowYear;

        if(nextSeason == SeasonEnum.None){
            nextSeason = SeasonEnum.Spring;
            nextYear++;
        }

        var dispSeq = DOTween.Sequence();
        
        // テキストの表示(次目標値)
        dispSeq.Append(DOHeadText.DOAddText("目標達成！", headTxtDuration).SetEase(Ease.OutCirc));
        dispSeq.Append(moneyChangeTextTrans[0].DOScale(beforeScale[0], 0.6f).SetEase(Ease.OutCirc));
        dispSeq.Join(moneyChangeText[1].DOFade(1, 0.6f).SetEase(Ease.OutCirc).SetDelay(0.4f));
        // 値のTween
        dispSeq.Append(DOTween.To(goalMoneyMan.NowGoalMoney, (val) =>{
            moneyChangeText[1].text = "<color=yellow>" + Mathf.RoundToInt(val) + "</color><size=70%>アマ</size>";
        }, goalMoneyMan.GetGoalMoney(nextYear, nextSeason), 0.75f).SetEase(Ease.OutExpo));

        // テキストの表示(投資額)
        dispSeq.Append(moneyChangeTextTrans[2].DOScale(beforeScale[1], 0.6f).SetEase(Ease.OutCirc).SetDelay(0.75f));
        dispSeq.Append(moneyChangeText[3].DOFade(1, 0.6f).SetEase(Ease.OutCirc));
        // 値のTween
        dispSeq.Append(DOTween.To(() => 0, (val) =>{
            moneyChangeText[3].text = "<color=red>-" + Mathf.RoundToInt(val) + "</color><size=70%>アマ</size>";
        }, goalMoneyMan.CalcSubMoney(), 0.75f).SetEase(Ease.OutExpo));

        float screenWidth = ScreenSizeGetter.I.GetTrueScreenSize().x;

        // 全テキストを左にスクロールして退場
        dispSeq.Append(moneyChangeTextTrans[0].DOAnchorPosX(-screenWidth, 1.2f).SetEase(Ease.InBack).SetDelay(0.7f));
        dispSeq.Join(moneyChangeTextTrans[1].DOAnchorPosX(-screenWidth, 1.2f).SetEase(Ease.InBack));
        dispSeq.Join(moneyChangeTextTrans[2].DOAnchorPosX(-screenWidth, 1.2f).SetEase(Ease.InBack));
        dispSeq.Join(moneyChangeTextTrans[3].DOAnchorPosX(-screenWidth, 1.2f).SetEase(Ease.InBack));
        // ヘッダーテキストを消す
        dispSeq.Join(DOHeadText.DORemove(headTxtDuration).SetEase(Ease.OutCirc));
        
        yield return dispSeq.WaitForCompletion();
        // アニメーション終了
        isAnimating = false;
    }

    // 次ゴールの表示処理の準備。スケール変更前の値を返す
    private Vector2[] PrepareDispNextGoal(){
        // 必要なコンポーネント取得・オブジェクトの有効化
        for(int i=0;i<moneyChangeText.Length;i++){
            moneyChangeTextTrans[i] = moneyChangeText[i].GetComponent<RectTransform>();
            moneyChangeText[i].gameObject.SetActive(true);
        }

        // スケール変更前の値を保存する
        var beforeScale = new Vector2[2];
        beforeScale[0] = moneyChangeTextTrans[0].localScale;
        beforeScale[1] = moneyChangeTextTrans[2].localScale;

        // 文字のスケールを0に
        moneyChangeTextTrans[0].localScale = Vector2.zero;
        // 文字を透明に
        moneyChangeText[1].text = "<color=yellow>" + goalMoneyMan.NowGoalMoney() + "</color><size=70%>アマ</size>";
        moneyChangeText[1].color = new Color(moneyChangeText[1].color.r, moneyChangeText[1].color.g, moneyChangeText[1].color.b, 0);
        // 文字のスケールを0に
        moneyChangeText[2].text = "今期目標の<size=120%><color=red>" + (int)(goalMoneyMan.investFromGoalRatio*100) + "%</color></size>を\n投資します";
        moneyChangeTextTrans[2].localScale = Vector2.zero;
        // 文字を透明に
        moneyChangeText[3].text = "";
        moneyChangeText[3].color = new Color(moneyChangeText[3].color.r, moneyChangeText[3].color.g, moneyChangeText[3].color.b, 0);

        return beforeScale;
    }

    // パッシブアイテムを選択する
    private IEnumerator SelectPassiveItem(){
        // コンポーネントをセットする
        itemSelector.SetNeedComponents(passiveHolder, passiveLottery);
        // 入手可能パッシブが無いならスキップ
        if(!itemSelector.isRemainingPassive()){
            PrepareSelectPassiveItem();

            DOHeadText.DOAddText("モジュール選択", headTxtDuration);
            // 選択終了まで待つ
            yield return StartCoroutine(itemSelector.MakeSelection(3));
            // 確定したアイテムを取得する
            gotPassiveItem = itemSelector.decisionItem;
            // 獲得アイテムを図鑑に登録
            MainSceneDataManager.MCInst.SaveFindPassive(gotPassiveItem);

            // アニメーション開始
            isAnimating = true;
            yield return DOHeadText.DORemove(headTxtDuration).WaitForCompletion();
        }else yield return null;
    }

    // パッシブアイテムセットの準備
    private void PrepareSelectPassiveItem(){
        // 有効化
        itemSelector.gameObject.SetActive(true);
    }

    // 入手パッシブの確認画面
    private IEnumerator ConfirmPassiveItem(){
        // アニメーション開始
        isAnimating = true;

        // パッシブ確認画面の召喚
        var confirmWindow = Instantiate(itemConfirmWindow);
        confirmWindow.transform.SetParent(GetComponent<RectTransform>(), false);
        // 画面のトランスフォーム・確認画面制御のコンポーネント取得
        var confirmTrans = confirmWindow.GetComponent<RectTransform>();
        var confirmer = confirmWindow.GetComponent<DisplayConfirm>();

        // ウィンドウに入手パッシブ情報をセット
        confirmer.SetItemData(gotPassiveItem);

        // 一旦大きさをゼロに
        confirmTrans.localScale = Vector2.zero;
        // パッシブ画面登場を待つ
        yield return confirmTrans.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutExpo).WaitForCompletion();
        // ここで上の文字も変化
        DOHeadText.DOAddText("モジュール獲得！", 0.5f);

        // ウィンドウも動作開始
        yield return StartCoroutine(confirmer.StartConfirm());
        // アニメーション終了
        isAnimating = false;

        // ボタンをクリックするまで待機
        yield return StartCoroutine(confirmer.WaitForTouchButton());
        
        // 上部のテキストを削除し終了
        yield return DOHeadText.DORemove(0.7f);
        Debug.Log("パッシブ確認したよ～");
    }

    // シーズン遷移を終わらせるコルーチン
    private IEnumerator EndSeasonChange(){
        // アニメーション開始
        isAnimating = true;
        Sequence endSeq = DOTween.Sequence();
        // 遷移画面全体を上にスクロール
        endSeq.Append(GetComponent<RectTransform>().DOAnchorPosY(ScreenSizeGetter.I.GetTrueScreenSize().y, 2f).SetEase(Ease.InOutQuint).SetDelay(0.5f));
        yield return endSeq.WaitForCompletion();
        // アニメーション終了
        isAnimating = false;
        Time.timeScale = 1;
        seasonEndProcess.StartNextSeason();
        // データをセーブ、抽選パッシブリストをリセット
        MainSceneDataManager.MCInst.SaveNowPlayingData();
        MainSceneDataManager.MCInst.ResetLotteryPassiveList();
    }
}
