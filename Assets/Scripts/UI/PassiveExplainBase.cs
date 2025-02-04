using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


// パッシブアイテムの説明ウィンドウ用の汎用処理
public class PassiveExplainBase : MonoBehaviour
{
    // 必要なUI要素
    [SerializeField] protected Image itemIcon;    // アイコン
    [SerializeField] protected TextMeshProUGUI itemName;  // アイテム名前
    [SerializeField] protected TextMeshProUGUI itemRank;  // アイテムのランク
    [SerializeField] protected TextMeshProUGUI itemType;  // アイテムタイプ
    [SerializeField] protected TextMeshProUGUI itemExplain;   //アイテムの説明文
    [SerializeField] protected Scrollbar explainScrollBar;  // 説明文描画のプログレスバー
    
    // パッシブアイテムに応じてそれをセットする
    public virtual void SetItemData(PassiveItemBase item){
        itemIcon.sprite = item.itemSprite;  // アイコン
        itemName.text = item.itemName;      // アイテム名
        // アイテム名は文字色をタイプのイメージカラーに
        itemName.color = SweetsEnumConv.SweetsToColor(item.itemType);
        // ランク数をIで表す
        var rankText = "";
        for(int i=0;i<item.itemRank;i++){
            rankText += 'I';
        }
        itemRank.text = rankText;           // アイテムランク

        itemType.text = "<color=#D5F8FF>TYPE：</color>" + SweetsEnumConv.SweetsToEnglish(item.itemType);  // アイテムタイプ
        // やはりタイプに関する記述の色はイメージカラーに
        itemType.color = SweetsEnumConv.SweetsToColor(item.itemType);
        itemExplain.text = item.itemExplanation;    // 説明文

        // スクロールバーを上に
        //explainScrollBar.value = 1;
        StartCoroutine(SetScroll());
    }
    // スクロールバーをずらす処理(2F目に行う)
    private IEnumerator SetScroll(){
        yield return null;
        explainScrollBar.value = 1;
        yield return null;
        explainScrollBar.value = 1;
    }
}
