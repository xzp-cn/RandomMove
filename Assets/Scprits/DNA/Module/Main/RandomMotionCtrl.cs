using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMotionCtrl : MonoBehaviour {
    public int num = 20;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < num; i++)
        {
            GameObject o2= ResManager.GetPrefab("Prefabs/o2");
            o2.transform.SetParent(transform, false);
            o2.layer = LayerMask.NameToLayer("O2");
        }
	}
}
