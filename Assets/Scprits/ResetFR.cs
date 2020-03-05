using GCSeries;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFR : MonoBehaviour {

    private void OnEnable()
    {
        GetComponentInParent<FAR>().ResetFARPosFollowViewScale();
    }
}
