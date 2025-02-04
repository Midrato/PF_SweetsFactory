using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PassiveViewButton : MonoBehaviour
{
    // 自身のボタン
    [HideInInspector] public Button passiveViewButton;
    [SerializeField] private Image passiveIcon;

    void Awake()
    {
        passiveViewButton = GetComponent<Button>();
    }

    // ボタンのセットを行う
    public void SetButton(Sprite icon, UnityAction viewPassive){
        passiveIcon.sprite = icon;
        passiveViewButton.onClick.AddListener(viewPassive);
    }

    // モジュール画像の色をセット
    public void SetButtonImageCol(Color color){
        passiveIcon.color = color;
    }
}
