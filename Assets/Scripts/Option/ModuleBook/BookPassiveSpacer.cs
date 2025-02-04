using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookPassiveSpacer : ObjectSpacer
{
    [SerializeField] private AllPassiveData passiveData;
    [SerializeField] private OptContentsWindow bookWindow;
    [SerializeField] private BookViewer viewer;
    // まだ発見していないことを伝える用テキスト
    [SerializeField] private TextMeshProUGUI isNotFoundText;
    // モジュール発見数を表示するテキスト
    [SerializeField] private TextMeshProUGUI foundPassiveNumText;

    [SerializeField] private GameObject bookButtonPrefab;

    protected override List<RectTransform> InstObjects()
    {
        // 発見済みモジュールリストを取得するためロード
        var accumData = DataManager.Inst.LoadAccumulativeData();
        var bookButtonList = new List<RectTransform>();
        int founds = 0;
        for(int i=0;i < passiveData.allPassiveItems.Count;i++){
            var bButton = Instantiate(bookButtonPrefab);
            var _bButton = bButton.GetComponent<PassiveViewButton>();
            
            // この番号のパッシブを発見済みなら、ボタンを押したら説明を表示するように
            var thisPassive = passiveData.allPassiveItems[i];
            if(accumData.GetIsFoundPassive(i)){
                founds++;
                _bButton.SetButton(thisPassive.itemSprite, () => ShowPassiveStatus(thisPassive));
                _bButton.SetButtonImageCol(Color.white);
            }else{
                _bButton.SetButton(thisPassive.itemSprite, TouchIsNotFound);
                _bButton.SetButtonImageCol(Color.black);
            }
            var bButtonTrans = bButton.GetComponent<RectTransform>();
            bButtonTrans.SetParent(contentsTrans, false);
            bookButtonList.Add(bButtonTrans);
        }
        foundPassiveNumText.text = $"発見モジュール  {founds}/{passiveData.allPassiveItems.Count}";
        return bookButtonList;
    }

    public void ShowPassiveStatus(PassiveItemBase item){
        if(bookWindow.isCanControlOption){
            SoundManager.I.PlaySE("button1");
            viewer.SetItemData(item);
        }
    }

    public void TouchIsNotFound(){
        if(bookWindow.isCanControlOption){
            SoundManager.I.PlaySE("cancel");
            viewer.ResetViewer();
            isNotFoundText.text = "未発見";
        }
    }
}
