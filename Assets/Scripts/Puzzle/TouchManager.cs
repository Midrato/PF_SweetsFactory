using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchManager : MonoBehaviour
{

    private PuzzleManager pzMan;
    private GeneralTouchSensor touchSens;

    // 現在が工場メニューか取得用
    [SerializeField] private MenuChanger menuChanger;
    void Start()
    {
        pzMan = GetComponent<PuzzleManager>();
        touchSens = new GeneralTouchSensor();
    }

    void Update()
    {
        if((touchSens.GetTouch() || touchSens.GetTouchUp()) && !pzMan.cannotPuzzle && menuChanger.isTargetMenu(MenuEnum.Factory)){
            // タッチ位置のワールド座標を二次元ベクトルとして取得
            Vector2 worldPoint = GetTouchToWorldPoint(touchSens.TouchPosition());
            if(touchSens.GetTouchDown()){
                // クリックした瞬間の処理
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if(hit){
                    // クリックしたオブジェクトがスイーツなら
                    if(hit.collider.gameObject.tag == "Sweets"){
                        Debug.Log("HIT!");
                        // クリックしたスイーツが選択可ならば取得し、スイーツの始点とする
                        var _sweetMan = hit.collider.gameObject.GetComponent<SweetsManager>();
                        if(_sweetMan.canSelect){
                            pzMan.SelectFirstSweets(_sweetMan);
                        }
                    }
                }
            }else if(touchSens.GetTouchUp() && pzMan.isGrabSweets){
                // クリック解除時の処理
                pzMan.DrawTouchPosition(worldPoint);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if(hit){
                    if(hit.collider.gameObject.tag == "Sweets"){
                        var _sweetMan = hit.collider.gameObject.GetComponent<SweetsManager>();
                        if(_sweetMan == pzMan.connectMans.Last()){
                            // スイーツを消して得点に
                            StartCoroutine(pzMan.DeleteSweets());
                        // それ以外... スイーツをつかむのをキャンセル
                        }else pzMan.CancelGrabSweets();
                    }else pzMan.CancelGrabSweets();
                }else pzMan.CancelGrabSweets();
            }else if(pzMan.isGrabSweets){
                // なぞっている時の処理
                pzMan.DrawTouchPosition(worldPoint);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if(hit){
                    // スイーツをなぞったら
                    if(hit.collider.gameObject.tag == "Sweets"){
                        var _sweetMan = hit.collider.gameObject.GetComponent<SweetsManager>();
                        if(_sweetMan.canSelect){
                            // 繋げられるなら繋ぐ関数を起動
                            pzMan.TraceSweets(_sweetMan);
                        }else if(pzMan.connectMans.Count >= 2){
                            if(_sweetMan == pzMan.connectMans[pzMan.connectMans.Count-2]){
                                // 2つ以上なぞっていてかつ、一つ前の要素に触れたら
                                pzMan.CancelLastSweets();
                            }
                        }
                    }
                }
            }
        }
        
    }
    // Touchからタッチした点のワールド座標を取得する
    private Vector3 GetTouchToWorldPoint(Vector2 touchPosition){
        Vector3 cameraPosition = new Vector3(touchPosition.x, touchPosition.y, 10f);
        // タッチ位置のワールド座標
        return Camera.main.ScreenToWorldPoint(cameraPosition);
    }

}
