using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSettingForWindows : MonoBehaviour {
    [SerializeField]
    private int m_Wight = 1920;
    [SerializeField]
    private int m_Height =1080;
    [SerializeField]
    private bool m_IsFullScreen = true;
    private void Awake()
    {
        Screen.SetResolution(this.m_Wight, this.m_Height, this.m_IsFullScreen);
    }
}
