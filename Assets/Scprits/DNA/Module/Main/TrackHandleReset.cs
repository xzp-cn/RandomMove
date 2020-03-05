using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrackHandleReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetPenHandeLocalScale();
	}
	
	// Update is called once per frame
	void SetPenHandeLocalScale () {     
        string ed = Application.streamingAssetsPath + "/handle.txt";
        float scale = 2;
        if (File.Exists(ed))
        {
            try
            {
              scale= float.Parse(File.ReadAllText(ed));

            }
            catch (System.Exception ex)
            {
                F3D.AppLog.AddMsg("ScreenCamera3D.CloseSelfDelay(): Exception " + ex.Message);
            }
        }
        else
        {
            File.WriteAllText(ed, scale.ToString());
        }

        transform.localScale =Vector3.one* scale;
    }
}
