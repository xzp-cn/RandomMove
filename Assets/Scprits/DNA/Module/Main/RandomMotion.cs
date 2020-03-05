using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using liu;
using GCSeries;

public class RandomMotion : MonoBehaviour {
    [SerializeField]
    float rangeX=0.4f;
    [SerializeField]
    float rangeY = 0.5f;
    [SerializeField]
    float rangeZ= 0.5f;    
    Tween tween;
    bool isComplete = false;
    Vector3 lastPos;
    Vector3 backPos;
    Quaternion rotation;
    Vector3 scale;
    Vector3 originalPos;
    Transform originPar;
    float z;
    // Use this for initialization
    private void Awake()
    {
        isDraging = false;
        isComplete = true;

    }
    private void OnEnable()
    {
        originalPos = transform.position;
        BoxCollider bc = GetComponent<BoxCollider>();
        //bc.contactOffset = 0.2f;        
    }
    void Start () {
        originalPos = transform.position;        
        rotation = transform.localRotation;
        scale = transform.localScale;
        originPar = transform.parent;

        rangeX = transform.parent.localScale.x / 2;
        rangeY = transform.parent.localScale.y / 2;
        rangeZ = transform.parent.localScale.z / 2;
        //Debug.LogError(transform.parent);
        Vector3 moveDir = new Vector3(UnityEngine.Random.Range(-rangeX, rangeX), UnityEngine.Random.Range(-rangeY, rangeY), UnityEngine.Random.Range(-rangeZ, rangeZ));
        Vector3 newPos = transform.position + moveDir;
        if(transform.name=="o2")
        {
            transform.position = newPos;
        }


        z = originalPos.z;
        timeLimit = 10;
    }

    // Update is called once per frame
    float time = 0f;
    public float timeLimit= 2f;
    void UpdateCube () {
        
        if (isComplete)
        {
            isComplete = false;
            Vector3 moveDir = new Vector3(UnityEngine.Random.Range(-rangeX, rangeX), UnityEngine.Random.Range(-rangeY, rangeY), UnityEngine.Random.Range(-rangeZ, rangeZ));
            Vector3 newPos = transform.position + moveDir;
            
            if (newPos.x < originalPos.x - rangeX || newPos.x > originalPos.x + rangeX)
            {
                moveDir.x = -moveDir.x;
            }
            if (newPos.y < originalPos.y - rangeY || newPos.y > originalPos.y + rangeY)
            {
                moveDir.y = -moveDir.y;
            }
            if (newPos.z < originalPos.z - rangeZ || newPos.z > originalPos.z + rangeZ)
            {
                moveDir.z = -moveDir.z;
            }
            Vector3 endpos = transform.position + moveDir;

            //if (transform.name == "Co2")
            //{
            //    Debug.DrawLine(transform.position, endpos, Color.red, 10);
            //}


            tween = transform.DOMove(endpos, timeLimit);
            tween.SetEase(Ease.Linear);
            tween.onComplete = () => {
                isComplete = true;
            };
        }      

    }
   
