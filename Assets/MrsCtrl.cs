using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GCSeries;
using liu;

public class MrsCtrl : MonoBehaviour
{

    private void OnEnable()
    {
        if (this.gameObject.GetComponent<MRSystem>() != null)
        {
            this.gameObject.GetComponent<MRSystem>().ViewerScale = 1.5f;
            this.gameObject.GetComponent<MRSystem>().isAutoSlant = false;
        }
    }
    private void Reset()
    {
        if (this.gameObject.GetComponent<MRSystem>() != null)
        {
            this.gameObject.GetComponent<MRSystem>().ViewerScale = 1.5f;
            this.gameObject.GetComponent<MRSystem>().isAutoSlant = false;
        }
    }
}
