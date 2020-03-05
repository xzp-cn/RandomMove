using liu;
using Runing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using xuexue.common.drag2dtool;
using operatemodeltool;

namespace GCSeries
{
    /// <summary>
    /// 简单拖拽的示例
    /// </summary>
    public class SimpleDrag : MonoBehaviour
    {        
        /// <summary>
        /// 创建的笔的射线物体
        /// </summary>
        GameObject _penObj;

        [HideInInspector]
        public PenRay tempPenRay;

        /// <summary>
        /// 是否在点击的时候震动一下
        /// </summary>
        public bool enableShake = true;



        Camera camera2D;
        void Start()
        {
            //设置屏幕为3D显示模式
            // FCore.SetScreen3D();

            FCore.EventKey0Down += OnKey0Down;
            FCore.EventKey0Up += OnKey0Up;

            FCore.EventKey1Down += OnKey0Down;
            FCore.EventKey1Up += OnKey0Up;

            _penObj = new GameObject("penRay");
            tempPenRay = _penObj.AddComponent<PenRay>();


            //通过3DUI物体找到挂在在上面的UIButton3D脚本。
            // uibutton3d = FindObjectOfType<UIButton3D>();
            camera2D = Monitor23DMode.instance.camera2D;
        }

        void OnApplicationQuit()
        {
            //在程序退出的时候设置屏幕为2D显示
            FCore.SetScreen2D();
        }

        /// <summary>
        /// 记录当前拖拽的物体
        /// </summary>
        [HideInInspector]
        public GameObject _curDragObj;

        private GameObject Raycast(out RaycastHit raycastHit)
        {
            if (Physics.Raycast(FCore.penRay, out raycastHit, tempPenRay.rayLength, ~(1 << LayerMask.NameToLayer("environment"))))
            {
                return raycastHit.collider.gameObject;
            }
            return null;
        }


        /// <summary>
        /// 3D 拖拽或者移动
        /// </summary>
        private void OnKey0Down()
        {
            RaycastHit raycastHit;
            dragObj = Raycast(out raycastHit);

            if (dragObj != null)
            {
                if (Monitor23DMode.instance.is3D)
                {

                    if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("cannotDrag"))//不可拖拽
                    {
                        return;
                    }


                    dragObj = raycastHit.collider.gameObject;
                    _curDragObj = dragObj;
                    if (dragObj.layer == LayerMask.NameToLayer("feiTag"))
                    {
                        if (SpliteControl.Instance != null && SpliteControl.Instance.curSpliteState == SpliteControl.SpliteState.Combine)
                        {
                            _curDragObj = dragObj.transform.parent.parent.gameObject;
                        }
                    }

                    //不可拖拽变量
                    if (dragObj.layer == LayerMask.NameToLayer("environment") || dragObj.layer == LayerMask.NameToLayer("tagEnvironment")|| dragObj.layer == LayerMask.NameToLayer("leftwall"))//不可拖拽
                    {
                        return;
                    }
                    ///氧气拖拽
                    GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.CLICKDOWN_O2, _curDragObj);

                    ///二氧化碳拖拽
                    GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.CLICKDOWN_CO2, _curDragObj);

                    if (GlobalConfig.Instance.operationModel == OperationModel.Move)//移动物体
                    {

                        //添加抓取的物体
                        FCore.addDragObj(_curDragObj, raycastHit.distance, true);
                    }
                    else if (GlobalConfig.Instance.operationModel == OperationModel.Rotate)//旋转物体
                    {
                        OperationModelTool.Instance.AddRotaObject(_curDragObj);
                    }

                    if (enableShake)
                    {
                        FCore.PenShake();//震动一下
                    }

