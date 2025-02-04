using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CannotUseText : MonoBehaviour
{
    // 使用不可テキスト表示時間
    private float cannotUseDuration = 1f;

    private TextMeshProUGUI myText;

    // 動作を保存用のコルーチン変数
    private Coroutine nowCoroutine;

    void Start(){
        myText = GetComponent<TextMeshProUGUI>();
        myText.text = "";
    }

    // 使用不可を示す演出
    private IEnumerator CannotUseItem(string errorReason){
        // エラー理由のテキスト
        myText.text = errorReason;
        // キャンセルSEを鳴らす
        SoundManager.I.PlaySE("cancel");
        yield return new WaitForSeconds(cannotUseDuration);
        myText.text = "";
    }

    public void DispCannotUseItem(string errorReason){
        // 現在動作中のコルーチンを停止
        if(nowCoroutine != null){
            StopCoroutine(nowCoroutine);
        }
        
        // 使用不可表示をする
        nowCoroutine = StartCoroutine(CannotUseItem(errorReason));
    }

    void OnDisable(){
        myText.text = "";
    }
}
