using GCSeries;
using liu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CMoudle : MonoBehaviour
{
    Transform leftBtnPar;
    public const int LeftToggleNum = 0;
    GameObject prefabAniTest;
    Animator amt;
    AnimatorEventComponent aec;
    TagLabelController tac;
    public float speed=0.1f;
    public Vector3 point;
    public float radius = 0.3f;
    //public System.Action growAction, distribAction;
    private void Awake()
    {
        Debug.Log("CMoudle");
    }
    // Use this for initialization
    void Start()
    {

        GlobalConfig.Instance.SetOperationModel(0);
        Image img= Tools.GetScenesObj("UI").transform.Find("InprojectionIgnoreCanvas/Left/Exit_ProjectionScreen/Rotate").GetComponent<Image>();
        img.raycastTarget = false;
        img.color = new Color32(60, 60, 60, 255);
        //ColorUtility.TryParseHtmlString("$$$2656"); ;

        simpleDrag = Tools.GetScenesObj("SimpleDrag").GetComponent<SimpleDrag>();

        GlobalEntity.GetInstance().RemoveAllListeners();
        Transform scenePar = Tools.GetScenesObj("Environment").transform;
        scenePar.Find("shengwu_weiguan").gameObject.SetActive(false);
        scenePar.Find("xuguan").gameObject.SetActive(true);

        //设置右边物体
        Transform left = Tools.GetScenesObj("UI").transform.Find("InprojectionIgnoreCanvas/Left");        

        prefabAniTest = ResManager.GetPrefab("Prefabs/feipaoAniTest");
        prefabAniTest.transform.SetParent(transform, false);
        tac = prefabAniTest.GetComponent<TagLabelController>();
        //加载Mesh       
        GameObject mesh = ResManager.GetPrefab("Prefabs/feipaoMesh");
        mesh.transform.SetParent(transform, false);
        //加载氧气
        GameObject feipaoO2 = ResManager.GetPrefab("Prefabs/feipaoO2");
        feipaoO2.transform.SetParent(transform, false);
        RandomMotionCtrl feipaoO2Ctrl =feipaoO2.CheckAddComponent<RandomMotionCtrl>();
        //加载CO2
        GameObject feipaoCO2 = ResManager.GetPrefab("Prefabs/feipaoCO2");
        feipaoCO2.transform.SetParent(transform, false);
        //显示标签
        PanelControl[] pctrl = tac.GetComponentsInChildren<PanelControl>();
        for (int i = 0; i < pctrl.Length; i++)
        {
            pctrl[i].offSetY =0.05f;
        }

        //Vector3[] pos = new Vector3[] { };
        //GameObject tag=null;
        //for (int i = 0; i < 2; i++)
        //{
        //    tag = ResManager.GetPrefab("Prefabs/CanvasTag");
        //}       
        tac.ShowTag(true);

        //显示隐藏标签
        GlobalEntity.GetInstance().AddListener<string>(MsgEnum.CLICK_TAG, (objName) => {
            Transform tagObj = transform.Find("feipaoAniTest/tag/" + objName);
            bool isShow = tagObj.gameObject.activeSelf;
            tagObj.gameObject.SetActive(!isShow);
            //Debug.Log(objName);
        });

        // sys= Tools.GetScenesObj("EventSystem").GetComponent<EventSystem>();
        //拖拽标签
        int num = 0;
        GlobalEntity.GetInstance().AddListener<GameObject>(MsgEnum.DRAG_TAG, (dragObj) =>
        {
            if (dragObj != null && dragObj.layer == LayerMask.NameToLayer("backEnvironment"))
            {
                RaycastHit raycastHit;
                GameObject hitObj = null;            
                RaycastHit? Hit = Raycast(out raycastHit, LayerMask.NameToLayer("tagEnvironment"));
                if (Hit!=null)
                {
                    hitObj = Hit.GetValueOrDefault().collider.gameObject;
                }
              

                if (hitObj != null && hitObj.name == dragObj.name.Split('_')[0])
                {
                    dragObj.gameObject.SetActive(false);
                    if (hitObj.name == "feipao")
                    {
                        hitObj.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "肺泡膜";                       
                    }
                    else
                    {
                        hitObj.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "毛细血管膜";
                    }
                    num++;
                    if (num==2)
                    {
                        prefabAniTest.transform.Find("Tip").gameObject.SetActive(false);
                    }
                    dragObj.gameObject.SetActive(false);
                    hitObj.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    dragObj.GetComponent<ObjItem>().InitState();
                }
            }
        });
        //抬起氧气
        GlobalEntity.GetInstance().AddListener<GameObject>(MsgEnum.CLICKUP_O2, (dragObj) =>
        {
            if (dragObj.layer==LayerMask.NameToLayer("O2"))//当前物体是氧气
            {
                RaycastHit rhit;
                RaycastHit? hit = Raycast(out rhit,LayerMask.NameToLayer("environment"));
                RandomMotion motion= dragObj.GetComponent<RandomMotion>();
                motion.isDraging = false;
                if (hit!=null)//在毛细血管范围
                {
                    GameObject hitObj = hit.GetValueOrDefault().collider.gameObject;
                    if (hitObj.name!= "maoxixuegaun"&&!hitObj.name.Contains("xuexibao"))
                    {
                        motion.SetBack();
                        return;
                    }
                    

                    point =hit.GetValueOrDefault().point;
                    //Collider[] _colliders= Physics.OverlapSphere(point, radius);
                    Transform posPar= prefabAniTest.transform.Find("f_10/pos");
                    int length= posPar.childCount;
                    Debug.Log(length);
                    if (length>0)
                    {
                        Transform o2 =hitObj.transform;
                        float minDis =1;
                        float _dis =1;
                        //Debug.LogError(length);
                        Transform xuexibao = null;
                        for (int i = 0; i < length; i++)
                        {                        
                            Transform hxb = posPar.GetChild(i);                         
                            if (hxb.gameObject.name.Contains("xuexibao")&& hxb.gameObject.activeSelf)
                            {
                                //if (xuexibao==null)
                                //{
                                //    xuexibao = hxb;
                                //}
                                if (Monitor23DMode.instance.is3D)
                                {
                                    _dis = Vector3.Distance(point, hxb.position);
                                }
                                else
                                {
                                    _dis = Vector2.Distance(point, hxb.position);
                                }
                                //Debug.Log(_dis + "   " + hxb);
                                if (minDis > _dis)
                                {                                    
                                    minDis = _dis;
                                    xuexibao = hxb;
                                }
                            }
                       }
                        //吸收氧气
                        dragObj.transform.SetParent(xuexibao);
                        dragObj.transform.localPosition = Vector3.zero;
                        dragObj.transform.DOScale(Vector3.zero, 0.3f).onComplete = () =>
                        {
                            motion.ResetMove();
                            MoveItem item= xuexibao.GetComponent<MoveItem>();
                            item.AbsorbO2();
                        };
                    }
                    else
                    {
                        //氧气返回
                        motion.SetBack();
                    }
                }
                else
                {
                    motion?.SetBack();
                }
            }
        });
        //按下氧气
        GlobalEntity.GetInstance().AddListener<GameObject>(MsgEnum.CLICKDOWN_O2, (dragObj) =>
        {
            if (dragObj.layer == LayerMask.NameToLayer("O2"))
            {
                //Debug.Log("O2");
                RandomMotion rm= dragObj.GetComponent<RandomMotion>();
                rm.SetPause();
            }
        });
        //抬起二氧化碳
        string followCo2 = string.Empty;
        GlobalEntity.GetInstance().AddListener<GameObject>(MsgEnum.CLICKUP_CO2, (dragObj) =>
        {
            if (dragObj.layer == LayerMask.NameToLayer("CO2"))//当前物体是co2
            {
                RaycastHit rhit;
             
                RaycastHit? hit = Raycast(out rhit,LayerMask.NameToLayer("environment"));
                //Debug.LogError(hit);
                RandomMotion motion =dragObj.GetComponent<RandomMotion>();
                if (hit != null)//在毛细血管范围
                {
                    GameObject hitObj = hit.GetValueOrDefault().collider.gameObject;
                    Debug.Log(hitObj.name);
                    if (hitObj.name == "maoxixuegaun"||hitObj.name.Contains("xuexibao"))
                    {
                    
                        motion.SetPause();
                        Transform origin= transform.Find("feipaoAniTest/f_10/pos/" + followCo2);//血管中的二氧化碳位置
                        //dragObj.transform.DOMove(origin.position, 0.3f).onComplete=()=> {

                        //};  
                        dragObj.gameObject.SetActive(false);//
                        origin.GetComponent<MeshRenderer>().enabled = true;
                        origin.GetComponent<BoxCollider>().enabled = true;
                    }
                    else if (hitObj.name== "qiu"||hitObj.name== "maoxixuegaun_middle")//二氧化碳进入
                    {                        
                        Transform co2Par = transform.Find("feipaoCO2");
                        dragObj.transform.SetParent(co2Par.transform, false);
                        dragObj.transform.localPosition = Vector3.zero;                        
                        motion.Move();
                    }
                }
                else
                {
                    //外界墙面
                    Transform origin = transform.Find("feipaoAniTest/f_10/pos/" + followCo2);//血管中的二氧化碳位置
                    dragObj.transform.DOMove(origin.position, 0.3f).onComplete = () => {
                        dragObj.gameObject.SetActive(false);//
                        origin.GetComponent<MeshRenderer>().enabled = true;
                        origin.GetComponent<BoxCollider>().enabled = true;
                    };
                }
            }
        });
       
        //按下二氧化碳
        GlobalEntity.GetInstance().AddListener<GameObject>(MsgEnum.CLICKDOWN_CO2, (dragObj) =>
        {
            if (dragObj.layer == LayerMask.NameToLayer("CO2"))
            {
                //Debug.Log(dragObj.name);

                dragObj.GetComponent<MeshRenderer>().enabled = false;//隐藏血管中的CO2
                dragObj.GetComponent<BoxCollider>().enabled = false;
                followCo2 = dragObj.name;

                Transform co2Par= transform.Find("feipaoCO2");
                Transform co2 = null;
                for (int i = 0; i < co2Par.childCount; i++)
                {
                    Transform temp = co2Par.GetChild(i);
                    if (!temp.gameObject.activeSelf)
                    {
                        co2 = temp;
                        break;
                    }
                }
                if (co2 == null)
                {
                    co2 = ResManager.GetPrefab("Prefabs/Co2").transform;
                    co2.SetParent(co2Par);
                    co2.gameObject.layer = LayerMask.NameToLayer("CO2");              
                   
                }
                simpleDrag._curDragObj = co2.gameObject;
                simpleDrag.dragObj = co2.gameObject;
                co2.gameObject.SetActive(false);

                co2.position = dragObj.transform.position;
                co2.gameObject.SetActive(true);
                
               

                RandomMotion rm = co2.gameObject.CheckAddComponent<RandomMotion>();
                rm.SetPause();

            }
        });
    }

    SimpleDrag simpleDrag;
    private RaycastHit? Raycast(out RaycastHit raycastHit,LayerMask layer)
    {
        if (simpleDrag==null)
        {
              simpleDrag = Tools.GetScenesObj("SimpleDrag").GetComponent<SimpleDrag>();
        }
        Physics.queriesHitBackfaces = true;
        if (Monitor23DMode.instance.is3D)//3d状态下
        {
            if(Physics.Raycast(FCore.penRay, out raycastHit, Mathf.Infinity, 1 <<layer /*LayerMask.NameToLayer("environment")*/))
            {
                //Debug.LogError(raycastHit);
                return raycastHit;
            }
        }
        else
        {           
            Ray ray = Monitor23DMode.instance.camera2D.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1<<layer/*1 << LayerMask.NameToLayer("environment")*/))
            {
                return raycastHit;
            }      
        }
        Physics.queriesHitBackfaces = false;
        return null;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            amt.speed = speed;
        }
    }

    /// <summary>
    /// 人口增长
    /// </summary>
    void OnC1Click(bool isOn)
    {
        if (isOn)
        {
            //人口增长
            GameObject go = ResManager.GetPrefab("Prefabs/C1Moudle");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);

            OnPopuGrowthClickCallback();
        }
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
    void OnC2Click(bool isOn)
    {
        if (isOn)
        {
            //人口分布
            GameObject pg = ResManager.GetPrefab("Prefabs/C2Moudle");
            pg.transform.SetParent(transform);
            pg.transform.localScale = Vector3.one;
            pg.transform.localPosition = Vector3.zero;
            pg.transform.localRotation = Quaternion.Euler(Vector3.zero);
            OnPopuDistributionClickCallback();
        }
    }
    /// <summary>
    /// 人口分布点击回调
    /// </summary>
    void OnPopuDistributionClickCallback()
    {

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

        Image img = Tools.GetScenesObj("UI").transform.Find("InprojectionIgnoreCanvas/Left/Exit_ProjectionScreen/Rotate").GetComponent<Image>();
        img.raycastTarget = true;
        img.color = new Color32(255, 255, 255, 255);
    }    
    private void OnDestroy()
    {
     
    }


    private void OnDrawGizmos1()
    {
        Gizmos.DrawSphere(point, radius);
        Gizmos.color = Color.blue;
    }
}