    float radius = 0.5f;
    public bool isStop = false;
    private void UpdateSphere()
    {      
        time += Time.deltaTime;
        if (time >=timeLimit)
        {
            time = 0;
            Vector3 moveDir = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) * Vector3.forward*Random.Range(0,radius);
            Vector3 endpos = originalPos + moveDir;
            //Debug.DrawLine(originalPos, endpos, Color.red, 10);
            tween = transform.DOMove(endpos, timeLimit);
        }
    }
    public bool isDraging = false;
    private void Update()
    {
        if (!isDraging)
        {
            UpdateCube();
        }     
    }
    private void LateUpdate()
    {
        if (isDraging)
        {
            RaycastHit rhit;
            RaycastHit? hit;
            hit = Raycast(out rhit);
            //Debug.Log(hit);
            if (hit!=null)
            {
                //Debug.Log(hit.GetValueOrDefault().transform.name);
                lastPos = transform.position;
            }
            else
            {
                //Debug.Log(transform.name);
                transform.position = lastPos;
            }
        }
    }
    SimpleDrag simpleDrag;
    private RaycastHit? Raycast(out RaycastHit raycastHit)
    {
        if (simpleDrag == null)
        {
            simpleDrag = Tools.GetScenesObj("SimpleDrag").GetComponent<SimpleDrag>();
        }
        Physics.queriesHitBackfaces = true;
        if (Monitor23DMode.instance.is3D)//3d状态下
        {
            if (Physics.Raycast(FCore.penRay, out raycastHit, Mathf.Infinity,  ~(/*1<<LayerMask.NameToLayer("environment")|*/1<<LayerMask.NameToLayer("backEnvironment")| 1 << LayerMask.NameToLayer("tagEnvironment")| 1 <<gameObject.layer)))
            {               
                return raycastHit;
            }
        }
        else
        {
            Ray ray = Monitor23DMode.instance.camera2D.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity,~(/*1 << LayerMask.NameToLayer("environment") | */1 << LayerMask.NameToLayer("tagEnvironment")| 1 << LayerMask.NameToLayer("backEnvironment") |1 << gameObject.layer)))
            {
                return raycastHit;
            }
        }
        Physics.queriesHitBackfaces = false;
        return null;
    }

    Camera3D cam3d;
    Vector3 GetTouchPos
    {
        get
        {
            Vector3 touchPos;
            if (Monitor23DMode.instance.is3D)
            {
                if (cam3d==null)
                {
                    cam3d = Tools.GetScenesObj("Camera3D").GetComponent<Camera3D>();
                }
              touchPos= cam3d.cam_l.ScreenToWorldPoint(FCore.penPosition);
            }
            else
            {
                touchPos= Monitor23DMode.instance.camera2D.ScreenToWorldPoint(Input.mousePosition);
            }
            return touchPos;
        }
    }
    /// <summary>
    /// 开始拖拽停止运动
    /// </summary>
    public void SetPause()
    {      
        isDraging = true;
        backPos = transform.position;
        DOTween.Pause(transform);        
    }
    /// <summary>
    /// 停止拖拽返回
    /// </summary>
    public void SetBack()
    {
        //this.enabled = true;
        transform.DOMove(backPos, 0.3f).onComplete = () =>//
        {
            isDraging = false;
            DOTween.Play(transform);
        };
        //BoxCollider bc;        
    }

    /// <summary>
    /// 返回到初始点，重新运行
    /// </summary>
    public void ResetMove()
    {
        transform.SetParent(originPar);
        transform.localRotation = rotation;
        transform.localScale = scale;
        transform.position = originalPos;
        isDraging = false;
        isComplete = true;
        //Debug.Log(isComplete);
    }

    public void Move()
    {
        DOTween.Kill(transform);
        transform.localPosition = Vector3.zero;
        originalPos =transform.TransformPoint(Vector3.zero);//需要的是世界坐标
        isDraging = false;
        isComplete = true;   
    }

    /// <summary>
    /// 进入半球或者血管
    /// </summary>
    //public bool isEntrigger = false;
    //private void OnTriggerEnter(Collider other)
    //{

    //    //Debug.Log(other.gameObject.name);
    //    if (other.gameObject.name== "o2")
    //    {
    //        isEntrigger = true; 
    //        transform.position = lastPos;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.name == "o2")
    //    {
    //        isEntrigger = true;
    //        transform.position = lastPos;
    //    }
    //}

    ///// <summary>
    ///// 推出半球或者血管
    ///// </summary>
    ///// <param name="other"></param>
    //private void OnTriggerExit(Collider other)
    //{
    //    other.contactOffset = 0.1f;
    //    //Debug.Log(other.gameObject.name);
    //    if (other.gameObject.name == "Mesh_halfQiu")
    //    {
    //        isEntrigger = false;
    //        //other
    //    }
    //}


    private void OnDisable()
    {
        lastPos = transform.position;
    }
    Vector3 hitPos;
    private void OnDrawGizmos1()
    {
        Gizmos.DrawSphere(hitPos, 0.01f);
        Gizmos.color =new Color(255,255,100,10);
    }
}
