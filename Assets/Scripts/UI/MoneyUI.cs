using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyUI : UITextBase
{
    [SerializeField] private MoneyManager moneyMan;

    // 売上時のUI強調のサイズ差
    [SerializeField] private float[] textScaleDiff = new float[2];

    private int currentMoney = 0;

    protected override void Start()
    {
        base.Start();
        currentMoney = moneyMan.nowMoney;
        myText.text = currentMoney.ToString("N0") + "アマ";
    }

    protected override void Update()
    {
        if(currentMoney != moneyMan.nowMoney){
            ChangedContent();
        }
    }

    public override void ChangedContent()
    {
        StartCoroutine(VibrationText(currentMoney));
        currentMoney = moneyMan.nowMoney;
    }

    private IEnumerator VibrationText(int beforeMoney){
        // 一瞬大きくして戻す動きを行う
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(textScaleDiff[1] * Vector3.one, 0.15f).SetEase(Ease.InExpo));
        seq.Append(transform.DOScale(textScaleDiff[0] * Vector3.one, 0.15f).SetEase(Ease.OutExpo));

        // 加算処理中は繰り返す
        seq.SetLoops(-1, LoopType.Restart);

        //int tweenValue = 0;

        // 値を滑らかに増加させるTween
        var textTween =  DOTween.To(() => beforeMoney, (val) => {
            int roundValue = Mathf.RoundToInt(val);
            myText.text = roundValue.ToString("N0") + "アマ";
        }, moneyMan.nowMoney, 1f).OnKill(() => seq.Kill());

        // テキストのTweenが終了したら演出seqをキル
        yield return textTween.WaitForCompletion();
        seq.Kill();
        // スケールを初期値に戻す
        transform.localScale = textScaleDiff[0] * Vector3.one;
    }
}
