using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // 各スイーツのベース単体価値
    [SerializeField]private int[] sweetsPriceBase = new int[5];
    // 各スイーツのベース倍率
    [SerializeField]private float[] sweetsMagnificationBase = new float[5];

    // 各スイーツの実行単体価値
    [field : SerializeField]public int[] sweetsPrice {get;private set;} = new int[5];
    // 各スイーツの実行倍率
    [field : SerializeField]public float[] sweetsMagnification {get;private set;} = new float[5];

    // UI表示用
    [SerializeField] private GameObject scoreDisplayer;
    [SerializeField] private GameObject comboDisplayer;
    [SerializeField] private GameObject canvas;
    // カメラを揺らすコンポーネント
    [SerializeField] private CameraVibration cameraShaker;

    // 所持金を管理するマネージャー
    [SerializeField] private MoneyManager moneyMan;
    // スコア計算のためのパッシブアイテムホルダー
    [SerializeField] private PassiveHolder passiveHolder;

    // スコア上昇使用アイテムの使用数
    private int itemUseCount = 0;
    [Space]
    // アイテムにつき上昇するスコア倍率
    [SerializeField] private float itemMagnification = 1f;
    // 一度でアイテムを何個まで使えるか
    [field : SerializeField] public int itemUseLimit {get;private set;} = 3;

    [Space]

    // seの大きさの閾値(3段階)
    [SerializeField] private int[] seThreshold = new int[2];


    // パッシブによる補正を元に実行単体価値・実行倍率をセットする
    public void SetPriceAndMagnification(int[] sweetsPriceDiff, float[] sweetsMagnificationDiff){
        for(int i=0;i<sweetsPrice.Length;i++){
            sweetsPrice[i] = sweetsPriceBase[i] + sweetsPriceDiff[i];
            sweetsMagnification[i] = sweetsMagnificationBase[i] + sweetsMagnificationDiff[i];
        }
    }
    
    // スイーツのタイプ・消した数・売上から売り上げを計算
    public int CalcScore(SweetsEnum sweetsType, int count, float magnification){
        // スコアは整数
        // 実効単価×消した数×倍率+固定補正値
        
        // 実効単価
        int effectiveUnitPrice = sweetsPrice[(int)sweetsType] + passiveHolder.GetSpecialSweetsPrice(sweetsType, count);
        Debug.Log("実行単価:" + effectiveUnitPrice + "アマ");
        // 売り上げ
        return Mathf.RoundToInt(effectiveUnitPrice * count * magnification) + passiveHolder.GetBonusPrice(sweetsType, count);
    }

    // スイーツのタイプ・消した数から倍率を計算
    private float CalcMagnification(SweetsEnum sweetsType, int count){
        // 2つ以上消したら倍率が発生するように
        // 1 + 実効スイーツ倍率×(消した数-1) + 固定補正倍率
        // アイテム使用倍率はさらに全体にかかる

        // 実行倍率
        float effectiveUnitMagni = sweetsMagnification[(int)sweetsType] + passiveHolder.GetSpecialSweetsMagnification(sweetsType, count);
        Debug.Log("実行倍率:" + effectiveUnitMagni + "%");
        // 適用倍率
        float appliedMagnification = (1 + effectiveUnitMagni * (count-1) + passiveHolder.GetBonusMagnification(sweetsType, count)) * (1f + itemUseCount * itemMagnification);
        // 倍率が0を下回っていたならば0に補正
        if(appliedMagnification < 0)appliedMagnification = 0;
        return appliedMagnification;
    }

    // 単純にスイーツ単価×消した数のスコアから変化した割合を百分率で表現
    private int CalcMagniPercent(int baseScore, int count, int appliedScore){
        float ratio = (float)appliedScore / (baseScore * count);
        return (int)(ratio * 100);
    }

    public void DisplayScore(SweetsEnum sweetsType, int count, Vector3 displayPoint){
        float magnification = CalcMagnification(sweetsType, count);
        int score = CalcScore(sweetsType, count, magnification);

        // スコアが0を下回るなら0にする
        score = (score < 0) ? 0 : score;

        // 倍率をパーセントで表したもの
        int magniToPercent = CalcMagniPercent(sweetsPrice[(int)sweetsType], count, score);
        Debug.Log(score + "アマ");
        Debug.Log(magniToPercent + "%");
        // 所持金に加算
        moneyMan.AddMoney(score);

        // スコア : UIに表示
        var scoreDisp = Instantiate(scoreDisplayer, displayPoint, quaternion.identity);
        scoreDisp.transform.SetParent(canvas.transform);
        // 描画コルーチンを開始
        StartCoroutine(scoreDisp.GetComponent<ScoreDislpayer>().DisplayScore(score, magniToPercent));

        // コンボ : UIに表示
        // スコアより60だけ下に表示
        var comboDisp = Instantiate(comboDisplayer, displayPoint+60*Vector3.down, quaternion.identity);
        comboDisp.transform.SetParent(canvas.transform);
        // 描画コルーチン開始
        StartCoroutine(comboDisp.GetComponent<ComboDisplayer>().DisplayCombo(count));
        // スコアが大きいならカメラを揺らして演出
        if(score >= 100000)cameraShaker.ShakeCamera(1.2f);

        // スコアの大きさに応じてSEを再生
        PlayScoreSE(score);
        
        // アイテム使用カウントをリセット
        itemUseCount = 0;
        passiveHolder.resetCondition();
    }

    // アイテムを使用する。最大数によって出来なかった場合falseを返す
    public bool SetUseItemCount(int count){
        if(count > itemUseLimit){
            return false;
        }else{
            itemUseCount = count;
            return true;
        }
    }

    private void PlayScoreSE(int score){
        string seName = "";
        if(score < seThreshold[0]){
            seName = "lowCombo";
        }else if(score >= seThreshold[0] && score < seThreshold[1]){
            seName = "midCombo";
        }else{
            seName = "highCombo";
        }

        SoundManager.I.PlaySE(seName);
    }


    // パッシブの効果を含めた基礎価格
    public int GetIncludePassivePrice(SweetsEnum type){
        return sweetsPrice[(int)type] + passiveHolder.GetSpecialSweetsPrice(type, 0, false);
    }

    // パッシブの効果を含めたコンボ倍率
    public float GetIncluddePassiveMagni(SweetsEnum type){
        return sweetsMagnification[(int)type] + passiveHolder.GetSpecialSweetsMagnification(type, 0, false);
    }
}
