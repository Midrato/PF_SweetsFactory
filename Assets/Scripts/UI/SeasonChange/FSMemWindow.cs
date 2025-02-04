using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FSMemWindow : MonoBehaviour
{
    // 画面タッチで画面閉じる操作を受け付けるか
    private bool acceptClose = false;
    // 画面を閉じるか
    private bool isClose = false;

    private GeneralTouchSensor tSens;

    // 表示されるテキストオブジェクトたちのリスト
    [SerializeField] private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    private List<RectTransform> textsTrans = new List<RectTransform>();
    private List<CanvasGroup> textsAlpha = new List<CanvasGroup>();

    void Awake(){
        foreach(var txt in texts){
            textsTrans.Add(txt.GetComponent<RectTransform>());
            textsAlpha.Add(txt.GetComponent<CanvasGroup>());
        }
        tSens = new GeneralTouchSensor();
    }

    void Update(){
        if(acceptClose && tSens.GetTouchUp()){
            isClose = true;
        }
    }

    public IEnumerator ShowResult(){
        PrepareShowResult();
        // クリアデータとしてセーブ
        MainSceneDataManager.MCInst.SaveNowAccumulativeData(false, true);

        var seq1 = DOTween.Sequence();
        seq1.Append(SoundManager.I.DOFadeOutBGM(0.5f));
        yield return seq1.WaitForCompletion();

        var seq2 = DOTween.Sequence();
        // 上部テキスト
        var beforeScaleHigh = textsTrans[0].localScale;
        textsAlpha[0].alpha = 1;
        textsTrans[0].localScale = beforeScaleHigh * 4;
        // 画面上部の文字を急に縮小させ登場
        seq2.Append(textsTrans[0].DOScale(beforeScaleHigh, 0.2f).SetEase(Ease.OutExpo)
                .OnComplete(() => SoundManager.I.PlaySE("GameClear")));
        seq2.Append(textsTrans[0].DOShakeAnchorPos(0.5f, 40, 10, 3));
        
        // 中部説明テキスト
        seq2.Append(textsAlpha[1].DOFade(1, 1.4f).SetDelay(1f));

        yield return seq2.WaitForCompletion();

        var seq3 = DOTween.Sequence();
        // 中部売上総額表示
        var beforeScaleMoney = textsTrans[2].localScale;
        // テキストが拡大しながら入ってくる
        textsTrans[2].localScale = Vector3.zero;
        textsAlpha[2].alpha = 1;
        seq3.Append(textsTrans[2].DOScale(beforeScaleMoney, 1.5f).SetEase(Ease.OutBack).SetDelay(1f));

        // 下部説明テキスト
        seq3.Append(textsAlpha[3].DOFade(1, 1f).SetDelay(0.8f));

        yield return seq3.WaitForCompletion();
        yield return new WaitForSeconds(2f);
        var closeTextTween = textsAlpha[4].DOFade(1, 1f);

        // 処理が終了したら終了可能に
        acceptClose = true;
        while(!isClose){
            yield return null;
        }
        closeTextTween?.Kill();

        var afterSeq = DOTween.Sequence();
        foreach(var alp in textsAlpha){
            afterSeq.Join(alp.DOFade(0, 1f));
        }

        yield return afterSeq.WaitForCompletion();
        // BGMをかけなおす
        SoundManager.I.PlayBGM("Result");
    }

    private void PrepareShowResult(){
        // テキストオブジェクトを透明に
        foreach(var alp in textsAlpha){
            alp.alpha = 0;
        }
        // セーブデータから総売り上げ値を取得
        var saveData = DataManager.Inst.LoadPlayingData();
        SetTotalSaleVal(saveData.totalMoney);
    }

    // 総売り上げの値を設定
    private void SetTotalSaleVal(int totalSale){
        texts[2].text = $"<align=left><size=70%>総売り上げ</size></align>\n<color=yellow>{totalSale:N0}</color>\n<align=right><size=75%>アマ</align>";
    }
}
