using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using MyToolsSetting;
using System.Text;
using System;

public class TCPServer
{
    public string _recStr;
    private const int BUFFER_SIZE = 128;
    private Socket serverSocket;
    private List<Socket> clientSockets = new List<Socket>();

    public TCPServer(string ip ,int port ,int connectMaxCount)
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        serverSocket.Bind(iPEndPoint);
        serverSocket.Listen(connectMaxCount);
        Accept();
    }
    /// <summary>
    /// 连接客户端
    /// </summary>
    void Accept()
    {
        try
        {
            serverSocket.BeginAccept(new AsyncCallback(Accept_Callback), serverSocket);
        }
        catch (Exception e)
        {
            MyDebug.Log(e.Message, MyColor.State.red);
        }
    }

    /// <summary>
    /// 客户端连接至服务器返回信息
    /// </summary>
    /// <param name="ar"></param>
    private void Accept_Callback(IAsyncResult ar)
    {
        Socket socket = serverSocket.EndAccept(ar);
        MyDebug.Log(">>>" + socket.LocalEndPoint.ToString() + " 连接至服务器",MyColor.State.blue);
        if (!clientSockets.Contains(socket))
        {
            clientSockets.Add(socket);
            StateObject stateObject = new StateObject(BUFFER_SIZE, socket);

            try
            {
                socket.BeginReceive(stateObject.buffer, 0, BUFFER_SIZE, 0, new AsyncCallback(Receive_Callback), stateObject);
            }
            catch (Exception e)
            {
                MyDebug.Log(e.Message,MyColor.State.red);
            }
        }
        Accept();
    }

    /// <summary>
    /// 客户端向服务器发送信息
    /// </summary>
    /// <param name="ar"></param>
    private void Receive_Callback(IAsyncResult ar)
    {
        StateObject stateObject = (StateObject)ar.AsyncState;

        int read = stateObject.socket.EndReceive(ar);

        if (read > 0)
        {
            this._recStr = Encoding.UTF8.GetString(stateObject.buffer, 0, read);
            MyDebug.Log(">>>接收客户端信息:" + this._recStr, MyColor.State.yellow);
            try
            {
                stateObject.socket.BeginReceive(stateObject.buffer, 0, BUFFER_SIZE, 0, new AsyncCallback(Receive_Callback), stateObject);
            }
            catch (Exception e)
            {
                MyDebug.Log(e.Message,MyColor.State.red);
            }
        }
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message"></param>
    public void Send(string message)
    {
        MyDebug.Log(">>>向客户端发送:" + message, MyColor.State.green);
        byte[] msg = Encoding.UTF8.GetBytes(message);
        foreach (var item in clientSockets)
        {
            if (item.Connected)
            {
                try
                {
                    item.Send(msg, msg.Length, 0);
                }
                catch (Exception e)
                {
                    MyDebug.Log(e.Message,MyColor.State.red);
                }
            }
        }
    }
    /// <summary>
    /// 关闭服务器
    /// </summary>
    public void CloseSocket()
    {
        if (serverSocket.Connected)
        {
            try
            {
                MyDebug.Log(">>>关闭服务器", MyColor.State.blue);
                serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();
            }
            catch (Exception e)
            {
                MyDebug.Log(e.Message,MyColor.State.red);
            }
        }
    }
}
public class StateObject
{
    public byte[] buffer;
    public Socket socket;

    public StateObject(int size, Socket socket)
    {
        buffer = new byte[size];
        this.socket = socket;
    }
}
