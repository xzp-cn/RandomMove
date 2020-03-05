using liu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagCtrl : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(Monitor23DMode.instance.CameraLeft.transform.forward) * Quaternion.AngleAxis(180, Vector3.up);
    }
}
