using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class SceneEffectPanel : MonoBehaviour
{
    // パズル可能設定用パズルマネージャー
    [SerializeField] private PuzzleManager puzzleMan;
    // メニュー遷移可否設定用
    [SerializeField] private MenuChanger menuChanger;

    // シャッター画面のトランスフォーム
    private RectTransform myTrans;
    // 透明度変更用キャンバスグループ
    private CanvasGroup canvGp;

    // シャッターが開いているか
    public bool isOpen {get;private set;} = false;

    void Start()
    {
        myTrans = GetComponent<RectTransform>();
        canvGp = GetComponent<CanvasGroup>();
        // パズルできないように設定
        puzzleMan.SetCannotPuzzle(true);
        // メニューチェンジ不可に
        menuChanger.canChangeMenu = false;
        // パズルを不透明に
        canvGp.alpha = 1;

        // シャッターを上げてゲーム開始
        StartCoroutine(ShutterOpening());
        Debug.Log("ScreenHeight : " + ScreenSizeGetter.I.GetTrueScreenSize().y);
    }

    private IEnumerator ShutterOpening(){
        // シャッターを上げて終了を待つ
        yield return myTrans.DOAnchorPosY(ScreenSizeGetter.I.GetTrueScreenSize().y, 1.5f).SetEase(Ease.InOutCirc).WaitForCompletion();
        // パズル可能に
        puzzleMan.SetCannotPuzzle(false);
        this.gameObject.SetActive(false);
        // メニューチェンジ可能に
        menuChanger.canChangeMenu = true;
        isOpen = true;
    }

    public IEnumerator ShutterClosing(){
        isOpen = false;
        // シャッターを下げて終了を待つ
        yield return myTrans.DOAnchorPosY(0, 1f).SetEase(Ease.OutQuint).WaitForCompletion();
    }
}
