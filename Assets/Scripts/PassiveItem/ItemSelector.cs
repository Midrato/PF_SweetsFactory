using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{
    // パッシブ保存先
    [SerializeField] private PassiveHolder holder;
    // パッシブ抽選用
    [SerializeField] private PassiveLottery lottery;
    // アイテム決定用のエンターボタン
    [SerializeField] private Button enterButton;
    // パッシブアイテム列挙用のバーオブジェクト
    [SerializeField] private GameObject passiveItemBar;

    // バーを表示し始める点のトランスフォーム
    [SerializeField] private RectTransform barTargetPoint;
    // バー間の距離
    [SerializeField] private int barDistance = 40;
    // バーの出現時間差
    [SerializeField] private float barDuration = 0.2f;
    // 生み出したパッシブバーの保管用
    private List<PassiveItemBar> passiveBars = new List<PassiveItemBar>();

    // どのアイテムを選択しているか、インデックスで表現(-1は未選択)
    private int itemSelect = -1;
    // 抽選アイテムの保管
    private List<PassiveItemBase> lotteryedItems = new List<PassiveItemBase>();

    // 入手が確定したアイテム
    public PassiveItemBase decisionItem = null;

    // リロール動作のをしているかのチェッカー
    public bool canReroll {get;private set;} = false;
    public void SetCanReroll(bool value){canReroll = value;}
    // 画面上のボタンのキャンバスグループ
    [SerializeField] private List<CanvasGroup> buttonCanvs = new List<CanvasGroup>();

    // パッシブを抽選した回数
    private int lotteryTimes = 0;

    // パッシブを受け取り拒否したか
    public bool isRejectSelection = false;

    void Start()
    {
        enterButton.interactable = false;
        itemSelect = -1;

        enterButton.onClick.AddListener(EnterItem);
    }

    void Update()
    {
        // buttonが操作可能になるのを感知する
        enterButton.interactable = itemSelect != -1;
    }

    // 必要なコンポーネント類をセットする
    public void SetNeedComponents(PassiveHolder _holder, PassiveLottery _lottery){
        holder = _holder;
        lottery = _lottery;
    }

    private IEnumerator SummonPassiveBar(int options){
        // 選択リスト一旦リセット
        canReroll = false;
        itemSelect = -1;
        passiveBars.Clear();
        lotteryedItems.Clear();

        // 抽出済みならセーブデータから取得
        var data = MainSceneDataManager.MCInst.LoadLotteryPassives(lotteryTimes, options);
        if(data != null){
            // 抽出リストをデコード
            lotteryedItems = holder.DecodePassive(data);
        }else{
            // アイテムを抽選
            lotteryedItems = lottery.LotteryItemsNTimes(options);
            // 抽出パッシブをセーブ
            MainSceneDataManager.MCInst.SaveLotteryPassives(holder.EncodePassive(lotteryedItems));
        }
        // 抽選回数カウント
        lotteryTimes++;
        

        // options数だけアイテムがない可能性があるため、アイテム数を使っていく
        int itemNum = lotteryedItems.Count;

        for(int i=0;i<itemNum;i++){
            // 最初のポジションを設定
            float firstPosX = ScreenSizeGetter.I.GetTrueScreenSize().x;
            // 高さはiに応じて低く
            float firstPosY = -i * (passiveItemBar.GetComponent<RectTransform>().sizeDelta.y + barDistance);;

            // パッシブバーを召喚
            var itemWindow = Instantiate(passiveItemBar);
            itemWindow.transform.SetParent(barTargetPoint, false);
            // バーの初期位置を設定
            itemWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(firstPosX, firstPosY);

            // パッシブアイテムバーの設定のためコンポーネントを取得
            passiveBars.Add(itemWindow.GetComponent<PassiveItemBar>());
            // アイテムデータのセット
            passiveBars[i].SetItemData(lotteryedItems[i]);
            // 押したボタンを有効にする
            int nowIndex = i;
            passiveBars[i].SetButtonAction(() => {
                ResetSelection();
                itemSelect = nowIndex;
                passiveBars[nowIndex].isSelect = true;
            });
            SoundManager.I.PlaySE("showPassive");
            yield return new WaitForSeconds(barDuration);
        }
        // パッシブバーが出てくるまで少し待つ
        yield return new WaitForSeconds(passiveBars.Count >= 1 ? passiveBars[0].showDuration : 0);
        // リロール可能に
        canReroll = true;
    }

    public IEnumerator MakeSelection(int options){

        // パッシブバーを出す
        yield return StartCoroutine(SummonPassiveBar(options));

        // 入手アイテムが確定するまで処理終了待ち
        while(!decisionItem && !isRejectSelection){
            yield return null;
        }
        Debug.Log("確定だ！");
        // 選択バーをどかす
        yield return StartCoroutine(EndSelectPassive());
    }

    public void EnterItem(){
        // 操作不可能だったらなにもしない
        if(!canReroll)return;
        canReroll = false;

        // 抽選回数リセット
        lotteryTimes = 0;
        // 決定アイテムとして追加
        decisionItem = lotteryedItems[itemSelect];
        // ホルダーにアイテムを追加する
        holder.AddPassiveItem(decisionItem);
        // 抽出元リストから除外
        lottery.passiveLotteryList.Remove(decisionItem);
        
        // 全ボタンを選択不可に
        itemSelect = -1;
        foreach(var bars in passiveBars)bars.SetButtonInteractable(false);
        enterButton.interactable = false;
    }

    // 全選択をリセット
    private void ResetSelection(){
        for(int i=0;i<passiveBars.Count;i++){
            passiveBars[i].isSelect = false;
        }
    }

    private IEnumerator ReleaseBars(){
        Sequence releaseSeq = DOTween.Sequence();
        for(int i=0;i<passiveBars.Count;i++){
            // 最初にバーをタッチ不能に
            passiveBars[i].SetButtonInteractable(false);
            // バーを退場させる。遅延は登場時と同様
            releaseSeq.Join(passiveBars[i].GetComponent<RectTransform>().DOAnchorPosX(-ScreenSizeGetter.I.GetTrueScreenSize().x, barDuration*4)
                                            .SetEase(Ease.InQuart)
                                            .SetRelative()
                                            .SetDelay(barDuration * i)
            );
        }
        yield return releaseSeq.WaitForCompletion();
        // 全バーが画面外に行ったら削除する
        for(int i=passiveBars.Count-1;i>=0;i--){
            Destroy(passiveBars[i].gameObject);
            passiveBars.RemoveAt(i);
        }
    }

    public IEnumerator EndSelectPassive(){
        // ボタンを消した後にバーをどかす
        FadeButtons(0.3f);
        yield return enterButton.GetComponent<CanvasGroup>().DOFade(0, 0.3f).WaitForCompletion();
        yield return StartCoroutine(ReleaseBars());
    }

    // ボタンをフェードアウトさせる
    private Sequence FadeButtons(float duration){
        var seq = DOTween.Sequence();
        foreach(var buttonCanv in buttonCanvs){
            seq.Join(buttonCanv.DOFade(0, duration));
        }
        return seq;
    }

    // 入手可能パッシブが残っているか
    public bool isRemainingPassive(){
        return lottery.passiveLotteryList.Count == 0;
    }

    // パッシブをリロールする アニメーション中でリロール出来ないならfalseを返す
    public bool RerollPassive(){
        // 連打対策
        if(canReroll){
            canReroll = false;
            StartCoroutine(_RerollPassive());
            return true;
        }
        return false;
    }

    private IEnumerator _RerollPassive(){
        // 選択解除
        itemSelect = -1;
        decisionItem = null;
        ResetSelection();
        yield return StartCoroutine(ReleaseBars());
        yield return StartCoroutine(SummonPassiveBar(3));
    }
}
