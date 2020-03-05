using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using huang.module.ui.settingui;

/// <summary>
/// 快捷键切换投屏
/// </summary>
public class ShortcutKeysForProjection : MonoBehaviour
{
    UISetting uISetting;

    UISetting GUISetting
    {
        get
        {
            if (uISetting == null) uISetting = FindObjectOfType<UISetting>();
            return uISetting;
        }

    }
    private void Update()
    {

        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(GUISetting.ShortcutKeySetVR2D());

                return;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {

                StartCoroutine(GUISetting.ShortcutKeySetAR2D());
                return;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {

                StartCoroutine(GUISetting.ShortcutKeySetVR3D());
                return;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {

                StartCoroutine(GUISetting.ShortcutKeySetAR3D());
                return;
            }
        }
    }

}
