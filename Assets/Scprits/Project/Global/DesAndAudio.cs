using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesAndAudio : MonoBehaviour
{
    public static DesAndAudio Instance;
    public List<AudioClip> audioClips;

    public TextAsset desTextAsset;
    public TextAsset desTitleTextAsset;

    public string[] desTitles;
    public string[] dess;

    private void Awake()
    {
        Instance = this;
        desTitles = desTitleTextAsset.text.Replace("\r\n", "\n").Split('\n'); 
        dess = desTextAsset.text.Replace("\r\n", "\n").Split('\n');
    }

}
