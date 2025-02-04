using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSweetsBuffer : MonoBehaviour
{
    [Header("何個アイテム消去履歴を保持するか")]
    [SerializeField] private int storageNum = 10;
    public List<SweetsEnum> deleteSweetsBuffer {get;private set;} = new List<SweetsEnum>();

    // 消去履歴に追加
    public void AddDeleteSweetsBuffer(SweetsEnum sweetsType){
        // 最新のものは先頭に
        deleteSweetsBuffer.Insert(0, sweetsType);
        if(deleteSweetsBuffer.Count >= storageNum){
            // 最新の10個を切り出す
            deleteSweetsBuffer = deleteSweetsBuffer.GetRange(0, storageNum);
        }
    }
}
