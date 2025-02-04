using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidthSolver
{
    // 左右の画面外に行きそうなUIを移動させる
    public void SolveWidth(RectTransform targetTrans){
        // アンカーポジション(画面中心が原点)
        Vector2 localPos = targetTrans.localPosition;
        // ターゲットの左右幅
        float targetWidth = targetTrans.sizeDelta.x * targetTrans.localScale.x / 2;
        // Screenの横幅
        float screenWidth = ScreenSizeGetter.I.GetTrueScreenSize().x;

        // 画面左にはみ出しているかの判定
        if(localPos.x - targetWidth < -1*screenWidth/2){
            // 飛び出している差分
            float diff = -1*screenWidth/2 - (localPos.x - targetWidth);
            // 飛び出している分だけ右に
            targetTrans.localPosition = localPos + diff * Vector2.right;
            Debug.Log("Solve!");
        // 画面右にはみだしているかの判定
        }else if(localPos.x + targetWidth > screenWidth/2){
            float diff = (localPos.x + targetWidth) - screenWidth/2;
            // 飛び出している分だけ左に
            targetTrans.localPosition = localPos + diff * Vector2.left;
        }
    }
}
