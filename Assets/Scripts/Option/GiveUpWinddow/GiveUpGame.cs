using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveUpGame : MonoBehaviour
{
    [SerializeField] private Button giveUpButton;
    [SerializeField] private OptContentsWindow contentsWindow;

    // ゲームオーバー遷移するオブジェクト
    private GoGameOver goGameOver;

    void Start(){
        goGameOver = GameObject.Find("SceneChanger")?.GetComponent<GoGameOver>();
        giveUpButton.onClick.AddListener(() => {
            // 画面操作不能にし、ゲームオーバーに
            contentsWindow.isCanControlOption = false;
            StartCoroutine(goGameOver.ChangeToFailScene());
        });
    }
}
