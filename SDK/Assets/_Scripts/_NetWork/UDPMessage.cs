using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using MyToolsSetting;
public class UDPMessage : MonoBehaviour {
    Socket socket;
    EndPoint remote;
    public static string str = "启动";
    int length = 0;
    Thread connectThread;
    void Start()
    {
        StartCoroutine(InitSocket());
    }

    IEnumerator InitSocket()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            remote = new IPEndPoint(IPAddress.Any, 1234);
            socket.Bind(remote);
            connectThread = new Thread(new ThreadStart(ReceiveMessage));
            connectThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SocketSend(string sendStr)
    {
        //清空发送缓存
        byte[] sendData = new byte[1024];
        //数据类型转换
        sendData = Encoding.Default.GetBytes(sendStr);
        //发送
        socket.Send(sendData, sendData.Length, SocketFlags.None);
    }

    public void ReceiveMessage()
    {
        while (true)
        {
            byte[] data = new byte[1024];
            try
            {
                length = socket.ReceiveFrom(data, ref remote);
            }
            catch (System.Exception e)
            {
                Debug.Log("接收信息错误，错误信息：  " + e.ToString());
            }
            if (length > 0)
            {
                str = Encoding.UTF8.GetString(data, 0, length);
            }
            Thread.Sleep(1000);//三秒接收一次
        }
    }
    //连接关闭
    void SocketQuit()
    {
        //关闭线程
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最后关闭socket
        if (socket != null)
            socket.Close();
    }
    void OnApplicationQuit()
    {
        SocketQuit();
    }
    private void OnDestroy()
    {
        SocketQuit();
    }
}
