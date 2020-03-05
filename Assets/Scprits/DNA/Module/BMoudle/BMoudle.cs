using DG.Tweening;
using GCSeries;
using UnityEngine;
using UnityEngine.UI;

public class BMoudle : MonoBehaviour
{
    Transform leftBtnPar;
    public const int LeftToggleNum = 2;

    TagLabelController tac;
    Transform feipaoAni;//气体交换动画
    // Use this for initialization
    Vector3 feipaoAniInitPos;
    public Material[] mats;
    void Start()
    {
        GlobalEntity.GetInstance().RemoveAllListeners();

        Transform scenePar = Tools.GetScenesObj("Environment").transform;
        scenePar.Find("shengwu_weiguan").gameObject.SetActive(false);
        scenePar.Find("xuguan").gameObject.SetActive(true);

        feipaoAni = ResManager.GetPrefab("Prefabs/feipaoAni").transform;
        feipaoAni.SetParent(transform, false);
        feipaoAniInitPos = feipaoAni.localPosition;
        tac = feipaoAni.GetComponent<TagLabelController>();
        AnimatorEventComponent aec = feipaoAni.Find("f_10").gameObject.CheckAddComponent<AnimatorEventComponent>();
        aec.PlayForward("Take 001");

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

        string[] textStrs = new string[] { "显示标签", "剖面", "测试" };
        UnityEngine.Events.UnityAction<bool>[] deleArray = new UnityEngine.Events.UnityAction<bool>[] { OnB1Click, OnB2Click, OnB3Click };
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

        PanelControl[] pctrl = tac.GetComponentsInChildren<PanelControl>();
        for (int i = 0; i < pctrl.Length; i++)
        {
            pctrl[i].offSetY = 0.027f;
        }
        tac.ShowTag(showTag);



    }

    public void RecoverMaterial()
    {       
        JiaoHu[] jiaoHus = transform.GetComponentsInChildren<JiaoHu>();
        foreach (var item in jiaoHus)
        {
            //Debug.Log("jiaohu");
            item.RecoverMaterial();
        }
    }

    /// <summary>
    /// 控制血管透明度
    /// </summary>
    void AlphaMask(bool isApha)
    {
    }

    bool showTag = false;
    /// <summary>
    /// 人口增长
    /// </summary>
    void OnB1Click(bool isOn)
    {
        Debug.Log("OnB1Click");
        Toggle tg = leftBtnPar.transform.Find("left/Toggle0").GetComponent<Toggle>();     
        Text label = tg.transform.GetComponentInChildren<Text>();
        showTag = !showTag;
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
    /// 按钮点击回调
    /// </summary>
    void OnPopuGrowthClickCallback()
    {

    }
    /// <summary>
    /// 人口分布
    /// </summary>
    void OnB2Click(bool isOn)
    {
        if (isOn)
        {
            // OnPopuDistributionClickCallback();
            //人口分布
            //GameObject pg = ResManager.GetPrefab("Prefabs/B2Moudle");
            //pg.transform.SetParent(transform);
            //pg.transform.localScale = Vector3.one;
            //pg.transform.localPosition = Vector3.zero;
            //pg.transform.localRotation = Quaternion.Euler(Vector3.zero);
            Transform meshHide = feipaoAni.Find("meshHide");
            MeshRenderer[] meshRenders = meshHide.GetComponentsInChildren<MeshRenderer>();
            meshRenders[0].material = mats[1];
            meshRenders[1].material = mats[3];

            //MeshRenderer[] meshRenders = meshHide.GetComponentsInChildren<MeshRenderer>();
            meshRenders[0].material.color = new Color(1, 1, 1, 1);
            meshRenders[1].material.color = new Color(1, 1, 1, 1);

            float time = 0;
            DOTween.To(
            () => {
                time = 0;
                return 0; },//第一个执行方法
            (a) =>
            {
                time += Time.deltaTime;
                meshRenders[0].material.color = new Color(1, 1, 1, 1-time);
                meshRenders[1].material.color = new Color(1, 1, 1, 1-time);
            },//update
            0,
            1).onComplete = () =>
            {
            };
        }
        else
        {
            Transform meshHide = feipaoAni.Find("meshHide");
            MeshRenderer[] meshRenders = meshHide.GetComponentsInChildren<MeshRenderer>();
            meshRenders[0].material = mats[0];
            meshRenders[1].material = mats[2];
        }
    }
    /// <summary>
    /// 人口分布点击回调
    /// </summary>
    void OnPopuDistributionClickCallback()
    {
        //Transform meshHide = feipaoAni.Find("meshHide");
        //MeshRenderer[] meshRenders = meshHide.GetComponentsInChildren<MeshRenderer>();
        //meshRenders[0].material = mats[0];
        //meshRenders[1].material = mats[3];
    }

    /// <summary>
    /// 人口分布
    /// </summary>
    void OnB3Click(bool isOn)
    {
        if (isOn)
        {
            //OnB3ClickCallback();
            ////人口分布
            //GameObject pg = ResManager.GetPrefab("Prefabs/B3Moudle");
            //pg.transform.SetParent(transform);
            //pg.transform.localScale = Vector3.one;
            //pg.transform.localPosition = Vector3.zero;
            //pg.transform.localRotation = Quaternion.Euler(Vector3.zero);

        }
    }
    /// <summary>
    /// 人口分布点击回调
    /// </summary>
    void OnB3ClickCallback()
    {
        if (this.transform.childCount != 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }


    /// <summary>
    ///销毁
    /// </summary>
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
    private void OnDestroy()
    {
   
    }
}
