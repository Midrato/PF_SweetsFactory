using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWaiter : MonoBehaviour
{
    private Button button;

    // ボタンのタッチ監視用
    private bool isTouch = false;

    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(TouchButton);
    }

    // ボタンがクリックするまで終了しないコルーチン
    public IEnumerator WaitToTouchButton(){
        isTouch = false;
        while(!isTouch){
            yield return null;
        }
    }

    private void TouchButton(){
        isTouch = true;
    }

}
