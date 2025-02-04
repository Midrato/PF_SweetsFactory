using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// 各スイーツタイプの出現確率・各スイーツのスコアなどを管理する
public class SweetsTypeManager : MonoBehaviour
{   
    // 全スイーツタイプのPrefab
    public GameObject[] allSweetsType = new GameObject[5];
    // 各スイーツのベース出現率
    [SerializeField]private int[] sweetsSpawnRatesBase = new int[5];
    // 各スイーツの実行出現率
    [field : SerializeField]public int[] sweetsSpawnRates {get;set;} = new int[5];

    // 出現率をパーセントで表現
    public float[] ratesInPercent = new float[5];
    
    private void Start(){
        //SetSweetsSpawnRates();
    }

    // スイーツの実行出現率をセットする(ついでに百分率表示も更新)
    public void SetSweetsSpawnRates(int[] sweetsSpawnRatesDiff){
        for(int i=0;i<sweetsSpawnRatesBase.Length;i++){
            sweetsSpawnRates[i] = sweetsSpawnRatesBase[i] + sweetsSpawnRatesDiff[i];
            // スポーンレートは0未満にならない
            if(sweetsSpawnRates[i] < 0)sweetsSpawnRates[i] = 0;
        }
        SetRatesInPercent();
    }
    
    // 出現率からランダムにスイーツのオブジェクトを取得
    public GameObject PickRandomSweets(){
        int total = sweetsSpawnRates.Sum();
        // 各確率の閾値
        int[] hresholds = new int[allSweetsType.Length-1];
        for(int i=0;i<hresholds.Length;i++){
            if(i == 0){
                hresholds[i] = sweetsSpawnRates[i];
            }else{
                hresholds[i] = hresholds[i-1] + sweetsSpawnRates[i];
            }
        }
        
        // 1~合計値の乱数
        int rand = UnityEngine.Random.Range(1, total);
        SweetsEnum resultType =SweetsEnum.None;

        for(int i=0;i<allSweetsType.Length;i++){
            if(i==0){
                // 最初の範囲は1~閾値0
                if(rand <= hresholds[i]){
                    resultType = (SweetsEnum)i;
                    break;
                }
            }else if(i==(allSweetsType.Length-1)){
                // 最後の範囲は閾値ラスト~最大値
                if(rand > hresholds[i-1]){
                    resultType = (SweetsEnum)i;
                    break;
                }
            }else{
                // その他は閾値i-1~閾値i
                if(rand > hresholds[i-1] && rand <= hresholds[i]){
                    resultType = (SweetsEnum)i;
                    break;
                }
            }
        }
        // 選ばれたタイプのオブジェクトを返す
        //Debug.Log(resultType);
        return allSweetsType[(int)resultType];
    }

    private void SetRatesInPercent(){
        int total = sweetsSpawnRates.Sum();
        for(int i=0;i<sweetsSpawnRates.Length;i++){
            float percent = (float)sweetsSpawnRates[i] / total;
            // (強引に)小数点以下3桁に丸め、パーセンテージに
            float round = Mathf.Round(percent*1000f)/10f;
            ratesInPercent[i] = round;
        }
    }

}
