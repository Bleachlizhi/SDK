using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MyToolsSetting
{
    public static class ActionTools
    {

        [SerializeField]
        private static Dictionary<MyEventName, Delegate> m_EventTable = new Dictionary<MyEventName, Delegate>();

        //回调委托
        public delegate void CallBack();
        public delegate void CallBack<T>(T arg);
        public delegate void CallBack<T,X>(T arg1,X arg2);
        public delegate void CallBack<T,X,Z>(T arg1,X arg2,Z arg3);
        public delegate void CallBack<T,X,Z,W>(T arg1,X arg2,Z arg3,W arg4);
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="myEventName"></param>
        /// <param name="callback"></param>
        private static void OnListenerAdding(MyEventName myEventName,Delegate callback)
        {
            if (!m_EventTable.ContainsKey(myEventName))
            {
                m_EventTable.Add(myEventName, null);
            }
            Delegate d = m_EventTable[myEventName];
            if (d != null && d.GetType() != callback.GetType())
            {
                throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托类型是{1}，要添加的委托类型为{2}", myEventName, d.GetType(), callback.GetType()));
            }
        }
        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="myEventName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private static void OnListenerRemoving(MyEventName myEventName, Delegate callback)
        {
            if (m_EventTable.ContainsKey(myEventName))
            {
                Delegate d = m_EventTable[myEventName];
                if (d == null)
                {
                    throw new Exception(string.Format("移除监听错误:事件{0}未添加任何委托", myEventName));
                }
                else if (d.GetType() != callback.GetType())
                {
                    throw new Exception(string.Format("移除监听错误:事件{0}没有对应的委托", myEventName));
                }
            }
            else
            {
                throw new Exception(string.Format("事件系统未包含{0}事件", myEventName));
            }
        }
        /// <summary>
        /// 移除未添加委托的事件
        /// </summary>
        /// <param name="myEventName"></param>
        private static void OnListenerRemoved(MyEventName myEventName)
        {
            if (m_EventTable[myEventName] == null)
                m_EventTable.Remove(myEventName);
        }


        //添加无参数委托
        public static void AddListener(MyEventName myEventName , CallBack callBack)
        {
            OnListenerAdding(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack)m_EventTable[myEventName] + callBack;
        }

        //添加1参数委托
        public static void AddListener<T>(MyEventName myEventName, CallBack<T> callBack)
        {
            OnListenerAdding(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack<T>)m_EventTable[myEventName] + callBack;
        }

        //添加2参数委托
        public static void AddListener<T,X>(MyEventName myEventName, CallBack<T,X> callBack)
        {
            OnListenerAdding(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack<T,X>)m_EventTable[myEventName] + callBack;
        }

        //添加3参数委托
        public static void AddListener<T, X, Z>(MyEventName myEventName, CallBack<T, X, Z> callBack)
        {
            OnListenerAdding(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack<T, X, Z>)m_EventTable[myEventName] + callBack;
        }

        //添加4参数委托
        public static void AddListener<T, X, Z, W>(MyEventName myEventName, CallBack<T, X, Z, W> callBack)
        {
            OnListenerAdding(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack<T, X, Z, W>)m_EventTable[myEventName] + callBack;
        }

        //移除无参数委托
        public static void RemoveListener(MyEventName myEventName , CallBack callBack)
        {
            OnListenerRemoving(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack)m_EventTable[myEventName] - callBack;
            OnListenerRemoved(myEventName);
        }

        //移除1参数委托
        public static void RemoveListener<T>(MyEventName myEventName, CallBack<T> callBack)
        {
            OnListenerRemoving(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack<T>)m_EventTable[myEventName] - callBack;
            OnListenerRemoved(myEventName);
        }

        //移除2参数委托
        public static void RemoveListener<T,X>(MyEventName myEventName, CallBack<T,X> callBack)
        {
            OnListenerRemoving(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack<T,X>)m_EventTable[myEventName] - callBack;
            OnListenerRemoved(myEventName);
        }

        //移除3参数委托
        public static void RemoveListener<T, X,Z>(MyEventName myEventName, CallBack<T, X,Z> callBack)
        {
            OnListenerRemoving(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack<T, X,Z>)m_EventTable[myEventName] - callBack;
            OnListenerRemoved(myEventName);
        }

        //移除4参数委托
        public static void RemoveListener<T, X, Z,W>(MyEventName myEventName, CallBack<T, X, Z, W> callBack)
        {
            OnListenerRemoving(myEventName, callBack);
            m_EventTable[myEventName] = (CallBack<T, X, Z,W>)m_EventTable[myEventName] - callBack;
            OnListenerRemoved(myEventName);
        }




        /// <summary>
        /// 广播无参数类型
        /// </summary>
        /// <param name="myEventName"></param>
        public static void Broadcast(MyEventName myEventName)
        {
            Delegate d;
            Debug.Log(m_EventTable.TryGetValue(myEventName, out d));
            if (m_EventTable.TryGetValue(myEventName, out d))
            {
                CallBack callBack = d as CallBack;
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(string.Format("广播失败:事件{0}未包含无参数类型委托", myEventName));
                }

            }
        }
        /// <summary>
        /// 广播1参数类型
        /// </summary>
        /// <param name="myEventName"></param>
        public static void Broadcast<T>(MyEventName myEventName,T arg)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(myEventName, out d))
            {
                CallBack<T> callBack = d as CallBack<T>;
                if (callBack != null)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("广播失败:事件{0}未包含1参数类型委托", myEventName));
                }

            }
        }

        /// <summary>
        /// 广播2参数类型
        /// </summary>
        /// <param name="myEventName"></param>
        public static void Broadcast<T,X>(MyEventName myEventName,T arg1, X arg2)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(myEventName, out d))
            {
                CallBack<T,X> callBack = d as CallBack<T,X>;
                if (callBack != null)
                {
                    callBack(arg1,arg2);
                }
                else
                {
                    throw new Exception(string.Format("广播失败:事件{0}未包含2参数类型委托", myEventName));
                }

            }
        }

        /// <summary>
        /// 广播3参数类型
        /// </summary>
        /// <param name="myEventName"></param>
        public static void Broadcast<T, X, Z>(MyEventName myEventName, T arg1, X arg2, Z arg3)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(myEventName, out d))
            {
                CallBack<T, X, Z> callBack = d as CallBack<T, X, Z>;
                if (callBack != null)
                {
                    callBack(arg1, arg2,arg3);
                }
                else
                {
                    throw new Exception(string.Format("广播失败:事件{0}未包含3参数类型委托", myEventName));
                }

            }
        }

        /// <summary>
        /// 广播4参数类型
        /// </summary>
        /// <param name="myEventName"></param>
        public static void Broadcast<T, X, Z , W>(MyEventName myEventName, T arg1, X arg2, Z arg3, W arg4)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(myEventName, out d))
            {
                CallBack<T, X, Z, W> callBack = d as CallBack<T, X, Z , W>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(string.Format("广播失败:事件{0}未包含3参数类型委托", myEventName));
                }

            }
        }

    }
}
/// <summary>
/// 注册事件
/// </summary>
public enum MyEventName
{
    test1,
    test2,
    test3
}

