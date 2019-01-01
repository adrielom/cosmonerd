using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingAudioSorce : MonoBehaviour
{

    public string songName;
    void Start()
    {
        print("hey");
        AssetBundle aBAudio = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundles/" + "songs");
        var clip = aBAudio.LoadAsset<AudioClip>(songName);
        print(clip);
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
