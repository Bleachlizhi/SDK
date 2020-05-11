using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace MyToolsSetting
{
    public static class Tool
    {

        /// <summary>
        /// 解析Json返回类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T ReadJson<T>(T t, string path)
        {
            StreamReader streamReader = new StreamReader(path, Encoding.UTF8);
            t = JsonMapper.ToObject<T>(streamReader.ReadToEnd());
            return t;
        }
        /// <summary>
        /// 写入json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="path"></param>
        public static void WriteJson<T>(T t, string path)
        {
            string str = JsonMapper.ToJson(t);
            File.WriteAllText(path, str, Encoding.UTF8);
        }
        /// <summary>
        /// 字符串转Vector3
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static Vector3 Parse(string str)
        {
            str = str.Replace("(", "").Replace(")", "");
            string[] strs = str.Split(',');
            return new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]));
        }
        /// <summary>
        /// byte[]转十六进制的字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ByteToHexString(byte[] buffer)
        {
            StringBuilder strB = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                strB.Append(buffer[i].ToString("X2"));
            }
            return strB.ToString();
        }
        /// <summary>
        /// byte[]转字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] buffer)
        {
            return System.Text.ASCIIEncoding.Default.GetString(buffer);
        }
        /// <summary>
        /// 字符串转byte
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string str)
        {
            return System.Text.ASCIIEncoding.Default.GetBytes(str);
        }
        /// <summary>
        /// 十六进制字符串转byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// 十六进制字符串转字符串
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            return encode.GetString(b);
        }
        /// <summary>
        /// 字符串转十六进制的字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }
    }

}