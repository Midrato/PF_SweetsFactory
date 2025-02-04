using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsSpecMaster : MonoBehaviour
{
    // スイーツのスペックセット用
    [SerializeField] private SweetsSpec[] sweetsSpecs = new SweetsSpec[5];

    // 情報取得用のコンポーネントたち
    [SerializeField] private SweetsTypeManager sweetsTypeMan;
    [SerializeField] private ScoreManager scoreMan;
    
    void OnEnable(){
        SetSpecs();
    }

    private void SetSpecs(){
        for(int i=0;i<sweetsSpecs.Length;i++){
            // 必要な情報を取得
            string price = scoreMan?.GetIncludePassivePrice((SweetsEnum)i).ToString("N0");
            // コンボ倍率を百分率に直したものを整数に丸め、さらに文字列にする
            string magnification = Mathf.Round(scoreMan.GetIncluddePassiveMagni((SweetsEnum)i) * 100).ToString() + "%";
            string spawnRate = sweetsTypeMan?.sweetsSpawnRates[i].ToString() + "コ";
            string spawnRatePercent = "(出現割合:" + sweetsTypeMan?.ratesInPercent[i].ToString() + "%)";
            
            sweetsSpecs[i]?.SetSpecTexts(price, magnification, spawnRate, spawnRatePercent);
        }
    }
}
