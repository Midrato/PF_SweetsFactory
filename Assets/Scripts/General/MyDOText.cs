using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MyDOText : MonoBehaviour
{
    private TextMeshProUGUI myText; 
    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 現在のテキストを滑らかに消去する
    /// </summary>
    /// <param name="duration">動作時間</param>
    /// <returns></returns>
    public Tween DORemove(float duration){
        Tween tween = DOTween.To(() => myText.text.Length-1, (val) => {
            var beforetxt = myText.text;
            int tweenLength = Mathf.RoundToInt(val);
            // 文字を消していく
            myText.text = tweenLength > 0 ? beforetxt.Substring(0, tweenLength) : "";
        }, 0, duration);

        return tween;
    }

    /// <summary>
    /// 指定のテキストに滑らかに変化させる
    /// </summary>
    /// <param name="dispText">表示テキスト</param>
    /// <param name="duration">動作時間</param>
    /// <returns></returns>
    public Tween DOAddText(string dispText, float duration){
        Tween tween = DOTween.To(() => 0, (val) => {
            int tweenLength = Mathf.RoundToInt(val);
            // 文字を追加していく
            myText.text = tweenLength != 0 ? dispText.Substring(0, tweenLength) : "";
        }, dispText.Length, duration);

        return tween;
    }
}
