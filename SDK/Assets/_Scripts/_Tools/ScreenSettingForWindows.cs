using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScreenSettingForWindows : MonoBehaviour {
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;  //边框用的
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;



    [SerializeField]
    private bool _isCrossScreen;
    [SerializeField]
    private int m_Wight = 1920;
    [SerializeField]
    private int m_Height =1080;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        Screen.SetResolution(this.m_Wight, this.m_Height, !this._isCrossScreen);
        if (this._isCrossScreen)
        {
            yield return new WaitForSeconds(.1f);
            SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);      //无边框
            SetWindowPos(GetForegroundWindow(), 0, 0, 0, this.m_Wight, this.m_Height, SWP_SHOWWINDOW);       //设置屏幕大小和位置
        }
    }
}
