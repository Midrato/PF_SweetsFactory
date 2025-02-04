using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class DisplayFader : MonoBehaviour
{   
    private Image image;
    private CanvasGroup canv;

    [SerializeField] private bool startClear = true;

    void Awake(){
        image = GetComponent<Image>();
        canv = GetComponent<CanvasGroup>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        
        // 初期状態の透明度
        canv.alpha = startClear ? 0 : 1;
        
    }
    
    
    public Tween MyDOFade(float goalValue, float time){
        return canv.DOFade(goalValue, time).SetEase(Ease.OutSine);
    }
}
