using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class SweetsManager : MonoBehaviour
{
    // スイーツのタイプ
    [field : SerializeField]public SweetsEnum sweetsType {get;private set;}
    // そのスイーツの盤面上の位置
    public Vector2Int puzzlePos;
    // スイーツのデフォルトスケール
    public Vector2 defaultScale {get;private set;} = new Vector2(1.3f, 1.3f);
    // 選択中のスイーツのスケール
    public Vector2 selectScale {get;private set;} = new Vector2(1.45f, 1.45f);

    // そのスイーツをなぞって選択できるか
    public bool canSelect {get;set;} = true;
    // 現在位置からこのスイーツを選択可能かどうか
    public bool canReach {get;set;} = true;
    // 現在このスイーツをつかんでいるか
    public bool isSelect {get;set;} = false;

    private bool isDesroying = false;

    [SerializeField] private SpriteRenderer _sprite;

    // Update is called once per frame
    void Update()
    {
        // デストロイ処理中は変化させない
        if(!isDesroying){
            if(!canReach && !isSelect){
                // spriteを暗くする
                _sprite.color = Color.grey;
            }else{
                _sprite.color = Color.white;
            }

            if(isSelect){
                transform.localScale = selectScale;
            }else{
                transform.localScale = defaultScale;
            }
        }
        
    }

    // 盤面上の指定の座標に移動する、破壊前用の処理
    public IEnumerator MovePosition(Vector3 tragetPos, float moveTime){
        isDesroying = true;
        Sequence delSeq = DOTween.Sequence();
        // 一瞬デカくして強調
        delSeq.Append(transform.DOScale(transform.localScale*1.5f, moveTime/4).SetLoops(2, LoopType.Yoyo).SetDelay(moveTime/4));
        delSeq.Join(transform.DOMove(tragetPos, moveTime).SetEase(Ease.OutCubic));
        
        yield return delSeq.WaitForCompletion();
        // スプライトを透明にする
        _sprite.color = new Color(0,0,0,0);
    }

    // スイーツを破壊する
    public IEnumerator DestroySweets(float delDuration){
        isDesroying = true;
        _sprite.DOFade(0, delDuration);
        
        yield return transform.DOMoveY(transform.position.y + 1f, delDuration).SetEase(Ease.InOutCirc).WaitForCompletion();
        Destroy(this.gameObject);
    }

    // 指定の距離だけ指定の時間をかけ落下させる
    public IEnumerator FallOwn(int distance, float unitDistance, float unitFallTime){
        // 現在のy座標の値から指定の距離だけ下の位置にトウィーン
        yield return transform.DOMoveY(transform.position.y - distance*unitDistance, unitFallTime*unitDistance).SetEase(Ease.InOutCirc).WaitForCompletion();
    }
}
