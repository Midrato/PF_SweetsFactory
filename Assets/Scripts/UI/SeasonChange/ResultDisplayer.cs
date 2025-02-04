//using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResultDisplayer : MonoBehaviour
{
    // レポート紙の最終表示位置
    [SerializeField] private Vector2Int paperPos = new Vector2Int(22, 0);
    // 結果テンポ表示の待ち時間
    [SerializeField] private float displayTempo = 0.25f;

    [Space]

    // 背景を暗くするフェーダー
    [SerializeField] private DisplayFader fade;
    // リザルト情報が乗っているレポート紙オブジェクト
    [SerializeField] private GameObject resultPaper;

    // レポート紙の上側、年や季節の文
    [SerializeField] private TextMeshProUGUI periodText;
    // レポート紙の真ん中、売上の表示
    [SerializeField] private TextMeshProUGUI valueText;
    // 成功、失敗の画像を表示する欄
    [SerializeField] private CanvasGroup resultCanv;
    // 成功、失敗の画像
    [SerializeField] private GameObject[] resultObjects = new GameObject[2];
    // 自分の子にシーズン遷移させるためのシーズン終了プロセス
    [HideInInspector] public SeasonEndProcess seasonEndProcess;

    [Space]
    //出現しうるボタンパターン
    [SerializeField] private GameObject[] buttonPatterns = new GameObject[3];
    // そこにあるボタンたち
    [SerializeField] private Button[] buttons = new Button[4];
    // 各ボタンの役割タイプ
    [SerializeField] private ResultEnum[] buttonTypes = new ResultEnum[4];

    private RectTransform paperRectTrans;

    // 現在アニメーションをしているか
    private bool isAnimating = false;

    private GeneralTouchSensor tSens;

    // タッチ時の加速時間
    private float animHighSpeed = 2f;

    void Awake(){
        paperRectTrans = resultPaper.GetComponent<RectTransform>();
        tSens = new GeneralTouchSensor();
    }

    void Update(){
        // タッチ中は速度animHighSpeed倍
        if(tSens.GetTouch() && isAnimating){
            Time.timeScale = animHighSpeed;
        }else Time.timeScale = 1;
    }

    public void SetSeasonEndProcess(SeasonEndProcess inst){
        seasonEndProcess = inst;
    }

    public IEnumerator ShowResult(TurnManager turnMan, int resultMoney, int goalMoney){
        // 事前準備
        PrepareResult(turnMan, resultMoney, goalMoney);
        // 目標達成をしたか
        bool isSuccess = resultMoney >= goalMoney;
        ResultEnum thisTimeResult;
        // 成功かどうかで違う画像をセット、結果も保存する
        if(isSuccess){
            resultObjects[0].SetActive(true);
            resultObjects[1].SetActive(false);
            thisTimeResult = ResultEnum.Success;
        }else{
            resultObjects[0].SetActive(false);
            resultObjects[1].SetActive(true);
            thisTimeResult = ResultEnum.Fail;
        }
        
        // アニメーションスタート
        isAnimating = true;

        // 音楽をフェードアウト
        SoundManager.I.DOFadeOutBGM(4f);

        // 画面を暗く
        fade.MyDOFade(0.7f, 0.5f);
        
        yield return new WaitForSeconds(1f);

        var paperSeq = DOTween.Sequence();
        // レポートペーパーを落とす
        paperSeq.Append(paperRectTrans.DOAnchorPosX(paperPos.x, 1f).SetEase(Ease.OutQuint));
        paperSeq.Join(paperRectTrans.DOAnchorPosY(paperPos.y, 1f).SetEase(Ease.OutQuart));
        paperSeq.Join(paperRectTrans.DORotate(4f * Vector3.back, 0.5f).SetEase(Ease.InOutCubic));

        yield return new WaitForSeconds(1.3f);

        // 結果をテンポよく表示
        SoundManager.I.PlaySE("resultDisp1");
        valueText.text += "<align=left>目標：</align>";
        yield return new WaitForSeconds(displayTempo);
        SoundManager.I.PlaySE("resultDisp1");
        valueText.text += "\n<align=right>" + goalMoney.ToString("N0") + "<size=60%>アマ</size></align>";
        yield return new WaitForSeconds(displayTempo);
        SoundManager.I.PlaySE("resultDisp1");
        valueText.text += "\n<color=#E5AB04><align=left>売上：</align>";
        yield return new WaitForSeconds(displayTempo);
        SoundManager.I.PlaySE("resultDisp1");
        valueText.text += "\n<align=right>" + resultMoney.ToString("N0") + "<size=60%>アマ</size></align></color>";
        yield return new WaitForSeconds(displayTempo);
        SoundManager.I.PlaySE("resultDisp1");
        valueText.text += "\n<color=red><align=left>達成率：</align>" + 
                        "\n<align=right>" + ((int)((float)resultMoney/goalMoney * 100)).ToString("N0") + "<size=70%>%</size></align></color>";
        
        // 最終結果のため少し溜める
        yield return new WaitForSeconds(displayTempo*2.5f);
        SoundManager.I.PlaySE("resultDisp2");
        
        // 画面手前からドンって感じに演出
        var resultImTrans = resultCanv.GetComponent<RectTransform>();
        var defScale = resultImTrans.localScale;
        resultImTrans.localScale = defScale * 5;
        resultCanv.alpha = 1;
        // 元のサイズに縮小、表示しきったら結果によるSE再生
        resultImTrans.DOScale(defScale, 0.2f).SetEase(Ease.OutExpo).OnComplete(() => {
            if(isSuccess){
                SoundManager.I.PlaySE("success");
            }else{
                SoundManager.I.PlaySE("fail");
            }
        });
        // 少し揺らす
        resultImTrans.DOShakeAnchorPos(0.5f, 50, 10, 3);

        yield return new WaitForSeconds(1.2f);
        isAnimating = false;
        // ボタンを表示
        ShowButton(thisTimeResult);
        

    }

    // リザルト表示の事前準備
    private void PrepareResult(TurnManager turnMan, int resultMoney, int goalMoney){

        // 上部のテキスト編集
        periodText.text = turnMan.nowYear + "年目　　"+ TurnManager.GetSeasonString(turnMan.nowSeason) + "\n売り上げレポート";

        // 中間部のテキストは一旦空に(動的表示するから)
        valueText.text = "";
        // イラストは一旦透明に
        resultCanv.alpha = 0;
        // buttonも押せるようにしておく
        SetButtonFunctions();
    }

    // ボタンの関数セッティング
    private void SetButtonFunctions(){
        for(int i=0;i<buttons.Length;i++){
            UnityAction action;
            switch(buttonTypes[i]){
                case ResultEnum.Success:
                    action = seasonEndProcess.Success_CallSeasonChangeWindow;
                    break;
                case ResultEnum.Fail:
                    action = seasonEndProcess.Fail_CallGameOverWindow;
                    break;
                case ResultEnum.FailAD:
                    action = seasonEndProcess.AD_CallAD;
                    break;
                default :
                action = () => Debug.LogAssertion("ButtonSettingError");
                break;
            }
            buttons[i].onClick.AddListener(action);
            // 自分を破壊する処理も行う
            buttons[i].onClick.AddListener(FadeOutWindow);
        }
    }

    private void ShowButton(ResultEnum _result){
        // 結果に応じたボタンパターンを表示する
        buttonPatterns[(int)_result].SetActive(true);
    }

    // フェードアウトし削除する
    private void FadeOutWindow(){
        // ボタンを押せないように
        foreach(var button in buttons){
            button.interactable = false;
        }
        GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() => Destroy(this.gameObject));
    }

}
