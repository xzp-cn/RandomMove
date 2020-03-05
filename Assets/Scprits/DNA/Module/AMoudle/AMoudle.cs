using GCSeries;
using UnityEngine;
using UnityEngine.UI;

public class AMoudle : MonoBehaviour
{
    Transform leftBtnPar;
    public const int LeftToggleNum = 2;
    SpliteControl spc;
    TagLabelController tac;
    //public System.Action growAction, distribAction;
    Vector3 lungTagCombinePos = new Vector3(0.01f, -0.326f, 0.32f);
    Vector3 lungTagSplitePos = new Vector3(-0.087f, -0.287f, 0.656f);
    GameObject bodyTag;
    private void Awake()
    {
        Debug.Log("AMoudle");
    }
    // Use this for initialization
    void Start()
    {
        GlobalEntity.GetInstance().RemoveAllListeners();
        //      
        //设置右边物体
        Transform left = Tools.GetScenesObj("UI").transform.Find("InprojectionIgnoreCanvas/Left");
        //leftBtnPar = left.Find("leftBtnPar");
        if (leftBtnPar == null)
        {
            leftBtnPar = new GameObject("leftBtnPar").transform;
            leftBtnPar.SetParent(left, false);
            leftBtnPar.localPosition = new Vector3(29f, 0, 0);
            leftBtnPar.localScale = Vector3.one;
        }
        //加载左侧按钮
        GameObject popu = ResManager.GetPrefab("Prefabs/leftBtn");
        popu.name = "left";
        popu.transform.SetParent(leftBtnPar);
        popu.transform.localPosition = Vector3.zero;
        popu.transform.localScale = Vector3.one;
        popu.transform.localRotation = Quaternion.Euler(Vector3.zero);

        string[] textStrs = new string[] { "拆分", "显示标签" };
        UnityEngine.Events.UnityAction<bool>[] deleArray = new UnityEngine.Events.UnityAction<bool>[] { OnA1Click, OnA2Click };
        for (int i = 0; i < LeftToggleNum; i++)
        {
            GameObject toggle = ResManager.GetPrefab("Prefabs/Toggle");
            toggle.transform.SetParent(popu.transform);
            toggle.transform.localPosition = Vector3.zero;
            toggle.transform.localScale = Vector3.one;
            toggle.transform.localRotation = Quaternion.Euler(Vector3.zero);
            toggle.GetComponentInChildren<Text>().text = textStrs[i];
            Toggle tg = toggle.GetComponentInChildren<Toggle>();
            tg.onValueChanged.RemoveAllListeners();
            tg.onValueChanged.AddListener(deleArray[i]);

            toggle.name = "Toggle" + i.ToString();
        }

        //
        bodyTag = ResManager.GetPrefab("Prefabs/lungTag");
        bodyTag.transform.SetParent(transform, false);
        bodyTag.transform.localPosition = lungTagCombinePos;

        spc = popu.transform.GetChild(0).gameObject.AddComponent<SpliteControl>();
        spc.spliteObj = bodyTag.transform.Find("F_fei_wz_1226");


        tac= spc.spliteObj.GetComponentInParent<TagLabelController>();

        float[] offsetYs = new float[] {-0.03f,-0.03f,0.03f, 0.04f,-0.03f, -0.032f, 0.032f, -0.032f};
        PanelControl[] pctrl=tac.GetComponentsInChildren<PanelControl>();
        for (int i = 0; i < pctrl.Length; i++)
        {
            pctrl[i].offSetY = offsetYs[i];
        }
        tac.ShowTag(showTag);

        GlobalEntity.GetInstance().AddListener<string>(MsgEnum.CLICK_TAG, (objName) =>
        {                   
            tac.ShowTag("tag_" + objName.Split('_')[1]);
            Debug.Log(objName);
        });


    }

    /// <summary>
    /// 拆分
    /// </summary>
    void OnA1Click(bool isOn)
    {
        Toggle tg = leftBtnPar.transform.Find("left/Toggle0").GetComponent<Toggle>();      
        spc.OnClickSpliteToggle();      
        bool isSplite = spc.curSpliteState == SpliteControl.SpliteState.Splite;
        Vector3 pos = isSplite? lungTagSplitePos : lungTagCombinePos;
        bodyTag.transform.localPosition = pos;
        tg.isOn = isSplite;
    }
    void BoxCtrl(bool isSplite)
    {
        BoxCollider bcs = tac.GetComponentInParent<BoxCollider>();
        //for (int i = 0; i < bcs.Length; i++)
        //{
        //    bcs[i].enabled = isSplite;
        //}
       bcs .enabled = !isSplite;
    }
    /// <summary>
    /// 按钮点击回调
    /// </summary>
    void OnA1MoudleClickCallback()
    {
        if (this.transform.childCount != 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }
    bool showTag = false;
    /// <summary>
    /// 标签
    /// </summary>
    void OnA2Click(bool isOn)
    {
        Toggle tg = leftBtnPar.transform.Find("left/Toggle1").GetComponent<Toggle>();      
        showTag = !showTag;
        Text label = tg.transform.GetComponentInChildren<Text>();
        if (showTag)
        {
            label.text = "隐藏标签";
        }
        else
        {
            label.text = "显示标签";
        }
        tac.ShowTag(showTag);
        tg.isOn = showTag;
    }
    /// <summary>
    /// 人口分布点击回调
    /// </summary>
    void OnA2MoudleClickCallback()
    {
        if (this.transform.childCount != 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }

    public void Dispose()
    {
        if (leftBtnPar != null)
        {
            Destroy(leftBtnPar.gameObject);
        }
        if (this.transform.childCount != 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
        FCore.clearDragObj();

    }

    /// <summary>
    ///销毁
    /// </summary>    
    private void OnDestroy()
    {

    }
}
