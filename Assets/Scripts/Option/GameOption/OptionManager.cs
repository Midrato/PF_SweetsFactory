using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{   
    // 設定調整用のスライダー
    [SerializeField] private Slider[] optionVolumes = new Slider[2];
    // 現在の設定表示用のテキスト
    [SerializeField] private TextMeshProUGUI[] optionVolTexts = new TextMeshProUGUI[2];
    // 変更を適用する用ボタン
    [SerializeField] private Button applyButton;
    // 変更キャンセル用ボタン
    [SerializeField] private List<Button> cancelButton = new List<Button>();
    
    // オプション適用のためのデータ
    private OptionData optionData;

    [SerializeField] private OptContentsWindow optionWindow;

    // 音量チェンジ時のse管理用
    //private Coroutine seCoroutine = null;
    // seを鳴らすインターバル
    private float seInterVal = 0.3f;
    private bool canPlaySe = true;

    void OnEnable()
    {
        optionData = DataManager.Inst.LoadOptionData();

        // スライドの値の初期設定
        optionVolumes[0].value = optionData.bgmVolume;
        optionVolumes[1].value = optionData.seVolume;
        optionVolumes[2].value = optionData.sweetsDelSpeed;

        UpdateContents();
    }

    void Start(){
        applyButton.onClick.AddListener(ApplyOption);
        foreach(var button in cancelButton){
            button.onClick.AddListener(CancelOption);
        }

        // 値のアップデート処理を仕込む
        foreach(var volume in optionVolumes){
            volume.onValueChanged.AddListener((float tmp) => UpdateContents());
            volume.onValueChanged.AddListener((float tmp) => StartCoroutine(PlayValueChangeSE()));
        }
    }

    void Update(){
        foreach(var _slider in optionVolumes){
            _slider.interactable = optionWindow.isCanControlOption;
        }
    }

    private void UpdateContents(){
        optionData.bgmVolume = optionVolumes[0].value;
        optionData.seVolume = optionVolumes[1].value;
        optionData.sweetsDelSpeed = optionVolumes[2].value;
        SetVolumeText();
        SoundManager.I.SetVolume(optionVolumes[0].value, optionVolumes[1].value);
    }

    private void ApplyOption(){
        DataManager.Inst.SaveOptionData(optionData);
        // サウンドマネージャーの音量を更新
        SoundManager.I.ApplyVolumeOption();
    }

    private void CancelOption(){
        // 音量を元に戻す
        SoundManager.I.ApplyVolumeOption();
    }

    private void SetVolumeText(){
        // 音量...0~100までに変換
        optionVolTexts[0].text = ((int)(optionVolumes[0].value * 100)).ToString();
        optionVolTexts[1].text = ((int)(optionVolumes[1].value * 100)).ToString();
        // 連鎖アニメーション速度...少数第2位切り捨て
        optionVolTexts[2].text = string.Format("{0:0.00}", optionVolumes[2].value);
    }

    private IEnumerator PlayValueChangeSE(){
        if(canPlaySe == true){
            canPlaySe = false;
            SoundManager.I.PlaySE("sweetsNazori");
            yield return new WaitForSeconds(seInterVal);
            canPlaySe = true;
        }
    }

}
