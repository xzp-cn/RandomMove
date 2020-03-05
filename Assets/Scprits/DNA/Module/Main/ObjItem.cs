using UnityEngine;

/// <summary>
/// 记录实验仪器的状态
/// </summary>
public class ObjItem : MonoBehaviour
{
    Vector3 initPos;
    Quaternion t;
    Vector3 scale;
    private void Awake()
    {
        initPos = transform.localPosition;
        t = transform.localRotation;
        scale = transform.localScale;
    }

    /// <summary>
    ///托盘中的初始状态
    /// </summary>
    public void InitState()
    {
        transform.localPosition = initPos;
        transform.localRotation = t;
        transform.localScale = scale;
    }
}
  
