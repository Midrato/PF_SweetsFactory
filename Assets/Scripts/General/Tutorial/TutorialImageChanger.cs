using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialImageChanger : MonoBehaviour
{
    // ページ移動用ボタン
    [SerializeField] private Button rightButton;
    [SerializeField] private Button LeftButton;
    // 現ページ表示テキスト
    [SerializeField] private TextMeshProUGUI pageText;
    // ページの画像
    [SerializeField] private List<Sprite> pageImages = new List<Sprite>();
    
    // ページを表示するImage
    [SerializeField] private Image image;

    // 現在のページ
    private int nowPage = 0;

    // 画面操作可能かのチェック用
    [SerializeField] private OptContentsWindow optionWindow;

    void Start(){
        UpdatePage();
        rightButton.onClick.AddListener(GoRight);
        LeftButton.onClick.AddListener(GoLeft);
    }

    void Update(){
        // ボタン操作可能かをセット
        SetButtonInteractable(optionWindow.isCanControlOption);
    }

    private void SetButtonInteractable(bool isInteractable){
        rightButton.interactable = isInteractable;
        LeftButton.interactable = isInteractable;
    }

    // 次のページへ
    private void GoRight(){
        // 一番右にいるなら何もしない
        if(nowPage == pageImages.Count-1)return;
        // ページを移動し更新
        nowPage++;
        UpdatePage();
    }

    // 前のページへ
    private void GoLeft(){
        // 一番左にいるなら何もしない
        if(nowPage == 0)return;
        // ページを移動し更新
        nowPage--;
        UpdatePage();
    }

    private void UpdatePage(){
        // それぞれページ端ならボタンを表示しない
        rightButton.gameObject.SetActive(nowPage != pageImages.Count-1);
        LeftButton.gameObject.SetActive(nowPage != 0);
        // ページ数表示を更新
        pageText.text = $"{nowPage+1} / {pageImages.Count}";
        image.sprite = pageImages[nowPage];
    }
}
