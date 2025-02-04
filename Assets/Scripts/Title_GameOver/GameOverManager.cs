using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    // 背景を暗くするフェーダー
    [SerializeField] private DisplayFader fader;
    // 上部のGAMEOVERという文字のトランスフォーム
    [SerializeField] private RectTransform gameOverTextTrans;
    // 最初のゲームオーバー表示の位置
    private Vector2 gameOverFirstPos;
    
    // 成績表示のキャンバスグループ
    [SerializeField] private CanvasGroup scoreCanv;
    // 最終期を表示するテキスト
    [SerializeField] private TextMeshProUGUI finalSeasonText;
    // 総売り上げを表示するテキスト
    [SerializeField] private TextMeshProUGUI totalScoreText;
    // ハイスコア表示のトランスフォーム
    [SerializeField] private RectTransform highScoreTrans;
    
    // 成績を取り出す元のスクリプタブルオブジェクト
    [SerializeField] private ScriptableEndData endData;

    // タッチでタイトルに戻るというテキストの明滅管理
    [SerializeField] private BlinkObject toTitleBlink;
    // シーン遷移管理
    [SerializeField] private GameOverSceneChanger sceneChanger;

    [Space]
    // 演出開始までのディレイ
    [SerializeField] private float startDelay = 0.3f;
    // 背景を暗くするまでの時間
    [SerializeField] private float dispFadeDuration = 0.5f;
    // ゲームオーバーテキストが落ちてくるまでの時間
    [SerializeField] private float gameOverTextDuration = 0.6f;
    // スコア表示までのディレイ
    [SerializeField] private float scoreDispDelay = 0.5f;
    // スコア表示の時間
    [SerializeField] private float scoreDispDuration = 1f;
    // スコア表示後に、タイトルに戻れるようになるまでにかかる時間
    [SerializeField] private float canGoTitleDelay = 1f;


    void Start()
    {
        StartCoroutine(StartGameover());
    }

    private IEnumerator StartGameover(){
        // 準備(初期設定)
        PrepareGameover();
        // 最初に少しディレイを挟む
        yield return new WaitForSeconds(startDelay);
        // ゲームオーバーBGMを流し、画面を暗く
        SoundManager.I.PlayBGM("GameOver");
        yield return fader.MyDOFade(0.6f, dispFadeDuration);
        // ゲームオーバーテキストを落とす
        yield return gameOverTextTrans.DOAnchorPos(gameOverFirstPos, gameOverTextDuration).SetEase(Ease.OutBounce).WaitForCompletion();
        // 成績表示のシークエンス
        Tween dispScoreTween;
        dispScoreTween = scoreCanv.DOFade(1, scoreDispDuration).SetDelay(scoreDispDelay);
        // 成績が1位だったらハイスコア更新と表示
        if(endData.ranking == 1){
            yield return dispScoreTween.WaitForCompletion();
            dispScoreTween = PowerFadeIn(highScoreTrans);
        }
        yield return dispScoreTween.WaitForCompletion();
        // 表示完了から少し時間挟む
        yield return new WaitForSeconds(canGoTitleDelay);
        // タイトルに戻れるように
        sceneChanger.canSceneChange = true;
        toTitleBlink.StartBlink();
    }

    // ゲームオーバー演出の準備
    private void PrepareGameover(){
        sceneChanger.canSceneChange = false;
        // ゲームオーバーテキストの初期位置
        gameOverFirstPos = gameOverTextTrans.anchoredPosition;
        gameOverTextTrans.anchoredPosition += new Vector2(0, ScreenSizeGetter.I.GetTrueScreenSize().y);
        // スコア表示部分の初期設定
        scoreCanv.alpha = 0;
        highScoreTrans.gameObject.SetActive(false);
        // 最終期テキスト
        finalSeasonText.text = $"<size=80%>最終期</size>\n{endData.data.lastYear}年目  {TurnManager.GetSeasonString(endData.data.lastSeason)}";
        // 総売り上げテキスト
        totalScoreText.text = $"<size=80%>総売り上げ</size>\n{endData.data.totalSales:N0}<size=70%>アマ</size>";
    }

    private Sequence PowerFadeIn(RectTransform targetTrans){
        targetTrans.gameObject.SetActive(true);
        // 画面手前からドンって感じに演出
        var defScale = targetTrans.localScale;
        targetTrans.localScale = defScale * 5;
        
        var fadeInSeq = DOTween.Sequence();
        // 元のサイズに縮小
        fadeInSeq.Append(targetTrans.DOScale(defScale, 0.2f).SetEase(Ease.OutExpo));
        // 少し揺らす
        fadeInSeq.Join(targetTrans.DOShakeAnchorPos(0.5f, 20, 10, 3));

        return fadeInSeq;
    }
}
