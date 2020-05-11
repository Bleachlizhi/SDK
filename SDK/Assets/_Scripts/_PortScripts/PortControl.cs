using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.IO;
using LitJson;
using MyToolsSetting;
public class PortControl : MonoBehaviour
{
    private enum Port
    {
        COM1,
        COM2,
        COM3,
        COM4,
        COM5
    }
    #region 定义串口属性
    //定义基本信息
    [SerializeField,Header("模拟串口")]
    private Port m_port;
    private byte[] buffer = new byte[1];
    public Parity parity = Parity.None;//效验位
    public int dataBits = 8;//数据位
    public StopBits stopBits = StopBits.One;//停止位
    private SerialPort sp = null;
    private Thread dataReceiveThread;
    private SystemCommandOfSerialPort m_Command;
    //接受到的string信息
    private string strbytes = string.Empty;

    #endregion


    #region 开启串口，打开线程
    void Start()
    {
        OpenPort();
        m_Command = new SystemCommandOfSerialPort();
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
        dataReceiveThread.Start();
        ActionTools.AddListener<object>(MyEventName.test1, WriteObj);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            File.WriteAllBytes(Application.streamingAssetsPath + "test.txt", buffer);
        }
    }
    #endregion
    #region 创建串口，并打开串口
    public void OpenPort()
    {
# if UNITY_EDITOR
        sp = new SerialPort(m_port.ToString(), 230400, parity, dataBits, stopBits);
# elif UNITY_STANDALONE_WIN
        Title_Port title = new Title_Port();
        title =  Tool.ReadJson(title,Application.streamingAssetsPath + "/Info.json") as Title_Port;
        sp = new SerialPort(title.info.portName, title.info.baudRate, parity, dataBits, stopBits);
#endif
        sp.ReadTimeout = 400;
        try
        {
            sp.Open();
        }
        catch (Exception ex)
        {
            MyDebug.Log(ex.Message,MyColor.State.red);
        }
    }
    #endregion
    #region 程序退出时关闭串口
    void OnApplicationQuit()
    {
        ClosePort();
    }
    public void ClosePort()
    {
        try
        {
            sp.Close();
            dataReceiveThread.Abort();
        }
        catch (Exception ex)
        {
            MyDebug.Log(ex.Message, MyColor.State.red);
        }
    }
    #endregion
    #region 接收数据
    void DataReceiveFunction()
    {
        #region 按字节数组发送处理信息，信息缺失
        int bytes = 0;
        while (true)
        {
            if (sp != null && sp.IsOpen)
            {
                try
                {
                    bytes = sp.Read(buffer, 0, buffer.Length);//接收字节
                    if (bytes == 0)
                    {
                        continue;
                    }
                    else
                    {
                        strbytes = Encoding.Default.GetString(buffer);
                        Debug.Log(strbytes);
                        ActionTools.Broadcast( MyEventName.test3, strbytes);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.GetType() != typeof(ThreadAbortException))
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            else
            {
                MyDebug.Log("端口异常",MyColor.State.red);
            }
          
            Thread.Sleep(100);
        }
        #endregion
    }
    #endregion
    #region 发送数据
    public void WriteByte(byte[] buffer)
    {
        if (sp.IsOpen)
        {
            sp.Write(buffer, 0, buffer.Length);
        }
    }
    public void WriteObj(object message)
    {
        if (sp.IsOpen)
        {
            sp.Write(message.ToString());
        }
    }
    #endregion
}
#region 串口信息列表
[System.Serializable]
public class Title_Port
{
    public Info_Port info;
}
[System.Serializable]
public class Info_Port
{
    public  string portName;//串口名
    public  int baudRate = 115200;//波特率
}
#endregion
#region 串口的系统命令
[System.Serializable]
public class SystemCommandOfSerialPort
{
    public byte[] Open = new byte[] { 0xA5, 0x60 };
    public byte[] Close = new byte[] { 0xA5, 0x65 };
    public byte[] Restart = new byte[] { 0xA5, 0x80 };
}
#endregion