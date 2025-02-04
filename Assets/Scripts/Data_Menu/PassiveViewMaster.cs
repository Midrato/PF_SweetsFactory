using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveViewMaster : ObjectSpacer
{
    // パッシブのボタン
    [SerializeField] private GameObject passiveButton;
    // パッシブ内容の表示用オブジェクト
    [SerializeField] private GameObject passiveViewWindow;
    // UIの親キャンバス
    [SerializeField] private RectTransform canvas;

    // パッシブホルダー
    [SerializeField] private PassiveHolder holder;

    // 現在のメニューを取得
    [SerializeField] private MenuChanger menuChanger;

    // パッシブアイテムに参照づけたパッシブボタンのトランスフォーム辞書型
    private Dictionary<PassiveItemBase, RectTransform> passiveButtonDic = new Dictionary<PassiveItemBase, RectTransform>();
    
    // 所持パッシブの中で、まだパッシブボタンを生成していない物を生成する、また、パッシブボタンの中に所持していないパッシブがあるなら除外する
    protected override List<RectTransform> InstObjects(){
        var nowPassives = holder.passiveItems;

        // 除外するキー
        var removeKeys = new List<PassiveItemBase>();
        // 先に除外
        foreach(var madePassive in passiveButtonDic.Keys){
            if(!nowPassives.Contains(madePassive)){
                removeKeys.Add(madePassive);
            }
        }
        foreach(var key in removeKeys){
            // キー削除するオブジェクトを破壊
            Destroy(passiveButtonDic[key].gameObject);
            passiveButtonDic.Remove(key);
        }

        // 生成
        foreach(var havePassive in nowPassives){
            // まだボタンを生成していないパッシブなら
            if(!passiveButtonDic.ContainsKey(havePassive)){
                var _button = InstantiatePassiveButton(havePassive);
                var buttonTrans = _button.GetComponent<RectTransform>();
                // 辞書に登録する
                passiveButtonDic.Add(havePassive, buttonTrans);
            }
        }
        return new List<RectTransform>(passiveButtonDic.Values);
    }
    
    // パッシブボタンの生成を行う
    private GameObject InstantiatePassiveButton(PassiveItemBase setItem){
        // ボタンを生成し、親子登録を行う
        var _button = Instantiate(passiveButton);
        _button.transform.SetParent(contentsTrans, false);

        // ボタンにアイコン、アクションを仕込む
        _button.GetComponent<PassiveViewButton>().SetButton(setItem.itemSprite, () => {
            SoundManager.I.PlaySE("changeWindow");
            InstantiateViewWindow(setItem);
        });

        return _button;
    }

    // ウィンドウにアイテムを表示内容として設定し、生成する
    private void InstantiateViewWindow(PassiveItemBase setItem){
        if(menuChanger.nowMenu == MenuEnum.Data && menuChanger.canChangeMenu){
            // メニュー遷移不能に
            menuChanger.canChangeMenu = false;
            // 画面を生成し、親子登録を行う
            var window = Instantiate(passiveViewWindow);
            window.transform.SetParent(canvas, false);

            var viewer = window.GetComponent<PassiveViewer>();
            viewer.setMenuChanger(menuChanger);
            viewer.SetItemData(setItem);
        }
        
    }
}
