using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 红细胞遇到氧气的时候，颜色会变成鲜红色
/// </summary>
public class JiaoHu : MonoBehaviour
{

    public Material originalMaterial;//红细胞吸收氧气之前的材质
    public Material afterMaterial;//红细胞吸收氧气后的材质
    private List<GameObject> hongxibao = new List<GameObject>();


    private void Awake()
    {
        //_Instance = this;
    }
    void Start()
    {
        Debug.Log(hongxibao.Count);

    }

    // Update is called once per frame 
    /// <summary>
    /// 与氧气接触的红细胞要变色
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //将变色的红细胞全部保存起来              
        hongxibao.Add(other.gameObject);
        if(this.transform.localScale!= new Vector3(1e-12f, 1e-12f, 1e-12f))
        Invoke("ChangeMaterial",1.5f);
    }
    /// <summary>
    /// 接触氧气，红细胞变色
    /// </summary>
    public void ChangeMaterial()
    {
        Debug.Log(hongxibao.Count);
        for (int i = 0; i < hongxibao.Count; i++)
        {
            hongxibao[i].GetComponent<Renderer>().material.CopyPropertiesFromMaterial(afterMaterial);
        }
    }
    /// <summary>
    /// 动画播放完成，红细胞恢复原来颜色
    /// </summary>
    public void RecoverMaterial()
    {

        for (int i = 0; i < hongxibao.Count; i++)
        {
            hongxibao[i].GetComponent<Renderer>().material.CopyPropertiesFromMaterial(originalMaterial);
        }
        hongxibao.Clear();
    }
}
