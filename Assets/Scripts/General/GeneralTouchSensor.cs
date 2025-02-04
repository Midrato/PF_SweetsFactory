using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTouchSensor
{
    private RuntimePlatform platform;

    public bool GetTouchDown(){
        switch(platform){
            case RuntimePlatform.Android:
                // スマホ操作のタッチ時判定を取得
                if(Input.touchCount > 0){
                    var touch = Input.GetTouch(0);
                    return touch.phase == TouchPhase.Began;
                }else return false;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                // パソコン操作、同様に
                return Input.GetMouseButtonDown(0);
        }
        return false;
    }

    public bool GetTouch(){
        switch(platform){
            case RuntimePlatform.Android:
                // スマホ操作のタッチ中判定を取得
                if(Input.touchCount > 0){
                    return true;
                }else return false;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                // パソコン操作、同様に
                return Input.GetMouseButton(0);
        }
        return false;
    }

    public bool GetTouchUp(){
        switch(platform){
            case RuntimePlatform.Android:
                // スマホ操作のタッチ解除判定を取得
                if(Input.touchCount > 0){
                    var touch = Input.GetTouch(0);
                    return touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
                }else return false;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                // パソコン操作、同様に
                return Input.GetMouseButtonUp(0);
        }
        return false;
    }

    public Vector3 TouchPosition(){
        switch(platform){
            case RuntimePlatform.Android:
                // スマホ操作のタッチ位置を取得
                if(Input.touchCount > 0){
                    var touch = Input.GetTouch(0);
                    return touch.position;
                }else return Vector3.zero;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                // パソコン操作、同様に
                return Input.mousePosition;
        }
        return Vector3.zero;
    }

    public GeneralTouchSensor(){
        platform = Application.platform;
    }
}
