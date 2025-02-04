using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeGetter : MonoBehaviour
{
    static public ScreenSizeGetter I {get;private set;}

    private RectTransform mainCanvasTransform;

    void Awake(){
        if(I == null){
            I = this;
            mainCanvasTransform = GetComponent<RectTransform>();
        }else{
            Destroy(this);
        }
    }

    // 正しい画面のピクセルサイズを得る
    public Vector2 GetTrueScreenSize(){
        return mainCanvasTransform.sizeDelta;
    }
}
