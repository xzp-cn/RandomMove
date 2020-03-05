using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GCSeries;

/// <summary>
/// 分支行为
/// </summary>
public class Branch : MonoBehaviour {

    /// <summary>
    /// 当前分支节点，根据节点去拿描述和音频
    /// </summary>
    public int branchIndex;

    MRSystem mRSystem;

    //public Vector3 mRInitPos = Vector3.zero;
    //public Vector3 mrInitEuler = Vector3.zero;
    //public float viewScale = 1;
    public void Start()
    {
        mRSystem = FindObjectOfType<MRSystem>();

        //mRSystem.transform.position = mRInitPos;
        //mRSystem.transform.eulerAngles = mrInitEuler;
        //mRSystem.ViewerScale = viewScale;
        mRSystem.isAutoSlant = false;

        //播放音频
        PlayAudio.Instance.PlayAudioVido();
        //显示文字
        DescriptPanel.Instance.DesText();
    }

    private void OnDestroy()
    {
        DescriptPanel.Instance.OnDestoryCall();
        PlayAudio.Instance.OnDestoryCall();
    }
}
