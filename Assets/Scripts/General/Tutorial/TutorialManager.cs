using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // 操作不能にするためのマネージャー類
    [SerializeField] private PuzzleManager puzzleMan;
    [SerializeField] private MenuChanger changer;
    [SerializeField] private SceneEffectPanel effectPanel;

    // チュートリアル画面のプレハブ
    [SerializeField] private GameObject tutorialPanel;

    [SerializeField] private RectTransform parentCanvas;

    // 背景暗転用フェーダー
    [SerializeField] private GameObject fader;

    // チュートリアルを見終わったか
    private bool isEndTutorial = false;

    // チュートリアル既出かチェック用
    private OptionData data;

    void Start()
    {
        // 起動初回のみチュートリアルを表示
        data = DataManager.Inst.LoadOptionData();
        if(!data.isShowTutorial){
            StartCoroutine(SummonTutorial());
        }
    }


    private IEnumerator SummonTutorial(){
        // シャッターが上がり切るまで待機
        while(!effectPanel.isOpen){
            yield return null;
        }
        // 操作不能に
        puzzleMan.SetCannotPuzzle(true);
        changer.canChangeMenu = false;
        // 背景を暗く
        var _fader = Instantiate(fader, parentCanvas, false);
        var bgFader = _fader.GetComponent<DisplayFader>();
        bgFader.MyDOFade(0.5f, 0.3f);

        var _tutorial = Instantiate(tutorialPanel, parentCanvas, false);
        // チュートリアル終了を検知
        _tutorial.GetComponent<OptContentsWindow>()?.SetOnCompleteClose(() => isEndTutorial = true);
        // チュートリアル読了まで待機
        while(!isEndTutorial){
            yield return null;
        }
        // チュートリアル終了後の処理
        // 操作可能に
        puzzleMan.SetCannotPuzzle(false);
        changer.canChangeMenu = true;
        // チュートリアルを見せたらそれを保存
        data.isShowTutorial = true;
        DataManager.Inst.SaveOptionData(data);
        // 背景を明るく、明るくしきったらデストロイ
        bgFader.MyDOFade(0f, 0.3f).OnComplete(() => Destroy(_fader));
    }
}
