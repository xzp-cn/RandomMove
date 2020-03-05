using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ToggleControl : MonoBehaviour
{
    //底部按钮
    public GameObject BottomToggleGroup;

    //当前点击的Toggle
    private Toggle currentToggle;

    public Transform  LeftToggle;//左侧按钮组合
    private  GameObject  Btn;//左侧按钮

    string leftToggle1 = "Part1";//左侧按钮组合名称
    string leftToggle2 = "Part2";
    string leftToggle3 = "Part3";

    string Fei = "Fei";
    string ZhiQiGuan = "ZhiQiGuan";
    string FeiPao = "FeiPao";
    string QiTiJiaoHuanModel = "QiTiJiaoHuan";

    public GameObject LungZhiqiguanBackgroud;//肺，支气管界面背景
    public GameObject FeipaoBackgroud;//肺泡界面背景

    /// <summary>
    /// 模型生成的挂载节点
    /// </summary>
    public Transform modelsRoot;

    private string text;
    void Start()
    {

        //设置背景
        BackgroudControl("肺");

        if (modelsRoot == null)
            modelsRoot = GameObject.Find("ModelsRoot").transform;
        if (modelsRoot.childCount > 0)
        {
            DeleteModelsRootChild();
        }

        if (LeftToggle == null)
            LeftToggle = GameObject.Find("UI/InprojectionScanCanvas/LeftUIButton/leftToggle").transform;
        if (LeftToggle.childCount > 0)
        {
            ClearBtns();
        }


        //加载按钮和模型
        LoadBtns(leftToggle1);
        JudgementModdle(Fei);

        //给下方的所有Toggle添加监听事件
        Toggle[] bottomToggle = BottomToggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < bottomToggle.Length; i++)
        {
            bottomToggle[i].onValueChanged.AddListener(ShowLeftToggle);
        }
        GameObject models = GameObject.Find("Models");

    }

    /// <summary>
    /// 清空场景的模型
    /// </summary>
    void DeleteModelsRootChild()
    {
        int chilCount = modelsRoot.childCount;
        for (int i = 0; i < chilCount; i++)
        {
            DestroyImmediate(modelsRoot.GetChild(0).gameObject);
        }
    }

    //根据下方不同的Toggle显示左侧相应的按钮
    void ShowLeftToggle(bool ison)
    {
        if (ison)
        {
            //当前点击的Toggle
            currentToggle = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
            //当前点击的Toggle的文字
            text = currentToggle.GetComponentInChildren<Text>().text.ToString();
            Debug.Log("ToggleControl.ShowLeftToggle().text" + text);
            switch (text)
            {
                case "肺":
                    BackgroudControl(text);
                    FeipaoBackgroud.SetActive(false);
                    LoadBtns(leftToggle1);
                    JudgementModdle(Fei);
                    break;
                case "支气管":
                    BackgroudControl(text);
                    JudgementModdle(ZhiQiGuan);
                    ClearBtns();
                    break;
                case "肺泡":
                    BackgroudControl(text);
                    LoadBtns(leftToggle2);
                    JudgementModdle(FeiPao);
                    break;
                case "气体交换":
                    BackgroudControl(text);
                    LoadBtns(leftToggle3);
                    qitiObj = JudgementModdle(QiTiJiaoHuanModel);
                    break;
            }
        }
    }

    GameObject qitiObj;
    /// <summary>
    /// 恢复材质
    /// </summary>

    public void RecoverMaterial()
    {
        Debug.Log("ToggleControl.recoverMaterial");
        if(qitiObj==null)
        {
            Debug.Log("null");
            Debug.LogError("RecoverMaterial() qitiObj为空");
            return;
        }
        JiaoHu[] jiaoHus = qitiObj.GetComponentsInChildren<JiaoHu>();
        foreach (var item in jiaoHus)
        {
            Debug.Log("jiaohu");
            item.RecoverMaterial();
        }
    }

    //控制左侧按钮的隐藏于显示
    void JudgementToggle(GameObject show, GameObject hide, GameObject hide2)
    {
        show.SetActive(true);
        hide.SetActive(false);
        hide2.SetActive(false);
    }
    //控制相应界面模型是否显示
    GameObject JudgementModdle(string show)
    {
        DeleteModelsRootChild();
        string path = "Prefabs/" + show;
        Object modelPrefab = Resources.Load(path, typeof(GameObject));
        GameObject obj = Instantiate(modelPrefab) as GameObject;
        obj.transform.parent = modelsRoot;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localEulerAngles = Vector3.zero;
        //重新加载肺的时候，按钮为拆分
        if (show == Fei)
        {
            GameObject.Find("UI/InprojectionScanCanvas/LeftUIButton/leftToggle/Part1(Clone)/Part1/Content/SplitCombine/Text").GetComponent<Text>().text = "拆分";
        }
        return obj;
    }//"UI/InprojectionScanCanvas/LeftUIButton/leftToggle/Part3(Clone)/Part3/Content"
    /// <summary>
    /// 背景控制
    /// </summary>
    void BackgroudControl(string name)
    {
        switch (name)
        {
            case "肺":
                LungZhiqiguanBackgroud.SetActive(true);
                FeipaoBackgroud.SetActive(false);
                break;
            case "支气管":
                LungZhiqiguanBackgroud.SetActive(true);
                FeipaoBackgroud.SetActive(false);
                break;
            case "肺泡":
                LungZhiqiguanBackgroud.SetActive(false);
                FeipaoBackgroud.SetActive(true);
                break;
            case "气体交换":
                LungZhiqiguanBackgroud.SetActive(false);
                FeipaoBackgroud.SetActive(false);
                break;
        }

    }
    /// <summary>
    /// 清空按钮
    /// </summary>
    void ClearBtns()
    {
        Debug.Log(LeftToggle.childCount);
        for (int i = 0; i < LeftToggle.childCount; i++)
        {
            Destroy(LeftToggle.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 加载按钮
    /// </summary>
    /// <param name="btn"></param>
    void LoadBtns(string btn)
    {
        ClearBtns();
        string btnPath = "Prefabs/" + btn;
        Object btnPrefab = Resources.Load(btnPath, typeof(GameObject));
        Btn = Instantiate(btnPrefab) as GameObject;
        Btn.transform.SetParent(LeftToggle);
        Btn.transform.localPosition = Vector3.zero;
        Btn.transform.localScale = Vector3.one;
        Btn.transform.localEulerAngles = Vector3.zero;
    }
}
