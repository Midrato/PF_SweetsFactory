using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShopBuyText : MonoBehaviour
{
    private TextMeshProUGUI myText;

    private RectTransform myTrans;

    // 表示する時間
    [SerializeField] private float dispDuration;

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
        myTrans = GetComponent<RectTransform>();
    }

    public void ShowText(string showText = "購入しました!"){
        myText.text = showText;

        // 上にスーッとしてから消えていく
        var seq = DOTween.Sequence();
        seq.Append(myTrans.DOAnchorPosY(100, dispDuration));
        seq.Join(myText.DOFade(0, dispDuration).SetEase(Ease.InExpo));

        // 消えたら自身を破壊
        seq.OnKill(() => Destroy(this.gameObject));
    }
}
