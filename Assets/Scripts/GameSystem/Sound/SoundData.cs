using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 音楽ファイルを名前で管理したもの
[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObject/SoundData")]
public class SoundData : ScriptableObject
{
    [Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip clip;
    }

    public SoundEntry[] seEntries;
}
