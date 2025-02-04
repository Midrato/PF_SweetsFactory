using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickSceneChanger : SceneChangeBase
{
    private GeneralTouchSensor touchSens;

    protected override void Start()
    {
        base.Start();
        touchSens = new GeneralTouchSensor();
    }

    void Update()
    {
        // スタート可能でタッチした瞬間に実行
        if(canSceneChange && touchSens.GetTouchDown()){
            ChangeScene();
        }
    }

}
