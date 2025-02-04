using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressbarUI : MonoBehaviour
{   
    // 進捗表示用プログレスバー
    private Scrollbar progressbar;

    [SerializeField] private MoneyManager moneyMan;
    [SerializeField] private PeriodGoalManager goalMan;

    private int currentValue;

    void Start(){
        progressbar = GetComponent<Scrollbar>();
        currentValue = CalcProgressByPercentage();
        progressbar.size = currentValue/100f;
    }

    void Update(){
        var progressPercentage = CalcProgressByPercentage();
        // 現在の値が変化したら更新する
        if(currentValue != progressPercentage){
            // 値変更前の進捗度
            var beforePercent = progressbar.size;
            // 値変更後の進捗度
            var goalPercent = CalcProgressValue(progressPercentage);
            // 値が違うならTweenアニメーション
            if(beforePercent != goalPercent){
                var progressTween = DOTween.To(() => progressbar.size, (val) => {
                    progressbar.size = val;
                }, goalPercent, 1f);

                progressTween.SetEase(Ease.InOutQuint);
            }
            currentValue = progressPercentage;
        }
    }

    private float CalcProgressValue(int percentage){
        // 進捗100%以上なら完全に進行
        if(percentage >= 100){
            return 1f;
        }else{
            // 100%未満なら割合に直す
            return percentage / 100f;
        }
    }

    // 所持金が目標金額の何%かを計算する
    private int CalcProgressByPercentage(){
        int nowMoney = moneyMan.nowMoney;
        int goalMoney = goalMan.NowGoalMoney();

        float ratio = (float)nowMoney / goalMoney;
        return (int)(ratio*100);
    }
}
