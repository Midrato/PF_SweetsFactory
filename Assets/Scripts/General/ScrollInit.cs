using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar))]
public class ScrollInit : MonoBehaviour
{
    private Scrollbar scroll;

    void Awake()
    {
        scroll = GetComponent<Scrollbar>();
    }

    void OnEnable(){
        // スクロールの位置を初期化
        scroll.value = 1;
    }
}
