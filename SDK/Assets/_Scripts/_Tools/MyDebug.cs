using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyToolsSetting
{
    public static class MyDebug
    {
        public static bool isDebug;
        public static void Log(string str)
        {
            if (!isDebug)
                return;
            Debug.Log("----------" + str);
        }
        public static void Log(string str, MyColor.State color)
        {
            if (!isDebug)
                return;
            Debug.Log("<color=" + color.ToString() + ">" + "----------" + str + "</color>");
        }
    }
    public static class MyColor
    {
        public enum State
        {
            black,
            blue,
            green,
            red,
            yellow
        }

    }
}

