using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpacer : MonoBehaviour
{
    // パッシブのスクロールバーのコンテンツ範囲
    [SerializeField] protected RectTransform contentsTrans;

    [Space]
    // ボタン配置：各行のボタン数
    [SerializeField] private int objectsNumInRow = 4;
    // ボタン設置：初期位置のずれ
    [SerializeField] private Vector2 firstPos = new Vector2(130, 130);
    // ボタン設置：ボタン同士の距離
    [SerializeField] private int objectsDistance = 246;
    // コンテンツ範囲：1列毎の縦幅
    [SerializeField] private int contentsPerHeight = 250;

    void OnEnable()
    {
        // パッシブボタンの生成
        var objectsTrans = InstObjects();
        // パッシブボタン位置の整理
        OrganizePassiveButtons(objectsTrans);
    }
    
    /// <summary>
    /// オブジェクトを生成し、生成したオブジェクト達のトランスフォームのリストを返す
    /// </summary>
    /// <returns></returns>
    protected virtual List<RectTransform> InstObjects(){
        return new List<RectTransform>();
    }

    // オブジェクトの位置を整理する
    private void OrganizePassiveButtons(List<RectTransform> objectsTrans){
        // 行数
        int rowsNum = 1 + (objectsTrans.Count-1) / objectsNumInRow;
        // (計算の兼ね合いで1行より小さくなったら1行にする)
        rowsNum = (rowsNum < 1) ? 1 : rowsNum;

        for(int i=0;i<objectsTrans.Count;i++){
            // このオブジェクトの列、行を取得
            int _row = i / objectsNumInRow;
            int _column = i % objectsNumInRow;

            // 初期位置 + オブジェクトの列×オブジェクト間距離, 初期位置 + オブジェクトの行×オブジェクト間距離とする
            objectsTrans[i].anchoredPosition = new Vector2(firstPos.x + _column * objectsDistance, -(firstPos.y + _row * objectsDistance));
        }

        // コンテンツの高さを設定
        contentsTrans.sizeDelta = rowsNum * contentsPerHeight * Vector2.up;
    }
}