                    GlobalConfig.Instance._curOperateObj = _curDragObj;

                }
            }
            else
            {
                GlobalEntity.GetInstance().Dispatch(MsgEnum.CLICK_NONE);
            }
        }

        public void OnKey0Up()
        {
            //移出抓取的物体         
            FCore.deleteDragObj(_curDragObj);

            //移除旋转的物体
            OperationModelTool.Instance.DeleRotaObject();

            if (dragObj != null && _time < 0.5f && (dragObj.layer == LayerMask.NameToLayer("Tag")| dragObj.layer == LayerMask.NameToLayer("feiTag")))
            {
                GlobalEntity.GetInstance().Dispatch<string>(MsgEnum.CLICK_TAG, dragObj.name);
            }
            _time = 0;
            isTiming = false;

            if (dragObj != null)
            {
                GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.DRAG_TAG, dragObj);

                //点起来
                GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.CLICKUP_CO2, _curDragObj);

                ///二氧化碳点起
                GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.CLICKUP_O2, _curDragObj);
            }

            dragObj = null;
            _curDragObj = null;
        }

        /// <summary>
        /// 计时器
        /// </summary>
        float _time = 0;
        bool isTiming = false;
        private void Update()
        {
            if (Monitor23DMode.instance.is3D == false)//这个判断不需要 如果需要在2/3D都能用鼠标拖拽的话
            {
                if (Input.GetMouseButtonDown(0)) {
                    isTiming = true;
                    _time = 0;
                    Drag2DObj();
                }

                if (Input.GetMouseButtonUp(0))
                {                                    
                    OnMouseBtnUp();
                }
            }

            if (isTiming)
            {
                _time += Time.deltaTime;               
            }
        }

        void OnMouseBtnUp()
        {
            Drag2DTool.Instance.clearDragObj(); 
            OperationModelTool.Instance.DeleRotaObject();

            if (dragObj!=null&&_time<0.5f&&(dragObj.layer==LayerMask.NameToLayer("feiTag")|| dragObj.layer == LayerMask.NameToLayer("Tag")))
            {
                GlobalEntity.GetInstance().Dispatch<string>(MsgEnum.CLICK_TAG, dragObj.name);
            }
            _time = 0;
            isTiming = false;
            if (dragObj!=null)
            {
                GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.DRAG_TAG, dragObj);

                GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.CLICKUP_O2, _curDragObj);
                ///二氧化碳拖拽
                GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.CLICKUP_CO2, _curDragObj);
            }
         
            //检测标签是否点击到           
            dragObj = null;
            _curDragObj = null;
        }

       public GameObject dragObj = null;
        void Drag2DObj()
        {
            RaycastHit raycastHit;
            //int defaultLayer = LayerMask.NameToLayer("Default");//这个层是模型
            Ray ray = Monitor23DMode.instance.camera2D.ScreenPointToRay(Input.mousePosition);
            var uiDis = 1000f;//鼠标到UI的距离
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                uiDis = Monitor23DMode.instance.f3DSpaceInputModule.hitUIDis;
            }   
          
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity/*,~(1 << LayerMask.NameToLayer("backEnvironment")*/))
            {
                if (uiDis < raycastHit.distance)//通过鼠标到UI跟鼠标到物体的距离判断是否进行对模型操作
                {
                    return;
                }
                if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("cannotDrag"))//不可拖拽
                {
                    return;
                }


                dragObj = raycastHit.collider.gameObject;
                _curDragObj = dragObj;
                if (dragObj.layer == LayerMask.NameToLayer("feiTag"))// 显示隐藏标签
                {                   
                    if (SpliteControl.Instance!=null&& SpliteControl.Instance.curSpliteState == SpliteControl.SpliteState.Combine)
                    {                        
                        _curDragObj = dragObj.transform.parent.parent.gameObject;
                    }
                }

                if (dragObj.layer == LayerMask.NameToLayer("environment") || dragObj.layer == LayerMask.NameToLayer("tagEnvironment") || dragObj.layer == LayerMask.NameToLayer("leftwall"))//不可拖拽
                {
                    return;
                }

                ///氧气拖拽
                GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.CLICKDOWN_O2, _curDragObj);

                ///二氧化碳拖拽                
                GlobalEntity.GetInstance().Dispatch<GameObject>(MsgEnum.CLICKDOWN_CO2, _curDragObj);

                if (GlobalConfig.Instance.operationModel == OperationModel.Move)
                {
                    Drag2DTool.Instance.addDragObj(_curDragObj, camera2D);
                }
                else if (GlobalConfig.Instance.operationModel == OperationModel.Rotate)
                {
                    OperationModelTool.Instance.AddRotaObject(_curDragObj);
                }

                GlobalConfig.Instance._curOperateObj = _curDragObj;

                

            }
            else
            {
                GlobalEntity.GetInstance().Dispatch(MsgEnum.CLICK_NONE);//点击空白处
            }
        }

    }
}
public enum MsgEnum
{
    CLICK_POINT,//点击闪光点
    CLICK_NONE,//点击空白处
    CLICK_TAG,//点击标签
    DRAG_TAG,//拖拽标签
    CLICKDOWN_O2,//拖拽标签
    CLICKUP_O2,
    CLICKDOWN_CO2,//拖拽标签
    CLICKUP_CO2//拖拽标签
}
