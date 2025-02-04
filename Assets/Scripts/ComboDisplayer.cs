using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ComboDisplayer : MonoBehaviour
{
    // コンボ表示のテキスト
    private TextMeshProUGUI ComboText;

    // UI透明度用のキャンバスグループ
    private CanvasGroup canvGp;

    // コンボの量によって変化させるためのスケール値
    [SerializeField] private int[] charScaleRange = new int[2]{100, 200};
    [SerializeField] private float[] scaleRange = new float[2]{1, 2};
    [SerializeField] private int[] comboRange = new int[2]{2, 15};

    void Awake()
    {
        canvGp = GetComponent<CanvasGroup>();
        ComboText = GetComponent<TextMeshProUGUI>();
        // 透明な状態からスタート
        canvGp.alpha = 0;
    }

    public IEnumerator DisplayCombo(int combo){
        // 消したスイーツ数が1より大きいならコンボ
        if(combo > 1){
            // 数字文字の大きさ、テキストの大きさを取得
            int charScale = MapCharScale(combo);
            float scale = MapScale(combo);

            // 例 <size=100%><color=yellow>10</color></size>COMBO!
            string displayText = "<size=" + charScale + "%><color=yellow>" + combo + "</color></size>COMBO";
            // コンボ数が多いなら強調
            if(combo >= 5)displayText += "!";

            // UIの左右幅を画面に収める
            transform.localScale = scale*Vector3.one;
            UIWidthSolver widthSolver = new UIWidthSolver();
            widthSolver.SolveWidth(GetComponent<RectTransform>());

            // テキストのスケールを0に
            transform.localScale = Vector3.zero;
            ComboText.text = displayText;

            // コンボ数表示アニメーション
            canvGp.alpha = 1;

            var scaleSeq = DOTween.Sequence();
            scaleSeq.Append(transform.DOScale(scale * Vector3.one, 1f).SetEase(Ease.OutElastic));
            scaleSeq.Append(canvGp.DOFade(0, 1f).SetEase(Ease.InQuart));

            // 少し揺らす 揺らす強さを数字のデカさで増やしたり
            transform.DOShakePosition(1.5f, 15 * scale, 10, 10, false, true);

            // 処理終了を待つ
            yield return scaleSeq.WaitForCompletion();
        }else yield return null;
        
        Destroy(this.gameObject);
    }

    private float MapScale(int combo){
        if(combo <= comboRange[0]){
            return scaleRange[0];
        }else if(combo >= comboRange[1]){
            return scaleRange[1];
        }else{
            // 値をマッピングする
            float percentage = (float)(combo - comboRange[0]) / (comboRange[1] - comboRange[0]);
            return scaleRange[0] + percentage * (scaleRange[1] - scaleRange[0]);
        }
    }

    private int MapCharScale(int combo){
        if(combo <= comboRange[0]){
            return charScaleRange[0];
        }else if(combo >= comboRange[1]){
            return charScaleRange[1];
        }else{
            // 値をマッピングする
            float percentage = (float)(combo - comboRange[0]) / (comboRange[1] - comboRange[0]);
            return Mathf.RoundToInt(charScaleRange[0] + percentage * (charScaleRange[1] - charScaleRange[0]));
        }
    }

}
