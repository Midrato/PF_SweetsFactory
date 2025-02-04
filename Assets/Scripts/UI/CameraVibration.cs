using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CameraVibration : MonoBehaviour
{
    private Tween myTween;

    [SerializeField] private float shakeTime;
    
    public void ShakeCamera(float shakePower){
        DOTween.Kill(myTween);
        myTween = transform.DOShakePosition(shakeTime, shakePower);
    }
}
