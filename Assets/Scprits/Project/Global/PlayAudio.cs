using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 声音的播放
/// </summary>
public class PlayAudio : MonoBehaviour
{

    public static PlayAudio Instance;

    AudioSource audioSource;

    public Button playBtn, stopBtn;

    bool playing = true;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        playBtn.onClick.AddListener(PlayBtnHandle);
        stopBtn.onClick.AddListener(StopBtnHandle);
    }

    private void StopBtnHandle()
    {
        playing = true;
        PlayAudioVido();
        playBtn.gameObject.SetActive(true);
        stopBtn.gameObject.SetActive(false);
    }

    private void PlayBtnHandle()
    {
        playing = false;
        StopPlayAudio();
        playBtn.gameObject.SetActive(false);
        stopBtn.gameObject.SetActive(true);
    }

    public void PlayAudioVido()
    {
        if (playing)
        {
            //if (audioSource.clip == null)
            {
                Branch branch = FindObjectOfType<Branch>();
                if (branch)
                {
                    if (DesAndAudio.Instance.dess.Length >= branch.branchIndex && branch.branchIndex > 0)
                    {
                        AudioClip audioClip = DesAndAudio.Instance.audioClips[branch.branchIndex - 1];
                        if (audioClip)
                        {
                            audioSource.clip = audioClip;
                        }
                        else
                        {
                            F3DDebug.Log("没有指定音频文件", new System.Diagnostics.StackTrace());
                        }
                    }
                }
                else
                {
                    F3DDebug.Log("当前节点没有继承Branch", new System.Diagnostics.StackTrace());
                }
            }
            audioSource.Play();
        }
    }

    public void StopPlayAudio()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// 销毁Branch的回调
    /// </summary>
    public void OnDestoryCall()
    {
        //if (audioSource)
        //{
        //    audioSource.Stop();
        //    audioSource.clip = null;
        //}
    }
}
