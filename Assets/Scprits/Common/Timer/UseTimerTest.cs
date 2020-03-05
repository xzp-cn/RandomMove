﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTimerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //每隔1秒执行
        MyFrameWork.FrameTimerHeap.AddTimer(0, 1000, () =>
        {
            F3D.AppLog.AddMsg("Exceute Check Screen State.");
        });


        //倒计时
        MyFrameWork.TimerManager.GetTimer(gameObject).StartTimer(5, (t) => {
            F3D.AppLog.AddMsg("Left Seconds: " + t);
        }, () => { F3D.AppLog.AddMsg("End Timer"); });
    }

    // Update is called once per frame
    void Update () {
        MyFrameWork.TimerManager.Instance.Update();
    }
}
