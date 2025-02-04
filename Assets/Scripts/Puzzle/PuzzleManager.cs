using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;


public enum SweetsEnum{
    cake,
    choko,
    ice,
    donut,
    mochi,
    None
};

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private ColumnManager[] columns = new ColumnManager[6];

    // 今スイーツをつかんでつなげている最中か
    [field : SerializeField]public bool isGrabSweets {get;private set;}
    // 今パズルできない状態か
    public bool cannotPuzzle {get;private set;} = false;

    // 繋いだスイーツの数
    private int numOfConnect = 0;
    // 繋いでいるスイーツのタイプ
    private SweetsEnum connectSweetsType = SweetsEnum.None;
    // 現在繋いでいる全スイーツのマネージャーリスト
    public List<SweetsManager> connectMans {get;private set;} = new List<SweetsManager>();

    // 今回上下左右の探知範囲を広げるか?    (使用アイテム用!)
    private bool isLongRangeThisTime = false;
    // スイーツ選択時に、探知範囲設定用に参照する関数のリスト
    public LongRangeSetter longRangeSetter;

    // パズルの行・列の数(ずっと6x6)
    public Vector2Int gridRange {get; private set;} = new Vector2Int(6, 6);
    // パズルグリッドの原点(左下)
    public Vector2 gridOrigin {get; private set;} = new Vector2(-5f, -5f);
    // グリッドの間隔の単位
    public float gridUnit {get; private set;} = 2f;

    [SerializeField] private float unitFallTimeBase = 0.25f;
    public float unitFallTime {get; private set;} = 0.25f;
    // スイーツを消すとき、スイーツをまとめる処理の時間
    [SerializeField] private float sweetsOrganizeTimeBase = 0.1f;
    private float sweetsOrganizeTime = 0.1f;
    // スイーツが消滅するアニメーション速度
    [SerializeField] private float sweetsDelDurationBase = 1f;
    private float sweetsDelDuration = 1f;

    [Space]
    // 連結を示す線
    private LineRenderer lineRend;
    // スコア管理
    private ScoreManager scoreMan;
    // ターン管理マネージャー
    [SerializeField] private TurnManager turnMan;
    // 消したスイーツの種類バッファ
    [SerializeField] private DeleteSweetsBuffer deleteBuffer;

    [Space]
    // メニューチェンジの可否を調整する
    [SerializeField] private MenuChanger menuChanger;

    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        scoreMan = GetComponent<ScoreManager>();

        // スイーツの初期配置
        BoardStartUp();

        SetNowDelAnimSpeed();
    }


    // Puzzleでの座標をワールド座標で返す
    public Vector3 PuzzlePositionToWorld(Vector2Int puzzlePos){
        float valueX = gridOrigin.x + puzzlePos.x * gridUnit;
        float valueY = gridOrigin.y + puzzlePos.y * gridUnit;
        return new Vector3(valueX, valueY, 0f);
    }
    
    // 最初にタッチしたスイーツを選択
    public void SelectFirstSweets(SweetsManager target){
        menuChanger.canChangeMenu = false;
        // 始点として線描画の点に加える
        lineRend.positionCount = 1;
        lineRend.SetPosition(0, PuzzlePositionToWorld(target.puzzlePos));
        isGrabSweets = true;
        target.isSelect = true;

        numOfConnect = 1;
        connectSweetsType = target.sweetsType;
        connectMans.Add(target);

        // 効果によって遠距離選択が有効化をチェック
        isLongRangeThisTime = longRangeSetter.GetCanLongRange();
        
        // 掴むSEも流す
        SoundManager.I.PlaySE("sweetsNazori");

        SetNextState();
    }

    // なぞったスイーツを選択
    public void TraceSweets(SweetsManager target){
        target.isSelect = true;

        // 接続状況の設定
        numOfConnect++;
        connectMans.Add(target);
        // ラインの設定
        lineRend.positionCount = numOfConnect;
        lineRend.SetPosition(numOfConnect-1, PuzzlePositionToWorld(target.puzzlePos));

        SoundManager.I.PlaySE("sweetsNazori");

        // 接続可能スイーツ再描画
        SetNextState();

    }

    // 一番最後につないだ要素をキャンセル
    public void CancelLastSweets(){
        // 末尾要素を取得し繋いでいるスイーツリストから除外
        var lastSweets = connectMans.Last();
        
        lastSweets.isSelect = false;

        numOfConnect--;
        connectMans.RemoveAt(connectMans.Count-1);
        lineRend.positionCount = numOfConnect;
        
        SetNextState();
    }

    // スイーツをつかむのをやめる
    public void CancelGrabSweets(){
        foreach(var manager in GetAllSweets()){
            if(manager){
                manager.isSelect = false;
                manager.canReach = true;
                manager.canSelect = true;
            }
        }
        connectMans.Clear();
        connectSweetsType = SweetsEnum.None;
        numOfConnect = 0;
        lineRend.positionCount = 0;

        isGrabSweets = false;
        menuChanger.canChangeMenu = true;
    }

    // 掴んでいるスイーツを消去し、そのスイーツのスコアを得る
    public IEnumerator DeleteSweets(){
        isLongRangeThisTime = false;
        longRangeSetter.AfterOperation();
        cannotPuzzle = true;
        menuChanger.canChangeMenu = false;

        // アニメーション速度更新
        SetNowDelAnimSpeed();

        // スイーツのタッチ可能フラグは事前に折る
        foreach(var sweets in GetAllSweets()){
            if(sweets)sweets.canSelect = false;
        }
        // 指を追いかけているトレーラーは削除
        lineRend.positionCount = numOfConnect;
        // 最初にスイーツをまとめるようなアニメーションをする
        for(int i=0;i<connectMans.Count-1;i++){
            // コンボ音を鳴らす
            SoundManager.I.PlaySE("combo");
            // 次につないだスイーツのローカルポジション
            var targetPos = PuzzlePositionToWorld(connectMans[i+1].puzzlePos);
            yield return StartCoroutine(connectMans[i].MovePosition(targetPos, sweetsOrganizeTime));
            // 動きの追従のため、次のスイーツを親にする
            //connectMans[i].transform.SetParent(connectMans[i+1].transform);
        }

        // 最後につないだスイーツのポジション
        var lastSweetsPos = connectMans.Last().transform.position;
        // 終点スイーツのポジションをスクリーン座標に変換
        var lastScreenPos = Camera.main.WorldToScreenPoint(lastSweetsPos);
        // スコア・コンボを描画させる
        scoreMan.DisplayScore(connectSweetsType, numOfConnect, lastScreenPos);
        // 動かせる数-1
        turnMan.SubRemainingMoves();
        // 繋いだスイーツをバッファに追加
        deleteBuffer.AddDeleteSweetsBuffer(connectSweetsType);
        
        // 繋いだスイーツのゲームオブジェクトを末尾から削除
        var _deleteProcess = new List<Coroutine>();
        for(int i = connectMans.Count-1;i>=0;i--){
            Vector2Int delSweetsPos = connectMans[i].puzzlePos;
            // 列のリストにnullとして登録
            columns[delSweetsPos.x].sweets[delSweetsPos.y] = null;
            // 消去する処理をコルーチンで開始
            _deleteProcess.Add(StartCoroutine(connectMans[i].DestroySweets(sweetsDelDuration)));
        }
        
        // 掴むのをやめる処理
        foreach(var manager in GetAllSweets()){
            if(manager){
                manager.isSelect = false;
                manager.canReach = true;
            }
        }
        connectMans.Clear();
        connectSweetsType = SweetsEnum.None;
        numOfConnect = 0;
        lineRend.positionCount = 0;

        // フルーツ消去待ち
        foreach(var _delProcess in _deleteProcess)yield return _delProcess;


        // 新たにスイーツを生成し、落下アニメーションさせる
        Coroutine fallProcess = StartCoroutine(FallSweets());
        // 落下処理の終了を待つ
        yield return fallProcess;

        // 落下後に選択可能に
        foreach(var sweets in GetAllSweets()){
            sweets.canSelect = true;
        }
        isGrabSweets = false;
        // ターン数が残っているならば操作可能にし、セーブする
        if(turnMan.remainingMoves > 0){
            MainSceneDataManager.MCInst.SaveNowPlayingData();
            cannotPuzzle = false;
            menuChanger.canChangeMenu = true;
        }
    }

    // スイーツ消去アニメーション時間の長さを設定する
    public void SetDelAnimSpeed(float speed){
        unitFallTime = unitFallTimeBase / speed;
        sweetsDelDuration = sweetsDelDurationBase / speed;
        sweetsOrganizeTime = sweetsOrganizeTimeBase / speed;
    }

    // オプションからデータを取得し，アニメーション速度を適用する
    public void SetNowDelAnimSpeed(){
        var data = DataManager.Inst.LoadOptionData();
        SetDelAnimSpeed(data.sweetsDelSpeed);
    }

    // スイーツ落下処理の統括をするコルーチン
    private IEnumerator FallSweets(){
        // 各列のスイーツ落下処理のリスト
        var fallCoroutines = new List<Coroutine>();
        foreach(var column in columns){
            // 各列でスイーツの落下処理を行う
            fallCoroutines.Add(StartCoroutine(column.FallSweetsColumn()));
        }

        // 全行の落下処理が終わるのを待つ
        foreach(var coroutine in fallCoroutines){
            yield return coroutine;
        }
    }

    // Puzzle盤面の次の状態を設定
    private void SetNextState(){
        // 現在位置から届きうるスイーツにフラグを立てる
        foreach(var sweets in GetAllSweets()){
            sweets.canReach = false;
            sweets.canSelect = false;
        }
        foreach(var sweets in GetCanReach(connectMans.Last(), isLongRangeThisTime)){
            sweets.canReach = true;
        }

        // 次に繋げられるスイーツにフラグを立てる
        foreach(var sweets in GetAroundSweets(connectMans.Last().puzzlePos, isLongRangeThisTime)){
            // 同じ種類のスイーツかつまだ繋げられていないならば
            if(sweets.sweetsType == connectSweetsType && !sweets.isSelect)sweets.canSelect = true;
        }
    }

    // 現在タッチしている点に線を描画
    public void DrawTouchPosition(Vector3 worldPoint){
        lineRend.positionCount = numOfConnect + 1;
        lineRend.SetPosition(numOfConnect, worldPoint);
    }

    // 現在盤面にあるすべてのスイーツのマネージャーリストを取得する
    private List<SweetsManager> GetAllSweets(){
        var value = new List<SweetsManager>();

        foreach(ColumnManager _column in columns){
            value.AddRange(_column.sweets);
        }

        return value;
    }

    // 周りにあるスイーツのスイーツマネージャーをリスト化し取得する
    private List<SweetsManager> GetAroundSweets(Vector2Int sweetsPos, bool isLongRange = false){
        var value = new List<SweetsManager>();
        
        // xが探知元として
        // 1 2 3 
        // 4 x 5
        // 6 7 8
        //の順で取得する。選択スイーツが端にあるなら弾く

        if(sweetsPos.x != 0 &&             sweetsPos.y != gridRange.y-1)value.Add(columns[sweetsPos.x-1].sweets[sweetsPos.y+1]);
        if(                                sweetsPos.y != gridRange.y-1)value.Add(columns[sweetsPos.x]  .sweets[sweetsPos.y+1]);
        if(sweetsPos.x != gridRange.x-1 && sweetsPos.y != gridRange.y-1)value.Add(columns[sweetsPos.x+1].sweets[sweetsPos.y+1]);
        if(sweetsPos.x != 0                                            )value.Add(columns[sweetsPos.x-1].sweets[sweetsPos.y]);
        if(sweetsPos.x != gridRange.x-1                                )value.Add(columns[sweetsPos.x+1].sweets[sweetsPos.y]);
        if(sweetsPos.x != 0 &&             sweetsPos.y != 0            )value.Add(columns[sweetsPos.x-1].sweets[sweetsPos.y-1]);
        if(                                sweetsPos.y != 0            )value.Add(columns[sweetsPos.x]  .sweets[sweetsPos.y-1]);
        if(sweetsPos.x != gridRange.x-1 && sweetsPos.y != 0            )value.Add(columns[sweetsPos.x+1].sweets[sweetsPos.y-1]);

        // ロングレンジ選択なら、追加で
        //     1
        //   x x x
        // 2 x x x 3
        //   x x x
        //     4
        // の範囲を取得
        if(isLongRange){
            if(sweetsPos.y < gridRange.y-2)value.Add(columns[sweetsPos.x]  .sweets[sweetsPos.y+2]);
            if(sweetsPos.x > 1            )value.Add(columns[sweetsPos.x-2]  .sweets[sweetsPos.y]);
            if(sweetsPos.x < gridRange.x-2)value.Add(columns[sweetsPos.x+2]  .sweets[sweetsPos.y]);
            if(sweetsPos.y > 1            )value.Add(columns[sweetsPos.x]  .sweets[sweetsPos.y-2]);
        }

        return value;
    }

    // 特定のスイーツマネージャー引数として、現在繋いでいるスイーツタイプから、接続できるスイーツを返す
    private List<SweetsManager> GetCanReach(SweetsManager sweetsMan, bool isLongRange = false){
        // 探査用キュー
        var work_queue = new Queue<Vector2Int>();
        // 探査済み保存リスト
        var searched = new List<SweetsManager>();
        
        work_queue.Enqueue(sweetsMan.puzzlePos);

        // 未探査スイーツがなくなるまで走査
        while(work_queue.Count != 0){
            var nowPos = work_queue.Dequeue();
            // 周りのスイーツのリストを取得
            var aroundSweets = GetAroundSweets(nowPos, isLongRange);

            foreach(SweetsManager _sweetsMan in aroundSweets){
                // スイーツタイプが異なる・選択済みならば除外
                if(_sweetsMan.sweetsType != connectSweetsType || _sweetsMan.isSelect)continue;

                // 走査済みでないなら走査キュー更新&探査済リストに追加
                if(!searched.Contains(_sweetsMan)){
                    work_queue.Enqueue(_sweetsMan.puzzlePos);
                    searched.Add(_sweetsMan);
                }
            }
        }

        return searched;
    }

    // パズル出来ない状態かのセッター
    public void SetCannotPuzzle(bool value){
        cannotPuzzle = value;
    }


    public IEnumerator Item_ResetBoard(){
        // パズル、メニュー遷移不能に
        SetCannotPuzzle(true);
        menuChanger.canChangeMenu = false;

        // 全列全スイーツに削除命令→全列のスイーツリストを全部空に
        // →列にスイーツを降らせる命令

        var deleteProcess = new List<Coroutine>();
        foreach(var column in columns){
            for(int i=0;i<column.sweets.Count;i++){
                // スイーツ削除の処理
                deleteProcess.Add(StartCoroutine(column.sweets[i].DestroySweets(sweetsDelDuration)));
                // 処理を予約したらnullに
                column.sweets[i] = null;
            }
        }

        // 削除処理の終了を待つ
        foreach(var process in deleteProcess){
            yield return process;
        }

        // 新たにスイーツを生成し、落下アニメーションさせる
        yield return StartCoroutine(FallSweets());

        // データをセーブ
        MainSceneDataManager.MCInst.SaveNowPlayingData();
        // パズル、メニュー遷移可能に
        SetCannotPuzzle(false);
        menuChanger.canChangeMenu = true;
    }

    /// <summary>
    /// 現在の盤面状況をスイーツの二重配列としてエンコードする
    /// </summary>
    /// <returns></returns>
    public SweetsEnum[] EncodeBoard(){
        var _board = new SweetsEnum[gridRange.x * gridRange.y];
        // 各行各列からスイーツのタイプを取りだす
        for(int i=0;i<gridRange.x;i++){
            // 保有しているスイーツが足りなかったら
            if(columns[i].sweets.Count < gridRange.y){
                return null;
            }
            for(int j=0;j<gridRange.y;j++){
                _board[i*gridRange.y + j] = columns[i].sweets[j].sweetsType;
            }
        }
        return _board;
    }

    public bool DecodeBoard(SweetsEnum[] sweetsData){
        // 存在しないなら失敗
        if(sweetsData == null)return false;

        // 引数が盤面のサイズに合わないなら失敗
        if(sweetsData.Length != gridRange.x * gridRange.y){
            return false;
        }

        for(int i=0;i<gridRange.x;i++){
            for(int j=0;j<gridRange.y;j++){
                columns[i].InstantiateSweetsFromEnum(sweetsData[i*gridRange.y + j]);
            }
        }
        return true;
    }

    /// <summary>
    /// 盤面の初期配置を決定
    /// </summary>
    private void BoardStartUp(){
        // データをロード
        var data = DataManager.Inst.LoadPlayingData();
        
        if(!DecodeBoard(data.nowBoard)){
            Debug.Log("ロード出来なかったね...");
            // デコード失敗した際の処理(初回起動時等)
            // ランダムなスイーツを生成
            for(int i=0;i<gridRange.x;i++){
                for(int j=0;j<gridRange.y;j++){
                    columns[i].InstantiateRandomSweets();
                }
            }
            // 生成後にセーブ
            MainSceneDataManager.MCInst.SaveNowPlayingData();
        }
    }
}
