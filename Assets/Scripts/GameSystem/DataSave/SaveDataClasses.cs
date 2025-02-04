using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 今までの最高売上等の恒久的なセーブデータ
/// </summary>
[System.Serializable]
public class AccumulativeData
{
    /// <summary>
    /// プレイ成績ランキング
    /// </summary>
    public List<EndPlayData> gradeRanking = new List<EndPlayData>();

    // ランキングを何位まで残すか
    public int leaveRankNum = 10;

    /// <summary>
    /// 発見したモジュールリスト 発見しているならtrue 図鑑用
    /// </summary>
    /// <typeparam name="bool"></typeparam>
    /// <returns></returns>
    public List<bool> foundPassives = new List<bool>();

    /// <summary>
    /// ランキングデータに結果を追加する
    /// </summary>
    public int AddRanking(EndPlayData data){
        // データが何位の物か(初期は最下位とする)
        int rank = gradeRanking.Count;

        Debug.Log(gradeRanking.Count + "個データあるよ");

        // 初回だったら1位として次へ
        if(gradeRanking.Count == 0){
            gradeRanking.Add(data);
            return 1;
        }
        
        // 稼いだ総額→経営期間によってランキングする
        // 一度も条件にひっかからなかったなら最下位(なにもしない)
        for(int i=0;i<gradeRanking.Count;i++){
            if(data.totalSales == gradeRanking[i].totalSales){
                if(data.lastYear == gradeRanking[i].lastYear){
                    // 現在の季節が遅いかで判定
                    rank = (int)data.lastSeason >= (int)gradeRanking[i].lastSeason ? i : i+1;
                }else{
                    // 現在の年が遅いかで判定
                    rank = data.lastYear > gradeRanking[i].lastYear ? i : i+1;
                }
                break;
            }else if(data.totalSales >= gradeRanking[i].totalSales){
                rank = i;
                break;
            }
        }
        

        Debug.Log(rank+1 + "位！" + rank + " " + leaveRankNum);
        // 結果に応じて代入を行う
        if(rank < leaveRankNum){
            Debug.Log("インサートしたよ");
            if(rank > gradeRanking.Count){
                gradeRanking.Add(data);
            }else{
                gradeRanking.Insert(rank, data);
            }

            // 要素数がランキング数を超えたら上位10データを取得
            if(gradeRanking.Count > leaveRankNum)gradeRanking = gradeRanking.GetRange(0, leaveRankNum);
            
        }
        // 順位を返す
        return rank+1;
    }

    public void AddFindPassive(int passiveNum){
        if(foundPassives.Count <= passiveNum){
            // 足りないリスト要素だけ要素を足す
            var addList = new List<bool>();
            for(int i=0;i<passiveNum - foundPassives.Count +1;i++)addList.Add(false);
            foundPassives.AddRange(addList);
        }
        foundPassives[passiveNum] = true;
    }

    public bool GetIsFoundPassive(int passiveNum){
        if(foundPassives.Count <= passiveNum){
            // 足りないリスト要素だけ要素を足す
            var addList = new List<bool>();
            for(int i=0;i<passiveNum - foundPassives.Count +1;i++)addList.Add(false);
            foundPassives.AddRange(addList);
        }
        return foundPassives[passiveNum];
    }
}

/// <summary>
/// 現在プレイしている回のセーブデータ
/// </summary>
[System.Serializable]
public class PlayingData
{
    /// <summary>
    /// 現在の年
    /// </summary>
    public int nowYear = 1;

    /// <summary>
    /// 現在のシーズン
    /// </summary>
    public SeasonEnum nowSeason = SeasonEnum.Spring;

    /// <summary>
    /// 現在の残り操作回数
    /// </summary>
    public int remainingMoves = 20;

    /// <summary>
    /// 現在の所持金
    /// </summary>
    public int nowMoney = 0;

    /// <summary>
    /// この回での売上の合計
    /// </summary>
    public int totalMoney = 0;

    /// <summary>
    /// 現在所持しているパッシブ
    /// </summary>
    public List<int> havingPassives = new List<int>();

    /// <summary>
    /// 現在所持しているアイテムの個数
    /// </summary>
    public int[] havingItemsNum = new int[3];

    /// <summary>
    /// この回のアイテム使用状況
    /// </summary>
    public int[] thisTimeItemUsage = new int[3];

    /// <summary>
    /// 現在の盤面にあるスイーツの情報 Jsonで保存するために2次元の要素を1次元に下ろしている
    /// </summary>
    public SweetsEnum[] nowBoard = null;

    /// <summary>
    /// 次に抽選されるモジュールの番号のリスト
    /// </summary>
    public List<int> lotteryPassiveLists = new List<int>();

    public void SetPlayingData(
        int _nowYear,
        SeasonEnum _nowSeason,
        int _remainingMoves,
        int _nowMoney,
        int _totalMoney,
        List<int> _havingPassives,
        int[] _havingItemNum,
        int[] _thisTimeItemUsage,
        SweetsEnum[] _nowBoard,
        List<int> _lotPassiceList
        ){
            nowYear = _nowYear;
            nowSeason = _nowSeason;
            remainingMoves = _remainingMoves;
            nowMoney = _nowMoney;
            totalMoney = _totalMoney;
            havingPassives = _havingPassives;
            havingItemsNum = _havingItemNum;
            thisTimeItemUsage = _thisTimeItemUsage;
            nowBoard = _nowBoard;
            lotteryPassiveLists = _lotPassiceList;
        }
}

/// <summary>
/// 回の成績を表すデータ
/// </summary>
[System.Serializable]
public class EndPlayData
{
    /// <summary>
    /// 最後の年
    /// </summary>
    public int lastYear = 1;
    
    /// <summary>
    /// 最後の季節
    /// </summary>
    public SeasonEnum lastSeason = SeasonEnum.Spring;

    /// <summary>
    /// 累計売上
    /// </summary>
    public int totalSales = 0;

    public bool isClear = false;

    /// <summary>
    /// 記録日
    /// </summary>
    public string recordedDay = "";

    public void SetEndPlayData(int _lastYear, SeasonEnum _lastSeason, int _totalSales, string _recordedDay, bool _isClear=false){
        lastYear = _lastYear;
        lastSeason = _lastSeason;
        totalSales = _totalSales;
        recordedDay = _recordedDay;
        isClear = _isClear;
    }

    
}

/// <summary>
/// 音量等のオプションを収めたデータ
/// </summary>
[System.Serializable]
    public class OptionData
    {
        /// <summary>
        /// BGMの大きさ
        /// </summary>
        public float bgmVolume = 1;
        /// <summary>
        /// SEの大きさ
        /// </summary>
        public float seVolume = 1;
        
        /// <summary>
        /// スイーツ連鎖アニメーション速度
        /// </summary>
        public float sweetsDelSpeed = 1;

        /// <summary>
        /// チュートリアルを表示したか
        /// </summary>
        public bool isShowTutorial = false;

        public OptionData(float _bgmVolume = 1, float _seVolume = 1, float _sweetsDelSp = 1, bool _isShowTutorial = false){
            bgmVolume = _bgmVolume;
            seVolume = _seVolume;
            sweetsDelSpeed = _sweetsDelSp;
        }

        // セーブデータの値が外れ値なら修正する
        public void CheckValue(){
            if(bgmVolume > 1 || bgmVolume < 0)bgmVolume = 1f;
            if(seVolume > 1 || seVolume < 0)seVolume = 1f;
            if(sweetsDelSpeed > 2f || sweetsDelSpeed < 0.8f)sweetsDelSpeed = 1f;
        }
    }