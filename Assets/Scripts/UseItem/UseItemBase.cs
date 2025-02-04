using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UseItemBase : MonoBehaviour
{   
    // アイテム番号
    [SerializeField] protected int itemNumber;
    // アイテム数表示用のテキスト
    [SerializeField] protected TextMeshProUGUI itemNumText;

    // アイテム発動ボタン
    protected Button itemUseButton;

    // アイテム使用可否判定用のコンポーネント
    [HideInInspector]public ItemManager itemManager;

    // 演出に使う用のアイテムのアイコン
    [field : SerializeField]public Image itemIcon {get;protected set;}

    // アイテム使用不可時の表示テキスト
    [SerializeField] protected CannotUseText cannotUseText;
    //private TextMeshProUGUI _cannotUseText;

    protected virtual void Start(){
        itemUseButton = GetComponent<Button>();

        // タッチイベントにアイテム使用を追加
        itemUseButton.onClick.AddListener(UseItem);
    }

    void Update(){
        // アイテム所持数テキストを設定
        itemNumText.text = "x" + itemManager.HaveItemNumber[itemNumber];
    } 

    // アイテム所持数0/パズル等処理中なら使用できない
    public virtual void UseItem(){
        if(itemManager.menuChanger.nowMenu != MenuEnum.Factory || !itemManager.menuChanger.canChangeMenu){
            return;
        }else if(itemManager.HaveItemNumber[itemNumber] <= 0){
            cannotUseText?.DispCannotUseItem("アイテムを所持していません!");
            return;
        }else{
            _UseItem();
            Debug.Log("UseItem!");
        }
    }

    // 編集用アイテム使用時効果
    protected virtual void _UseItem(){

    }

    // アイテム使用後に呼び出す
    protected virtual IEnumerator UsedItem(){
        itemManager.HaveItemNumber[itemNumber]--;
        itemManager.itemUsage[itemNumber]++;
        // 使用時セーブ
        MainSceneDataManager.MCInst.SaveNowPlayingData();
        yield return StartCoroutine(itemManager.ItemUseEffect(itemIcon.sprite));
    }

    public virtual void OnTurnEnd(){}
}
