using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndItemUseAnim : MonoBehaviour
{
    // アニメーションで呼び出される自壊メソッド
    public void EndAnim(){
        Destroy(this.gameObject);
    }
}
