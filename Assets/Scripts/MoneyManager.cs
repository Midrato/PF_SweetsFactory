using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    // 得た総合のお金
    public int totalMoney {get;private set;} = 0;
    // 現在の所持金
    [field : SerializeField]public int nowMoney {get;private set;} = 0;

    void Awake(){
        totalMoney = 0;
        nowMoney = 0;
    }

    void Start(){
        // データの取得
        var data = DataManager.Inst.LoadPlayingData();
        totalMoney = data.totalMoney;
        nowMoney = data.nowMoney;
    }
    
    // 所持金を加算する
    public void AddMoney(int addMoney){
        nowMoney += addMoney;
        totalMoney += addMoney;
    }

    // 所持金を減算する 減らした結果が負になるなら失敗
    public bool SubMoney(int subMoney){
        if(nowMoney - subMoney < 0){
            return false;
        }else{
            nowMoney -= subMoney;
            return true;
        }
    }
}
