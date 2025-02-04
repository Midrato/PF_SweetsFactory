using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveLottery : MonoBehaviour
{   
    // 全パッシブデータ取得元
    [SerializeField] private AllPassiveData allPassives;
    // パッシブ所持の管理を行う
    [SerializeField] private PassiveHolder holder;
    [Header("レア度別排出率(降順)")]
    [SerializeField] private float[] rareDropRate = new float[2];

    // 抽選元
    public List<PassiveItemBase> passiveLotteryList = new List<PassiveItemBase>();
    // レア度別抽出リスト
    public List<List<PassiveItemBase>> passivesByRare = new List<List<PassiveItemBase>>();

    void Start()
    {
        // 値渡しで取得
        passiveLotteryList = new List<PassiveItemBase>(allPassives.allPassiveItems);

        // セーブデータから入手済みのパッシブを排出リストから除外
        var data = DataManager.Inst.LoadPlayingData();

        if(data.havingItemsNum != null){
            List<int> sorted = new List<int>(data.havingPassives);
            // ソートされたパッシブリストを用意
            sorted.Sort();

            for(int i=sorted.Count;i > 0;i--){
                Debug.Log("除外するパッシブ番号 : " + sorted[i-1]);
                passiveLotteryList.RemoveAt(sorted[i-1]);
            }
        }
        
        SetPassivesByRare();

        //for(int i=0;i<30;i++)LotteryItem();
    }

    private void SetPassivesByRare(){
        // 最初に中身をクリア
        passivesByRare.Clear();
        // レア度の数だけリストを用意
        for(int i=0;i<allPassives.maxRare;i++)passivesByRare.Add(new List<PassiveItemBase>());

        foreach(var passive in passiveLotteryList){
            var rank = passive.itemRank;
            // レア度のリストに収める
            passivesByRare[rank-1].Add(passive);
        }
    }

    // パッシブアイテムをN回被り無しで抽選する
    public List<PassiveItemBase> LotteryItemsNTimes(int N){
        var value = new List<PassiveItemBase>();
        // もし抽出数が抽出元リストより多いなら丸ごと返す
        if(N >= passiveLotteryList.Count){
            return new List<PassiveItemBase>(passiveLotteryList);
        }

        for(int i=0;i<N;i++){
            if(i == 0){
                value.Add(LotteryItem());
            }else{
                // 2回目以降、被ったら再抽選
                var getItem = LotteryItem();
                // 被っているかの判定
                bool isLotteryed = false;
                for(int j=0;j<i;j++){
                    if(getItem == value[j]){
                        isLotteryed = true;
                        break;
                    }
                }

                if(isLotteryed){
                    i--;
                    continue;
                }else{
                    // 被らなかったら追加
                    value.Add(getItem);
                }
            }
        }
        return value;
    }

    // パッシブアイテムを抽選する
    public PassiveItemBase LotteryItem(){
        SetPassivesByRare();
        
        // 全アイテムが入手済みなら抜ける
        if(passiveLotteryList.Count == 0){
            Debug.Log("アイテムもうないです！");
            return null;
        }


        // 抽出アイテムのランク・そのランクのトッピングモジュールの数
        int lotteryItemRank;
        int rareListLength;

        // ランクのトッピングモジュールが0ならリロール
        while(true){
            // レア度を抽出
            lotteryItemRank = GetRandomRank();
            rareListLength = passivesByRare[lotteryItemRank-1].Count;
            // そのランクのトッピングモジュールがあるならループ抜ける
            if(rareListLength != 0){
                break;
            }
        }

        // レア度別リストからアイテムを抽出
        var lotteryItem = passivesByRare[lotteryItemRank-1][UnityEngine.Random.Range(0, rareListLength)];
        Debug.Log("抽出アイテム:" + lotteryItem.itemName);

        return lotteryItem;
    }

    public int GetRandomRank(){
        int lotteryRank = 0;

        // 各確率の閾値
        float[] hresholds = new float[allPassives.maxRare-1];
        for(int i=0;i<hresholds.Length;i++){
            if(i == 0){
                hresholds[i] = rareDropRate[i];
            }else{
                hresholds[i] = hresholds[i-1] + rareDropRate[i];
            }
        }
        
        // 乱数
        float rand = UnityEngine.Random.Range(0f, 1f);

        // 確率は降順で設定したので逆に適用ランクは逆に
        for(int i=0;i<allPassives.maxRare;i++){
            if(i==0){
                // 最初の範囲は0~閾値0
                if(rand <= hresholds[i]){
                    lotteryRank = allPassives.maxRare - i;
                    break;
                }
            }else if(i==(allPassives.maxRare-1)){
                // 最後の範囲は閾値ラスト~1
                if(rand > hresholds[i-1]){
                    lotteryRank = allPassives.maxRare - i;
                    break;
                }
            }else{
                // その他は閾値i-1~閾値i
                if(rand > hresholds[i-1] && rand <= hresholds[i]){
                    lotteryRank = allPassives.maxRare - i;
                    break;
                }
            }
        }

        Debug.Log("ItemRank:" + lotteryRank);
        return lotteryRank;
    }
}
