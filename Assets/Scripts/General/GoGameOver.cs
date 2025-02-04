using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGameOver : SceneChangeBase
{
    [SerializeField] private SceneEffectPanel scenePanel;

    /// <summary>
    /// ゲームオーバーシーンへ遷移する関数
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChangeToFailScene(){
        scenePanel.gameObject.SetActive(true);
        yield return StartCoroutine(scenePanel.ShutterClosing());
        // この回の記録を残し、記録消去
        MainSceneDataManager.MCInst.SaveNowAccumulativeData(true);
        MainSceneDataManager.MCInst.ClearData();
        ChangeScene();
    }
}
