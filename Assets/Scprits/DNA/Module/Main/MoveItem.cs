using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;

public class MoveItem : MonoBehaviour {

    // Use this for initialization    
    Vector3[] pos;
    TweenerCore<Vector3, Path, PathOptions>  tw;
    bool isComplete = false;
    public int length;
    public int changeIndex = 0;
    public int nextIndex=0;    
    float speed=0.002f;
    Vector3 movePos;

    public Material originalMaterial;//红细胞吸收氧气之前的材质
    public Material afterMaterial;//红细胞吸收氧气后的材质
                                  //public float minDis=1;
    void Start () {
        length= transform.parent.childCount;
        pos = new Vector3[length];
        int startIndex=transform.GetSiblingIndex();
        changeIndex = length - startIndex-1;//转折点索引
        for (int i = startIndex,j=0; j < length; j++)
        {           
            startIndex %= length;
           // Debug.Log(startIndex);
            pos[j]=transform.parent.GetChild(startIndex).position;
            startIndex++;
        }
        nextIndex =1;
        movePos = transform.position;
        if (originalMaterial==null||afterMaterial==null)
        {
            originalMaterial = Resources.Load<Material>("Prefabs/M_fei_wuyang");
            afterMaterial = Resources.Load<Material>("Prefabs/M_fei_youyang");
        }
        //Debug.Log(length+"   "+startIndex);

    }   
	// Update is called once per frame
	void UpdateTween () {

        if (isComplete)
        {
            isComplete = false;
            tw = transform.DOPath(pos, 10, PathType.CatmullRom, PathMode.Full3D, pos.Length + 10, Color.red);
            tw.onComplete = () =>
            {
                isComplete = tw.IsComplete();
            };
        }
        float dis= Vector3.Distance(transform.position, pos[changeIndex]);
        //if (minDis<dis)
        //{
        //    minDis = dis;
        //    Debug.Log(minDis);
        //}
        if ( dis< 0.01f)
        {
            Debug.Log("  "+dis);
            DOTween.KillAll();
            transform.position = pos[0];
        }
    }
   
    private void Update()
    {
        if (Vector3.Distance(transform.position,pos[nextIndex])> speed)
        {
            Vector3 dir=Vector3.Normalize(pos[nextIndex] - transform.position);
            Vector3 tempPos=transform.position+ dir* speed;
            transform.position = tempPos;
        }
        else
        {          
            nextIndex %=length;
            transform.position = pos[nextIndex];        
                       
            if (nextIndex==changeIndex)
            {
                //Debug.Log(changeIndex);
                //从初始位置重置
                transform.GetComponent<MeshRenderer>().enabled = true;
                transform.GetComponent<BoxCollider>().enabled = true;                

                transform.position = pos[(changeIndex+1)%length];                
                nextIndex+=2;

                if (this.name.Contains("xuexibao"))
                {
                    Material mat = transform.GetComponent<MeshRenderer>().material;
                    mat.CopyPropertiesFromMaterial(originalMaterial);
                }               
            }
            else
            {
                nextIndex += 1;
            }
            nextIndex %= length;
        }
    }

    public void AbsorbO2()
    {
        Material mat = transform.GetComponent<MeshRenderer>().material;
        mat.CopyPropertiesFromMaterial(afterMaterial);
    }
    void DrawSphere()
    {       

    }
}
