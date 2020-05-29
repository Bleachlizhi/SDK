using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using MyToolsSetting;
using System.Text;
using System;

public class TCPClient
{
    private string _ip;
    private int _port;
    private Thread _thread;
    private ThreadStart _threadStart;

    public string _recStr;
    int _recLength;


    private const int BUFFER_SIZE = 128;
    Socket _socket;
    IPEndPoint _iPEndPoint;
    bool isConnect;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public TCPClient(string ip, int port)
    {
        this._ip = ip;
        this._port = port;
        this._iPEndPoint = new IPEndPoint(IPAddress.Parse(this._ip), this._port);
        this._threadStart = new ThreadStart(Receive);
        this._thread = new Thread(this._threadStart);
        this._thread.Start();
    }
    /// <summary>
    /// 连接至服务器
    /// </summary>
    void SocketConnect()
    {
        if (this._socket != null)
            this._socket.Close();
        try
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MyDebug.Log(">>>准备连接", MyColor.State.green);
            _socket.Connect(this._iPEndPoint);
            MyDebug.Log(">>>连接至服务器：" + this._ip, MyColor.State.green);
        }
        catch (Exception e)
        {
            MyDebug.Log(e.ToString(), MyColor.State.red);
        }
    }
    /// <summary>
    /// 消息至服务端
    /// </summary>
    /// <param name="str"></param>
    public void Send(string str)
    {
        if (string.IsNullOrEmpty(str))
            return;
        if (!this._socket.Connected)
            return;
        byte[] _sendBuff = new byte[BUFFER_SIZE];
        _sendBuff = Encoding.UTF8.GetBytes(str);
        _socket.Send(_sendBuff, _sendBuff.Length, SocketFlags.None);
        MyDebug.Log(">>>发送:" + str, MyColor.State.green);
    }
    /// <summary>
    /// 接收服务端消息
    /// </summary>
    void Receive()
    {
        SocketConnect();
        while (true)
        {
            if (!this._socket.Connected)
            {
                Thread.Sleep(2000);
                SocketConnect();
            }
            else
            {
                byte[] _recBuff = new byte[BUFFER_SIZE];
                this._recLength = this._socket.Receive(_recBuff);
                if (this._recLength == 0)
                {
                    SocketConnect();
                    continue;
                }
                this._recStr = Encoding.UTF8.GetString(_recBuff, 0, this._recLength);
                MyDebug.Log(">>>接收到信息:" + this._recStr, MyColor.State.yellow);
            }
        }
    }
    /// <summary>
    /// 关闭Socket
    /// </summary>
    public void CloseSocket()
    {
        if (this._thread != null)
            this._thread.Abort();
        if (this._socket != null)
            this._socket.Close();
    }
}
