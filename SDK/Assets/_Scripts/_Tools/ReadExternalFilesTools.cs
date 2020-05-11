using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using MyToolsSetting;
using UnityEngine.Networking;
namespace MyToolsSetting
{
    public static class ReadExternalFilesTools
    {
        //public static string m_PhotoPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/图片";

        private static UnityWebRequest www;
        /// <summary>
        /// 查找创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public static bool CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                MyDebug.Log("没有该文件夹", MyColor.State.green);
                Directory.CreateDirectory(path);
                return false;
            }
            MyDebug.Log("该文件夹已找到", MyColor.State.green);
            return true;
        }
        /// <summary>
        /// 查找文件是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool FindFiles(string name)
        {
            if (File.Exists(name))
            {
                MyDebug.Log("该文件已找到", MyColor.State.green);
                return true;
            }
            MyDebug.Log("没有该文件", MyColor.State.green);
            return false;
        }
        /// <summary>
        /// 获取文件夹下的文件夹名字
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FindFileName(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
                return null;
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            return fileInfos[0].Name.Replace(".meta", "");
        }
        /// <summary>
        /// 获取文件夹中的所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        public static List<string> FindAllFiles(string path, string postfix)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
                return null;
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            List<string> list = new List<string>();
            foreach (var item in fileInfos)
            {
                if (item.FullName.Contains(postfix) && !item.FullName.Contains(".meta"))
                {
                    list.Add(item.Name);
                }
            }
            return list;
        }
        /// <summary>
        /// 读取文本并下载
        /// </summary>
        public static void ReadAndDownLoadTXT(MonoBehaviour mono, string path, string name, Action<string> ac)
        {
            if (!CreateFolder(path))
                return;
            if (!FindFiles(path + "/" + name))
                return;
            mono.StartCoroutine(DownLoadTXT(path + "/" + name, ac));

        }
        /// <summary>
        /// 读取图片并下载
        /// </summary>
        public static void ReadAndDownLoadPNG(MonoBehaviour mono, string path, string name, Action<Texture> ac)
        {
            if (!CreateFolder(path))
                return;
            if (!FindFiles(path + "/" + name))
                return;
            mono.StartCoroutine(DownLoadPNG(path + "/" + name, ac));
        }
        private static Texture2D PNG;
        private static IEnumerator DownLoadPNG(string url, Action<Texture> ac)
        {
            MyDebug.Log("下载中。。。Url:" + url, MyColor.State.blue);
            www = UnityWebRequest.Get(url);
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            www.downloadHandler = downloadTexture;
            yield return www.Send();
            if (www.error != null)
            {
                MyDebug.Log(www.error, MyColor.State.red);
            }
            else
            {
                MyDebug.Log("下载中。。。Url:" + url, MyColor.State.blue);
                PNG = downloadTexture.texture;
                MyDebug.Log(PNG.name, MyColor.State.yellow);
                ac(PNG);
            }
        }
        private static string TXT;
        private static IEnumerator DownLoadTXT(string url, Action<string> ac)
        {
            MyDebug.Log("下载中。。。Url:" + url, MyColor.State.blue);
            www = UnityWebRequest.Get(url);
            yield return www.Send();
            if (www.error != null)
            {
                MyDebug.Log(www.error, MyColor.State.red);
            }
            else
            {
                MyDebug.Log("下载中。。。Url:" + url, MyColor.State.blue);
                MyDebug.Log("下载中。。。Url:" + url, MyColor.State.blue);
                DownloadHandler downloadHandler_TXT = www.downloadHandler;
                TXT = downloadHandler_TXT.text;
                MyDebug.Log(TXT, MyColor.State.yellow);
                ac(TXT);
            }
        }
    }
}
