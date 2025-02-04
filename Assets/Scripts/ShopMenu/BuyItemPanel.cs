using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemPanel : MonoBehaviour
{
    // 目標金額参照用
    [SerializeField] private PeriodGoalManager goalMoneyMan;
    // メニュー参照用
    [SerializeField] private MenuChanger menuChanger;
    // 現在金額編集用
    [SerializeField] private MoneyManager moneyMan;
    // アイテム所持数編集用
    [SerializeField] private ItemManager itemMan;

    [Space]

    // 購入ボタン
    [SerializeField] private Button buyButton;
    // アイテムの値段表示用テキスト
    [SerializeField] private TextMeshProUGUI itemPriceText;

    // 購入通知テキスト
    [SerializeField] private GameObject boughtText;

    [Space]

    // 売るアイテムの番号
    [SerializeField] private int sellItemNumber;
    // 値段(今期稼がなければならない金額の最大値に対する割合)
    [SerializeField] private float itemPriceRatio = 0.05f;

    void Start()
    {
        buyButton.onClick.AddListener(BuyItem);
    }

    void Update()
    {
        // 現在の値段をセット
        itemPriceText.text = GetItemPrice().ToString("N0");
    }

    private int GetItemPrice(){
        // そのままの値段
        var calcPrice = goalMoneyMan.GetEarnByNext() * itemPriceRatio;

        // 桁数
        int digits = Mathf.FloorToInt(Mathf.Log10(calcPrice))+1;
        // 丸める倍率
        int roundScale = (int)Mathf.Pow(10, (digits - 3));

        // 有効数字3桁に丸めて返す
        return Mathf.RoundToInt(calcPrice/roundScale) * roundScale;
    }

    private void BuyItem(){
        // ショップでなければ終了
        if(menuChanger.nowMenu != MenuEnum.Shop)return;
        
        // 値段を取得
        var price = GetItemPrice();

        if(price > moneyMan.nowMoney){
            // お金が足りないことを示すテキスト表示
            var txt = Instantiate(boughtText);
            txt.transform.SetParent(this.transform, false);
            // テキストを流す
            txt.GetComponent<ShopBuyText>().ShowText("お金が足りません！");
            // キャンセルseを鳴らす
            SoundManager.I.PlaySE("cancel");

        }else{
            // お金を引き、アイテムを増やす。セーブもする
            moneyMan.SubMoney(price);
            itemMan.AddUseItem(sellItemNumber);
            MainSceneDataManager.MCInst.SaveNowPlayingData();
            // 購入を示すテキスト表示
            var txt = Instantiate(boughtText);
            txt.transform.SetParent(this.transform, false);
            // テキストを流す
            txt.GetComponent<ShopBuyText>().ShowText();
            // SEを鳴らす
            SoundManager.I.PlaySE("buyItem");
        }
    }
}
