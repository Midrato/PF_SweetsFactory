using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PassiveViewerBase : PassiveExplainBase
{
    // パッシブ表示画面のトランスフォーム
    [SerializeField] private RectTransform windowRect;
    // ウィンドウのデフォルトのスケール
    [SerializeField] private Vector3 defaultScale = new Vector3(1,1,1);
    // 画面暗転用
    [SerializeField] private DisplayFader fader;
    // 画面を閉じるためのボタン
    [SerializeField] private Button[] closeButtons = new Button[2];

    [SerializeField] private float dispDuration = 0.2f;

    // 画面を展開しているか
    private bool isOpened = false;
    // 画面を閉じているか
    private bool isClosing = false;

    void Awake()
    {
        // 一旦スケール0に
        windowRect.localScale = Vector3.zero;

        // ボタンに閲覧終了処理を加える
        foreach(var button in closeButtons){
            button.onClick.AddListener(EndView);
        }
    }

    public override void SetItemData(PassiveItemBase item)
    {
        base.SetItemData(item);
        StartView();
    }

    private void StartView(){
        var openSeq = DOTween.Sequence();
        // 画面を大きくして、画面暗転
        openSeq.Append(windowRect.DOScale(defaultScale, dispDuration));
        openSeq.Join(fader.MyDOFade(0.3f, dispDuration));

        //開ききったら画面展開フラグオン
        openSeq.OnKill(() => isOpened = true);
    }

    private void EndView(){
        Debug.Log("終わったね");
        // 開いている途中・閉じてる途中なら閉じれない
        if(!isOpened || isClosing){
            return;
        }else{
            isClosing = true;
            var closeSeq = DOTween.Sequence();

            // 画面を小さくして、画面暗転解除
            closeSeq.Append(windowRect.DOScale(Vector3.zero, dispDuration));
            closeSeq.Join(fader.MyDOFade(0, dispDuration));

            closeSeq.OnKill(() => {
                OnEndView();
                Destroy(this.gameObject);
            });
        }
    }

    protected virtual void OnEndView(){}
}
