using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DebugFPSCounter : MonoBehaviour
{
    // FPS計算用
    private int frameCount = 0;
    private float prevTime = 0f;
    private float fps = -1;

    // FPS画面表示用テキストボックス
    private TextMeshProUGUI fpsText;

    private bool showFPS = true;

    void Start(){
        fpsText = GetComponent<TextMeshProUGUI>();
        #if UNITY_EDITOR
        showFPS = true;
        #else
        Destroy(this.gameObject);
        #endif
    }

    void Update()
    {
        if(showFPS){
            
            frameCount++;
            float time = Time.realtimeSinceStartup - prevTime;
    
            if (time >= 0.5f) {
                fps = frameCount / time;
                // テキストに代入
                fpsText.text = "FPS : " + fps;
    
                frameCount = 0;
                prevTime = Time.realtimeSinceStartup;
            }
        }
        
    }
}
