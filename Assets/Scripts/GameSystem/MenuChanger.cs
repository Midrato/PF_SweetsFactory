using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum MenuEnum{
    Shop,
    Factory,
    Data,
    None
}

public class MenuChanger : MonoBehaviour
{
    // 各メニューの画面
    [SerializeField] private GameObject[] menuWindows = new GameObject[3];
    private RectTransform[] menuWindowsTrans = new RectTransform[3];

    // 起動時のメニュー
    [SerializeField]private MenuEnum startUpMenu = MenuEnum.Factory; 
    // 現在のメニュー
    public MenuEnum nowMenu {get;private set;}

    // メニュー変更用のボタン
    [SerializeField] private Button[] menuChangeButton = new Button[3];
    private Image[] buttonImage = new Image[3];

    [SerializeField] private Color notSelectCol;
    [SerializeField] private Color SelectCol;

    // メニュー遷移可能か
    public bool canChangeMenu = true;
    // 現在メニュー遷移しているか
    private bool isChangingMenu = false;

    [Space]
    // メニューチェンジに要する時間
    [SerializeField] private float menuChangeDuration = 1f;

    void Awake(){
        // Awakeで初っ端有効化してロードさせる
        foreach(var window in menuWindows){
            window.SetActive(true);
        }
    }

    void Start()
    {
        for(int i=0;i<menuWindows.Length;i++){
            menuWindowsTrans[i] = menuWindows[i].GetComponent<RectTransform>();
            buttonImage[i] = menuChangeButton[i].GetComponent<Image>();
        }

        StartUpMenu();
    }

    void Update(){
        // ボタン押下可能かどうかを常に切り替える
        foreach(var button in menuChangeButton){
            button.interactable = canChangeMenu;
        }
    }

    // スタートアップ時の設定
    private void StartUpMenu(){
        nowMenu = startUpMenu;
        for(int i=0;i<menuWindows.Length;i++){
            // 起動時のメニューのみ有効に
            if(i == (int)startUpMenu){
                menuWindows[i].SetActive(true);
                buttonImage[i].color = SelectCol;
            }else{
                menuWindows[i].SetActive(false);
                buttonImage[i].color = notSelectCol;
            }

            // ボタンにメニュー変更の処理を仕込む
            var targEnum = (MenuEnum)Enum.ToObject(typeof(MenuEnum), i);
            menuChangeButton[i].onClick.AddListener(() => StartCoroutine(ChangeMenu(targEnum)));
        }
        
    }

    private IEnumerator ChangeMenu(MenuEnum targMenu){
        Debug.Log("ここに変えたい!:" + targMenu);
        // メニューチェンジ可かつメニューチェンジ中でないかつ現在の項目でない
        if(canChangeMenu && !isChangingMenu && targMenu != nowMenu){
            // チェンジ開始
            isChangingMenu = true;

            // SEを鳴らす
            SoundManager.I.PlaySE("changeWindow");

            // メニュー位置は右か
            bool isRightMenu = targMenu > nowMenu;

            float screenWidth = ScreenSizeGetter.I.GetTrueScreenSize().x;

            menuWindows[(int)targMenu].SetActive(true);
            // ポジションを右または左に
            menuWindowsTrans[(int)targMenu].anchoredPosition = screenWidth * (isRightMenu ? Vector2.right : Vector2.left);

            var menuChangeSeq = DOTween.Sequence();

            if(isRightMenu){
                menuChangeSeq.Append(menuWindowsTrans[(int)nowMenu].DOAnchorPosX(-screenWidth, menuChangeDuration));
                menuChangeSeq.Join(menuWindowsTrans[(int)targMenu].DOAnchorPosX(-screenWidth, menuChangeDuration));

                menuChangeSeq.SetEase(Ease.OutQuad).SetRelative(true);
            }else{
                menuChangeSeq.Append(menuWindowsTrans[(int)nowMenu].DOAnchorPosX(screenWidth, menuChangeDuration));
                menuChangeSeq.Join(menuWindowsTrans[(int)targMenu].DOAnchorPosX(screenWidth, menuChangeDuration));

                menuChangeSeq.SetEase(Ease.OutQuad).SetRelative(true);
            }

            // チェンジ後のボタンの色変え
            for(int i=0;i<buttonImage.Length;i++){
                buttonImage[i].color = (i == (int)targMenu) ? SelectCol : notSelectCol;
            }

            // メニュー変更中はメニュー空白
            nowMenu = MenuEnum.None;

            // メニューチェンジ完了を待つ
            yield return menuChangeSeq.WaitForCompletion();
            // 現在のメニューを更新し、メニューチェンジ終了
            nowMenu = targMenu;
            for(int i=0;i<menuWindows.Length;i++){
                // 起動時のメニューのみ有効に
                menuWindows[i].SetActive(i == (int)nowMenu);
                
            }
            isChangingMenu = false;
        }
    }

    // 現在のメニューが該当メニューであるかを返す
    public bool isTargetMenu(MenuEnum targMenu){
        return !isChangingMenu && nowMenu == targMenu; 
    }
}
