using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectingPassiveViewMaster : ObjectSpacer
{
    [SerializeField] private GameObject passiveButton;
    [SerializeField] private GameObject passiveViewWindow;

    // 現在ウィンドウが操作可能かを取得する用
    [SerializeField] private OptContentsWindow optionWindow;
    private List<Button> buttons = new List<Button>();

    // UIの親キャンバス
    private Transform canvas;

    private PassiveHolder holder;

    void Awake(){
        canvas = GameObject.FindGameObjectWithTag("MainCanvas")?.transform;
        // パッシブホルダーを取得する
        holder = GameObject.Find("PassiveHolder")?.GetComponent<PassiveHolder>();
    }

    protected override List<RectTransform> InstObjects()
    {
        var passiveButtons = new List<RectTransform>();
        // 全パッシブのボタンを生成する
        foreach(var passive in holder.passiveItems){
            var _button = InstantiatePassiveButton(passive);
            var buttonTrans = _button.GetComponent<RectTransform>();
            passiveButtons.Add(buttonTrans);
        }
        return passiveButtons;
    }

    // パッシブボタンの生成を行う
    private GameObject InstantiatePassiveButton(PassiveItemBase setItem){
        // ボタンを生成し、親子登録を行う
        var _button = Instantiate(passiveButton);
        _button.transform.SetParent(contentsTrans, false);

        // ボタンにアイコン、アクションを仕込む
        var viewewButton = _button.GetComponent<PassiveViewButton>();
        viewewButton.SetButton(setItem.itemSprite, () => {
            SoundManager.I.PlaySE("changeWindow");
            InstantiateViewWindow(setItem);
        });

        // ボタンを追加
        buttons.Add(viewewButton.passiveViewButton);

        return _button;
    }

    // ウィンドウにアイテムを表示内容として設定し、生成する
    private void InstantiateViewWindow(PassiveItemBase setItem){
        if(optionWindow.isCanControlOption){
            optionWindow.isCanControlOption = false;
            // 画面を生成し、親子登録を行う
            var window = Instantiate(passiveViewWindow);
            window.transform.SetParent(canvas, false);
            var passiveWindow = window.GetComponent<SelectingHavePassiveViewer>();
            passiveWindow.SetItemData(setItem);
            // 閲覧終了時、画面操作可能になるように仕込む
            passiveWindow.endViewAct = () => optionWindow.isCanControlOption = true;
        }
    }
}
 