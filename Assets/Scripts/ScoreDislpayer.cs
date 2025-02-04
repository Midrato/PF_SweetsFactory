using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreDislpayer : MonoBehaviour
{
    // スコア・倍率のテキスト
    private TextMeshProUGUI scoreText;
    [SerializeField]private TextMeshProUGUI percentageText;
    
    // UI透明度用のキャンバスグループ
    private CanvasGroup canvGp;

    // スコアの量によって変化させるためのスケール値
    [SerializeField] private float[] scaleRange = new float[2]{1.5f, 3f};
    [SerializeField] private int[] scoreRange = new int[2]{1000, 100000};

    void Awake(){
        scoreText = GetComponent<TextMeshProUGUI>();
        canvGp = GetComponent<CanvasGroup>();
        // 透明な状態からスタート
        canvGp.alpha = 0;
    }

    public IEnumerator DisplayScore(int score, int percentage){
        // テキストに設定
        scoreText.text = score.ToString("N0");
        // 倍率は一定以上でないと描画しない
        if(percentage >= 150){
            percentageText.text = "x" + percentage.ToString() + "%";
        }else{
            percentageText.text = "";
        }

        var myTrans = GetComponent<RectTransform>();
        

        // スケールをスコアに準じたものに設定
        float scoreScale = CalcScale(score);
        myTrans.localScale = scoreScale * Vector3.one;

        // UIの左右幅を画面に収める
        UIWidthSolver widthSolver = new UIWidthSolver();
        widthSolver.SolveWidth(GetComponent<RectTransform>());

        // フェード処理
        var fadeSeq = DOTween.Sequence();
        fadeSeq.Append(canvGp.DOFade(1, 0.3f));
        fadeSeq.Append(canvGp.DOFade(0, 1.2f).SetDelay(0.3f));

        // わずかな移動処理
        var moveSeq = DOTween.Sequence();
        moveSeq.Append(myTrans.DOLocalMoveY(100, 0.3f).SetRelative(true));
        moveSeq.Append(myTrans.DOLocalMoveY(150, 1.6f).SetRelative(true).SetEase(Ease.InOutSine));
        
        // 処理終了を待つ
        yield return moveSeq.WaitForCompletion();
        Destroy(this.gameObject);
    }

    private float CalcScale(int score){
        if(score <= scoreRange[0]){
            return scaleRange[0];
        }else if(score >= scoreRange[1]){
            return scaleRange[1];
        }else{
            // 値をマッピングする
            float percentage = (score - scoreRange[0]) / (scoreRange[1] - scoreRange[0]);
            return scaleRange[0] + percentage * (scaleRange[1] - scaleRange[0]);
        }
    }



}
