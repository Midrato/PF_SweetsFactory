using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnManager : MonoBehaviour
{
    // 必要な他オブジェクトのマネージャー類
    [SerializeField] private GameObject puzzleMan;
    private PuzzleManager _puzzleMan;
    private SweetsTypeManager _sweetTypeMan;

    // 列の要素数
    public int numOfElements {get;private set;} = 6;

    // このColumnManagerが何列目か
    [SerializeField]private int ThisIndex;
    public int thisIndex {
        get{return ThisIndex;} 
        private set{ThisIndex = value;}
    }

    // 子のスイーツのリスト(6以上になる可能性があるためリスト)
    [field : SerializeField]public List<SweetsManager> sweets {get;private set;} = new List<SweetsManager>();
    void Awake()
    {
        _puzzleMan = puzzleMan.GetComponent<PuzzleManager>();
        _sweetTypeMan = puzzleMan.GetComponent<SweetsTypeManager>();

        /*// デバッグ初期生成
        for(int i=0;i<numOfElements;i++){
            InstantiateRandomSweets();
        }*/
    }

    // 新たなスイーツのインスタンスを生成する
    private SweetsManager InstantiateSweets(GameObject spawnSweets){
        int listIndex = sweets.Count;
        // 生み出したスイーツのローカルポジション
        Vector2 objPos = new Vector2(0, listIndex * _puzzleMan.gridUnit);
        var Obj = Instantiate(spawnSweets);
        // 親を自分(の列)に
        Obj.transform.parent = this.gameObject.transform;
        Obj.transform.localPosition = objPos;
        // パズル上の位置も設定
        var newSweetsMan = Obj.GetComponent<SweetsManager>();
        newSweetsMan.puzzlePos = new Vector2Int(thisIndex, listIndex);

        sweets.Add(newSweetsMan);
        return newSweetsMan;
    }
    
    // ランダムなスイーツのインスタンスを生成する
    public SweetsManager InstantiateRandomSweets(){
        var value = InstantiateSweets(_sweetTypeMan.PickRandomSweets());
        return value;
    }

    /// <summary>
    /// スイーツ名を受け取り、スイーツを生成する
    /// </summary>
    /// <param name="sweetsEnum"></param>
    /// <returns></returns>
    public SweetsManager InstantiateSweetsFromEnum(SweetsEnum sweetsEnum){
        var value = InstantiateSweets(_sweetTypeMan.allSweetsType[(int)sweetsEnum]);
        return value;
    }

    // 子のスイーツの座標をリセットする
    private void ResetPuzzlePosition(){
        for(int i=0;i<sweets.Count;i++){
            sweets[i].puzzlePos = new Vector2Int(thisIndex, i);
            
            Vector2 objPos = new Vector2(0, i * _puzzleMan.gridUnit);
            sweets[i].transform.localPosition = objPos;
        }
    }

    public IEnumerator FallSweetsColumn(){
        int deletes = CountNullsInSweets();
        // 足りない分だけスイーツ生成
        for(int i=0;i<deletes;i++)InstantiateRandomSweets();

        // null要素を削除しつつ末尾から走査して、各要素の落下ブロック数をカウント
        var fallDistances = new List<int>();
        for(int i=sweets.Count-1;i>=0;i--){
            if(sweets[i] != null){
                // スイーツがあるなら落下ブロック数0として先頭に追加
                fallDistances.Insert(0, 0);
            }else{
                // スイーツがないならリストから削除し、落下ブロックリストの要素全てに+1
                sweets.RemoveAt(i);
                for(int j=0;j<fallDistances.Count;j++)fallDistances[j]++;
            }
        }
        // 落下ブロック数を元に落下させる
        var _fallSweets = StartCoroutine(DoFallSweets(fallDistances));
        
        yield return _fallSweets;
        // 最後に位置をリセットして整える
        ResetPuzzlePosition();
    }

    // 子スイーツリストから消去されたものの数を数える
    private int CountNullsInSweets(){
        int value = 0;
        for(int i = sweets.Count-1;i>=0;i--){
            if(sweets[i] == null)value++;
        }
        //Debug.Log("列" + thisIndex + " 削除された要素数" + value);
        return value;
    }

    // 各要素の落ちるブロック数リストを引数として、実際に落とす
    private IEnumerator DoFallSweets(List<int> fallDistances){
        var _fallcoroutines = new List<Coroutine>();
        for(int i=0;i<sweets.Count;i++){
            // 落ちるカウントが0より大きいときにコルーチン実行
            if(fallDistances[i] > 0){
                _fallcoroutines.Add(StartCoroutine(sweets[i].FallOwn(fallDistances[i], _puzzleMan.gridUnit, _puzzleMan.unitFallTime)));
            }
        }
        // 全部終了するのを待つ
        foreach(var coroutine in _fallcoroutines){
            yield return coroutine;
        }
    }
}
