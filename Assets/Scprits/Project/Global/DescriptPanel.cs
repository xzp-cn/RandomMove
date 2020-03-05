using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Threading;

/// <summary>
/// 描述面板
/// </summary>
public class DescriptPanel : MonoBehaviour
{

    public static DescriptPanel Instance;
    public Text contentText,titleText;

    //public GameObject play, stop;
    /// <summary>
    /// 向左移动和向右移动按钮
    /// </summary>
    public Button leftBtn, rightBtn;

    /// <summary>
    /// panel 下面的child子节点
    /// </summary>
    Transform child;
    private void Awake()
    {
        Instance = this;
        child = transform.Find("Child");
        leftBtn.onClick.AddListener(LeftBtnHandle);
        rightBtn.onClick.AddListener(RightBtnHandle);
        leftBtn.gameObject.SetActive(false);
    }

    private void RightBtnHandle()
    {
        rightBtn.gameObject.SetActive(false);
        child.DOLocalMove(new Vector3(460, 0, 0), 1f).OnComplete(() => { leftBtn.gameObject.SetActive(true); });
    }

    private void LeftBtnHandle()
    {
        leftBtn.gameObject.SetActive(false);
        child.DOLocalMove(Vector3.zero, 1f).OnComplete(() => { rightBtn.gameObject.SetActive(true); });
    }

    /// <summary>
    /// 打开面板展示文字
    /// </summary>
    public void DesText()
    {
        if (!string.IsNullOrEmpty(contentText.text))
        {
            Branch branch = FindObjectOfType<Branch>();
            if (branch)
            {
                if (DesAndAudio.Instance.dess.Length >= branch.branchIndex && branch.branchIndex > 0)
                {
                    //获取使用文本的数据
                    string des = DesAndAudio.Instance.dess[branch.branchIndex - 1];
                    string titleName = DesAndAudio.Instance.desTitles[branch.branchIndex - 1];
                    if (!string.IsNullOrEmpty(titleName))
                    {
                        titleText.text = titleName;
                    }
                    else
                    {
                        F3DDebug.Log("没有指定描述", new System.Diagnostics.StackTrace());
                    }

                    if (!string.IsNullOrEmpty(des))
                    {
                        //ScreenMask.Instance.ShowMask();
                        //branch.desContent;
                        string desText = "\u3000\u3000" + des;
                        contentText.text = "";
                        contentText.text = desText;
                        //contentText.DOText(desText, 1f).OnComplete(()=> 
                        //{
                        //    ScreenMask.Instance.HideMask();
                        //});
                        //play.SetActive(true);
                        //stop.SetActive(false);

                        //audioSource.Play();
                    }
                    else
                    {
                        F3DDebug.Log("没有指定描述", new System.Diagnostics.StackTrace());
                    }
                }
            }
            else
            {
                F3DDebug.Log("当前节点没有继承Branch", new System.Diagnostics.StackTrace());
            }
        }

        child.localScale = Vector3.one;
    }

    public void ClosePanel()
    {
        child.localScale = Vector3.zero;
    }

    public void OnDestoryCall()
    {
        //if (child && contentText)
        //{
            //child.localScale = Vector3.zero;
            //contentText.text = "123";
        //}
    }
}
