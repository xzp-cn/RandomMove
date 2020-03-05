using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashItem : MonoBehaviour {

    public Color colorFrom, colorTo;
    public float step=0.1f;
    Material mat;
    private void Start()
    {
        SphereCollider bc=gameObject.CheckAddComponent<SphereCollider>();
        bc.enabled = true;
        ColorUtility.TryParseHtmlString("#00FF03", out colorFrom);
        ColorUtility.TryParseHtmlString("#1A6A26", out colorTo);
        mat = GetComponent<MeshRenderer>().materials[0];
        StartCoroutine(ColorFlash());
    }

    IEnumerator ColorFlash()
    {
        WaitForSeconds wf = new WaitForSeconds(0.01f);
        float lerp = step;
        Color temp;
        while (true)
        {
            for (float i = 0; i < 1; i+=0.1f)
            {
                temp = Color.Lerp(colorFrom, colorTo, i);
                mat.color = temp;
                yield return wf;
            }
            Color c = colorFrom;
            colorFrom = colorTo;
            colorTo = c;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
