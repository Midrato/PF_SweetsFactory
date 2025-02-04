using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    // あらゆるオブジェクトから呼び出し得るのでシングルトンに
    public static SoundManager I; 

    // BGM, SEの大きさの最大値
    [SerializeField] private float maxBgmVolume = 1;
    [SerializeField] private float maxSeVolume = 1;
    
    // BGM, SEの音声ファイルを収めるところ
    [field : SerializeField] public SoundData BGMClips {get;private set;}
    [field : SerializeField] public SoundData SEClips {get;private set;}

    // BGM再生用
    private AudioSource BGMSource;
    // 効果音再生用
    private AudioSource SESource;

    void Awake()
    {
        // シングルトン処理
        if(I == null){
            I = this;
            DontDestroyOnLoad(this.gameObject);
            var audioSources = GetComponents<AudioSource>();
            BGMSource = audioSources[0];
            SESource = audioSources[1];
        }else{
            Destroy(this.gameObject);
        }
    }

    void Start(){
        ApplyVolumeOption();
    }

    /// <summary>
    /// BGM名からBGMを再生する
    /// </summary>
    /// <param name="BGMName"></param>
    public void PlayBGM(string BGMName){
        foreach(var bgmClip in BGMClips.seEntries){
            // サウンド名が一致したら再生
            if(bgmClip.name == BGMName){
                Debug.Log("流したよ～ : " + BGMName);
                BGMSource.clip = bgmClip.clip;
                BGMSource.Play();
                break;
            }
        }
    }

    /// <summary>
    /// SE名からSEを再生する
    /// </summary>
    /// <param name="SEName"></param>
    public void PlaySE(string SEName){
        foreach(var seClip in SEClips.seEntries){
            // サウンド名が一致したら再生
            if(seClip.name == SEName){
                SESource.PlayOneShot(seClip.clip);
                break;
            }
        }
    }

    /// <summary>
    /// BGMを停止させる
    /// </summary>
    public void StopBGM(){
        BGMSource.Stop();
    }

    /// <summary>
    /// BGMをフェードアウトさせる
    /// </summary>
    /// <param name="duration"></param>
    public Tween DOFadeOutBGM(float duration){
        float beforeVolume = BGMSource.volume;
        Tween tween = DOTween.To(() => beforeVolume, (val) => {
            BGMSource.volume = val;
        }, 0, duration);

        tween.OnKill(() => {
            BGMSource.Stop();
            ApplyVolumeOption();
        });

        return tween;
    }
    
    public void SetVolume(float bgmVol, float seVol){
        BGMSource.volume = maxBgmVolume * bgmVol;
        SESource.volume = maxSeVolume * seVol;
    }

    // BGM,SEの音量設定を適用する
    public void ApplyVolumeOption(){
        var data = DataManager.Inst.LoadOptionData();
        
        BGMSource.volume = maxBgmVolume * data.bgmVolume;
        SESource.volume = maxSeVolume * data.seVolume;
    }
}
