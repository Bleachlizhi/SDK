using System.Net.Sockets;


using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

//添加的命名空间引用
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;


using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
public class Soft_加密 : MonoBehaviour
{

    protected static Soft_加密 instance = null;
    public static Soft_加密 Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        // set the singleton instance
        //instance = this;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }
    }




    public int g_试用日期_A = 20190307;
    public int g_试用日期_B = 20190316;

    public bool g_locked = true;
    public int g_lock_date = 0;
    public Dictionary<int, int> g_allready_passed_dates = new Dictionary<int, int>();
    public List<string> g_lock_files = new List<string>();

    public int get_day_str(int off_day)
    {
        DateTime now = DateTime.Now.AddDays(off_day);
        int year = (int)now.Year;
        int month = (int)now.Month;
        int day = (int)now.Day;
        string now_date = year.ToString() + month.ToString("d2") + day.ToString("d2");
        return int.Parse(now_date);
    }

    public bool test_leagle_file(string file)
    {

        bool file_exist = false;
        int today = get_day_str(0);

        XmlDocument doc = new XmlDocument();
        try
        {
            doc.Load(file);
            file_exist = true;
            //print("------------------file ok");
        }
        catch
        {// 不存在就创建一个
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //创建一个根节点（一级）
            XmlElement root = doc.CreateElement("game");
            doc.AppendChild(root);
            //创建节点（二级）
            XmlElement lock_node = doc.CreateElement("lock_date");
            XmlElement day_node = doc.CreateElement("used_date");


            lock_node.SetAttribute("date", get_day_str(-10).ToString());
            day_node.SetAttribute("dates", get_day_str(0).ToString());

            root.AppendChild(lock_node);
            root.AppendChild(day_node);
            // 如果文件夹不存在  创建文件夹
            Directory.CreateDirectory(file.Substring(0, file.LastIndexOf('/') + 1));
            doc.Save(file);

            file_exist = false;
            //print("------------------file error and create a new one");
        }

        XmlNode game_xn = doc.SelectSingleNode("game");

        XML_Element lock_xe = new XML_Element((XmlElement)game_xn.SelectSingleNode("lock_date"));
        int lock_date = lock_xe.f_int("date");

        XML_Element used_day_xe = new XML_Element((XmlElement)game_xn.SelectSingleNode("used_date"));
        List<int> used_days = used_day_xe.f_int_vector("dates");

        bool today_is_leagle = true;
        for (int i = 0; i < used_days.Count(); i++)
        {
            if (today < used_days[i])
            {// 这个日期比  当前日期要大   说明甲方改系统时间了
                today_is_leagle = false;
            }

        }
        if (today < lock_date)
        {
            //print("------------------1：today<锁码日期，    ");

            //return true;
        }
        if (today_is_leagle == true)
        {
            //print("------------------  2：today的日期是没有修改过的    ");

            //return true;
        }
        if (file_exist == true)
        {
            //print("------------------  3：文件是存在的");

            //return true;
        }

        // true 的条件   1：today小于锁码日期，    2：today的日期是没有修改过的    3：文件是存在的
        if (today < lock_date && today_is_leagle == true && file_exist == true)
        {
            //print("------------------1：today<锁码日期，    2：today的日期是没有修改过的    3：文件是存在的");

            return true;
        }
        else
        {
            //print("------------------ error");
            return false;

        }
    }


    public void read_dates(string file, ref int lock_day, ref Dictionary<int, int> allready_passed_dates)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode game_xn = doc.SelectSingleNode("game");

            XML_Element lock_xe = new XML_Element((XmlElement)game_xn.SelectSingleNode("lock_date"));
            int lock_date = lock_xe.f_int("date");

            XML_Element used_day_xe = new XML_Element((XmlElement)game_xn.SelectSingleNode("used_date"));
            List<int> used_days = used_day_xe.f_int_vector("dates");

            if (lock_day == 0)
            {
                lock_day = lock_date;
            }

            lock_day = lock_day < lock_date ? lock_day : lock_date;
            for (int i = 0; i < used_days.Count(); i++)
            {
                allready_passed_dates[used_days[i]] = used_days[i];
            }

        }
        catch
        {// 
        }

    }

    // Use this for initialization
    void Start()
    {
        try
        {
            g_lock_files.Add("my_data/ds.xml");
            g_lock_files.Add("c:/bm3/ds.xml");
            //g_lock_files.Add("d:/0574/ds.xml");


            bool is_all_leagle = true;
            for (int i = 0; i < g_lock_files.Count(); i++)
            {
                if (!test_leagle_file(g_lock_files[i]))
                {
                    is_all_leagle = false;
                    break;
                }
            }


            if (is_all_leagle)
            {
                 print("------------------is_all_leagle: true");

                for (int i = 0; i < g_lock_files.Count(); i++)
                {
                    read_dates(g_lock_files[i], ref g_lock_date, ref g_allready_passed_dates);
                }
            }
            else
            {
                 print("------------------is_all_leagle: false");

                g_allready_passed_dates.Clear();
                g_allready_passed_dates[get_day_str(0)] = get_day_str(0);
                g_lock_date = get_day_str(0) - 10;
            }

            // 重新保存数据文件
            save_xml();
            is_locked();

            if (is_all_leagle)
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
            }
            else
            {
                g_show_secret_gui = !g_show_secret_gui;
                // 重新计算sn
                get_sn();
                g_secret_code = "";

                if (g_show_secret_gui)
                {
                    UnityEngine.Cursor.visible = true;
                }
                else
                {
                    UnityEngine.Cursor.visible = false;
                }
            }
        }
        catch
        {
        }
    }

    public string g_sn = "12345677890";
    public string g_secret_code = "1234567890";
    public bool g_show_secret_gui = false;
    public string get_sn()
    {
        DateTime new_day = DateTime.Now;
        int year = (int)new_day.Year;
        int month = (int)new_day.Month;
        int day = (int)new_day.Day;
        int hour = (int)new_day.Hour;
        int minute = (int)new_day.Minute;
        int second = (int)new_day.Second;
        int mili = (int)new_day.Millisecond;

        int val =
            mili +
            UnityEngine.Random.Range(1111, 3555);

        g_sn = val.ToString();

        return g_sn;
    }
    // 测试密码 密钥
    public bool test_code(string code)
    {
        if (code.Length >= 0)
        {
            try
            {
                //  格式为 code = (天数*10000+sn+1234321)*13
                int key = int.Parse(code);
                print("key:" + key);
                int day_num = (key / 13 - 1234321) / 10000;
                int decode = (key / 13 - 1234321) % 10000;
                int sn_int = int.Parse(g_sn);


                print("-------day_num:  " + day_num);
                print("-------decode:  " + decode);
                if (sn_int == decode)
                {
                    clear_used_days_and_new_day_num(day_num);
                    return true;
                }
                else
                {
                    clear_used_days_and_new_day_num(0);
                    //解码失败
                    return false;
                }
            }
            catch
            {
                print("-------code:  格式不对");
                clear_used_days_and_new_day_num(0);
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    public void save_xml()
    {// 重新保存数据文件

        XmlDocument doc = new XmlDocument();
        XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
        doc.AppendChild(dec);
        //创建一个根节点（一级）
        XmlElement root = doc.CreateElement("game");
        doc.AppendChild(root);
        //创建节点（二级）
        XML_Element lock_node_xe = new XML_Element(doc.CreateElement("lock_date"));
        XML_Element day_node_xe = new XML_Element(doc.CreateElement("used_date"));


        List<int> days = new List<int>();
        foreach (int key in g_allready_passed_dates.Keys)
        {
            days.Add(key);
        }


        lock_node_xe.SetAttribute("date", g_lock_date);
        day_node_xe.SetAttribute("dates", days);

        root.AppendChild(lock_node_xe.m_xe);
        root.AppendChild(day_node_xe.m_xe);

        for (int i = 0; i < g_lock_files.Count(); i++)
        {
            doc.Save(g_lock_files[i]);
        }
    }

    public bool g_is_试用期内 = false;

    public bool is_locked()
    {
        if (get_day_str(0) <= g_lock_date)
        {
            g_locked = false;
            foreach (int key in g_allready_passed_dates.Keys)
            {
                if (key > get_day_str(0))
                {// 说明这时间之前用过了 ，  修改了系统时间， 不允许。						
                    g_locked = true;
                }
            }
        }
        else
        {
            g_locked = true;
        }


        if (g_试用日期_A <= get_day_str(0) && get_day_str(0) <= g_试用日期_B)
        {
            if (g_locked == true)
            {
                // 试用期
                g_is_试用期内 = true;
                //print("g_is_试用期内 = true");
            }
            else
            {
                g_is_试用期内 = false;
                //print("g_is_试用期内 = false");
            }


            g_locked = false;
        }

        if (g_locked)
        {
            //print("g_locked == true" );
        }
        else
        {
            //print("g_locked == false");
        }


        return g_locked;
    }

    // 设置一个新的锁定时间
    public void set_new_lock_day(int new_lock_day)
    {
        g_lock_date = new_lock_day;
        // 重新保存数据文件
        save_xml();
        is_locked();
    }

    // 清除使用时间
    public void clear_used_days_and_new_day_num(int day_num = 30)
    {
        DateTime new_day = DateTime.Now.AddDays(day_num);
        int year = (int)new_day.Year;
        int month = (int)new_day.Month;
        int day = (int)new_day.Day;
        string new_day_str = year.ToString() + month.ToString("d2") + day.ToString("d2");

        g_lock_date = int.Parse(new_day_str);
        g_allready_passed_dates.Clear();
        g_allready_passed_dates[get_day_str(0)] = get_day_str(0);

        // 重新保存数据文件
        save_xml();
        is_locked();
    }

    void OnGUI()
    {
        to_draw();
    }


    float g_t = 0;


    public GameObject g_演示;

    public void to_draw()
    {


        try
        {
            if (is_locked())
            {
                GUILayout.BeginArea(new Rect(100, 100, Screen.width - 200, Screen.height - 200));
                GUILayout.BeginVertical("box", GUILayout.MinWidth(Screen.width - 200));
                GUILayout.Space(Screen.height - 200);
                GUILayout.EndVertical();
                GUILayout.EndArea();

                GUILayout.BeginArea(new Rect(200, 200, Screen.width - 400, Screen.height - 400));
                GUILayout.BeginVertical("box", GUILayout.MinWidth(Screen.width - 400));
                GUILayout.Space(Screen.height - 400);
                GUILayout.EndVertical();
                GUILayout.EndArea();

                GUILayout.BeginArea(new Rect(100, 100, Screen.width - 200, Screen.height - 200));
                GUILayout.BeginVertical("box", GUILayout.MinWidth(Screen.width - 200));
                GUILayout.Space(Screen.height - 200);
                GUILayout.EndVertical();
                GUILayout.EndArea();

                GUILayout.BeginArea(new Rect(200, 200, Screen.width - 400, Screen.height - 400));
                GUILayout.BeginVertical("box", GUILayout.MinWidth(Screen.width - 400));
                GUILayout.Space(Screen.height - 400);
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }

            if (g_locked == false && g_is_试用期内 == true)
            {
                //GUI.TextArea(new Rect(100,100,200,200),"demo");
                g_演示.active = true;

            }
            else
            {
                g_演示.active = false;
            }
        }
        catch
        {
        }

        if (g_show_secret_gui == true)
        {// 密码保护

            GUILayout.BeginArea(new Rect(150, 150, 700, 800));

            GUILayout.BeginVertical("box", GUILayout.MinWidth(700));

            GUILayout.Space(16f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(200f);
            GUILayout.Label("date_limit", GUILayout.Width(60));
            GUILayout.EndHorizontal();
            GUILayout.Space(16f);

            GUILayout.BeginHorizontal();
            GUIStyle style = new GUIStyle();
            style.fontSize = 40;
            style.normal.textColor = new Color(1, 1, 1);
            GUILayout.Label("     sn:", style, GUILayout.Width(120));
            GUILayout.Label(g_sn, style, GUILayout.MinWidth(256f));
            GUILayout.EndHorizontal();
            GUILayout.Space(16f);

            GUILayout.BeginHorizontal();
            GUILayout.Label(" code:  ", style, GUILayout.Width(120));
            g_secret_code = GUILayout.TextField(g_secret_code, 192, style, GUILayout.MinWidth(256f));
            GUILayout.EndHorizontal();
            GUILayout.Space(16f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(64f);
            if (GUILayout.Button("ok", GUILayout.Width(100), GUILayout.Height(100)))
            {
                if (g_t > 1f)
                {
                    g_t = 0;

                    if (test_code(g_secret_code))
                    {
                        g_secret_code = "code ok";
                        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
                        //g_show_secret_gui = false;
                    }
                    else
                    {
                        g_secret_code = "code error !!!";
                        //g_show_secret_gui = false;
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(16f);
            GUILayout.Space(16f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(64f);
            if (GUILayout.Button("1", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "1";
            GUILayout.Space(16f);
            if (GUILayout.Button("2", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "2";
            GUILayout.Space(16f);
            if (GUILayout.Button("3", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "3";
            GUILayout.Space(16f);
            if (GUILayout.Button("←", GUILayout.Width(96), GUILayout.Height(64))) g_secret_code = g_secret_code.Substring(0, g_secret_code.Length - 1);
            GUILayout.Space(16f);
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);


            GUILayout.BeginHorizontal();
            GUILayout.Space(64f);
            if (GUILayout.Button("4", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "4";
            GUILayout.Space(16f);
            if (GUILayout.Button("5", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "5";
            GUILayout.Space(16f);
            if (GUILayout.Button("6", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "6";
            GUILayout.Space(16f);
            if (GUILayout.Button("clear", GUILayout.Width(96), GUILayout.Height(64))) g_secret_code = "";
            GUILayout.Space(16f);
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(64f);
            if (GUILayout.Button("7", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "7";
            GUILayout.Space(16f);
            if (GUILayout.Button("8", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "8";
            GUILayout.Space(16f);
            if (GUILayout.Button("9", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "9";
            GUILayout.Space(16f);
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(64f);
            if (GUILayout.Button("A", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "A";
            GUILayout.Space(16f);
            if (GUILayout.Button("0", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "0";
            GUILayout.Space(16f);
            if (GUILayout.Button("B", GUILayout.Width(64), GUILayout.Height(64))) g_secret_code += "B";
            GUILayout.Space(16f);
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);



            GUILayout.EndVertical();
            GUILayout.EndArea();



            //if (Encode_生成二维码.Instance.m_EncodedTex)
            //{
            //    int size = 300;
            //    GUI.DrawTexture(new Rect(Screen.width / 2 - size / 2, Screen.height / 2 - size / 2, 300, 300), Encode_生成二维码.Instance.m_EncodedTex);
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {

        g_t += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            g_show_secret_gui = !g_show_secret_gui;
            // 重新计算sn
            get_sn();
            g_secret_code = "";

           // Encode_生成二维码.Instance.Encode(g_sn,null);

            if (g_show_secret_gui)
            {
                UnityEngine.Cursor.visible = true;
            }
            else
            {
                UnityEngine.Cursor.visible = false;
            }

        }

    }
}
public class XML_Element
{
    public XML_Element(XmlElement xe)
    {
        m_xe = xe;
    }

    public string f_str(string key)
    {
        return m_xe.GetAttribute(key);
    }
    public bool f_bool(string key)
    {
        bool b = false;
        try
        {
            b = bool.Parse(m_xe.GetAttribute(key));
        }
        catch
        {
        }
        return b;
    }
    public int f_int(string key)
    {
        int i = 0;
        try
        {
            i = int.Parse(m_xe.GetAttribute(key));
        }
        catch
        {
        }
        return i;
    }
    public float f_float(string key)
    {
        float f = 0;
        try
        {
            f = float.Parse(m_xe.GetAttribute(key));
        }
        catch
        {
        }
        return f;
    }
    public double f_double(string key)
    {
        double d = 0;
        try
        {
            d = double.Parse(m_xe.GetAttribute(key));
        }
        catch
        {
        }
        return d;
    }
    public List<string> f_str_vector(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));
        return sv;
    }
    public List<bool> f_bool_vector(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        List<bool> bv = new List<bool>();
        for (int i = 0; i < sv.Count; i++)
        {
            bv.Add(bool.Parse(sv[i]));
        }

        return bv;
    }
    public List<int> f_int_vector(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        List<int> iv = new List<int>();
        for (int i = 0; i < sv.Count; i++)
        {
            iv.Add(int.Parse(sv[i]));
        }

        return iv;
    }
    public List<float> f_float_vector(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        List<float> fv = new List<float>();
        for (int i = 0; i < sv.Count; i++)
        {
            fv.Add(float.Parse(sv[i]));
        }
        return fv;
    }
    public List<double> f_double_vector(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        List<double> dv = new List<double>();
        for (int i = 0; i < sv.Count; i++)
        {
            dv.Add(double.Parse(sv[i]));
        }
        return dv;
    }

    public List<Vector2> f_vec2_vector(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        List<Vector2> vv = new List<Vector2>();
        for (int i = 0; i < sv.Count - 1; i = i + 2)
        {
            vv.Add(new Vector2(float.Parse(sv[i]), float.Parse(sv[i + 1])));
        }
        return vv;
    }

    public List<Vector3> f_vec3_vector(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        List<Vector3> vv = new List<Vector3>();
        for (int i = 0; i < sv.Count - 2; i = i + 3)
        {
            vv.Add(new Vector3(float.Parse(sv[i]), float.Parse(sv[i + 1]), float.Parse(sv[i + 2])));
        }
        return vv;
    }
    public List<Vector4> f_vec4_vector(string key)
    {
        //print("key:" + key);
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        List<Vector4> vv = new List<Vector4>();
        for (int i = 0; i < sv.Count - 3; i = i + 4)
        {
            vv.Add(new Vector4(float.Parse(sv[i]), float.Parse(sv[i + 1]), float.Parse(sv[i + 2]), float.Parse(sv[i + 3])));
        }
        return vv;
    }
    public List<Color> f_color_vector(string key)
    {
        //print("key:" + key);
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        List<Color> vv = new List<Color>();
        for (int i = 0; i < sv.Count - 3; i = i + 4)
        {
            vv.Add(new Color(float.Parse(sv[i]), float.Parse(sv[i + 1]), float.Parse(sv[i + 2]), float.Parse(sv[i + 3])));
        }
        return vv;
    }


    public Vector2 f_vec2(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        Vector2 v = new Vector2();
        if (sv.Count > 0)
        {
            v.x = float.Parse(sv[0]);
        }
        if (sv.Count > 1)
        {
            v.y = float.Parse(sv[1]);
        }
        return v;
    }
    public Vector3 f_vec3(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        Vector3 v = new Vector3();
        if (sv.Count > 0)
        {
            v.x = float.Parse(sv[0]);
        }
        if (sv.Count > 1)
        {
            v.y = float.Parse(sv[1]);
        }
        if (sv.Count > 2)
        {
            v.z = float.Parse(sv[2]);
        }
        return v;
    }
    public Vector4 f_vec4(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        Vector4 v = new Vector4();
        if (sv.Count > 0)
        {
            v.x = float.Parse(sv[0]);
        }
        if (sv.Count > 1)
        {
            v.y = float.Parse(sv[1]);
        }
        if (sv.Count > 2)
        {
            v.z = float.Parse(sv[2]);
        }
        if (sv.Count > 3)
        {
            v.w = float.Parse(sv[3]);
        }
        return v;
    }
    public Color f_color(string key)
    {
        List<string> sv = this.get_sub_string_vector(m_xe.GetAttribute(key));

        Color v = new Color();
        if (sv.Count > 0)
        {
            v.r = float.Parse(sv[0]);
        }
        if (sv.Count > 1)
        {
            v.g = float.Parse(sv[1]);
        }
        if (sv.Count > 2)
        {
            v.b = float.Parse(sv[2]);
        }
        if (sv.Count > 3)
        {
            v.a = float.Parse(sv[3]);
        }
        return v;
    }


    public void SetAttribute(string key, string val)
    {
        m_xe.SetAttribute(key, val);
    }
    public void SetAttribute(string key, bool val)
    {
        m_xe.SetAttribute(key, val.ToString());
    }
    public void SetAttribute(string key, int val)
    {
        m_xe.SetAttribute(key, val.ToString());
    }
    public void SetAttribute(string key, float val)
    {
        m_xe.SetAttribute(key, val.ToString());
    }
    public void SetAttribute(string key, double val)
    {
        m_xe.SetAttribute(key, val.ToString());
    }


    public void SetAttribute(string key, List<string> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i];
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }

    public void SetAttribute(string key, List<bool> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i].ToString();
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }

    public void SetAttribute(string key, List<int> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i].ToString();
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }
    public void SetAttribute(string key, List<float> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i].ToString();
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }
    public void SetAttribute(string key, List<double> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i].ToString();
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }


    public void SetAttribute(string key, List<Vector2> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i].x.ToString();
            str += " ";
            str += val[i].y.ToString();
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }

    public void SetAttribute(string key, List<Vector3> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i].x.ToString();
            str += " ";
            str += val[i].y.ToString();
            str += " ";
            str += val[i].z.ToString();
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }

    public void SetAttribute(string key, List<Vector4> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i].x.ToString();
            str += " ";
            str += val[i].y.ToString();
            str += " ";
            str += val[i].z.ToString();
            str += " ";
            str += val[i].w.ToString();
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }
    public void SetAttribute(string key, List<Color> val)
    {
        string str = "";
        for (int i = 0; i < val.Count; i++)
        {
            str += val[i].r.ToString();
            str += " ";
            str += val[i].g.ToString();
            str += " ";
            str += val[i].b.ToString();
            str += " ";
            str += val[i].a.ToString();
            str += " ";
        }
        m_xe.SetAttribute(key, str);
    }


    public void SetAttribute(string key, Vector2 val)
    {
        string str = "";
        str += val.x.ToString();
        str += " ";
        str += val.y.ToString();
        m_xe.SetAttribute(key, str);
    }
    public void SetAttribute(string key, Vector3 val)
    {
        string str = "";
        str += val.x.ToString();
        str += " ";
        str += val.y.ToString();
        str += " ";
        str += val.z.ToString();
        m_xe.SetAttribute(key, str);
    }
    public void SetAttribute(string key, Vector4 val)
    {
        string str = "";
        str += val.x.ToString();
        str += " ";
        str += val.y.ToString();
        str += " ";
        str += val.z.ToString();
        str += " ";
        str += val.w.ToString();
        m_xe.SetAttribute(key, str);
    }
    public void SetAttribute(string key, Color val)
    {
        string str = "";
        str += val.r.ToString();
        str += " ";
        str += val.g.ToString();
        str += " ";
        str += val.b.ToString();
        str += " ";
        str += val.a.ToString();
        m_xe.SetAttribute(key, str);
    }



    public List<string> get_sub_string_vector(string str)
    {
        str = str.Replace(',', ' ');
        List<string> ss = new List<string>();
        while (str.Count() > 0)
        {
            while (str.IndexOf(' ') == 0)
            {
                str = str.Substring(str.IndexOf(' ') + 1);
            }
            if (str.Count() > 0)
            {
                string sub_str;
                if (!str.Contains(' '))
                {
                    sub_str = str;
                    str = "";
                }
                else
                {
                    sub_str = str.Substring(0, str.IndexOf(' '));
                    str = str.Substring(str.IndexOf(' ') + 1);

                    while (str.IndexOf(' ') == 0)
                    {
                        str = str.Substring(str.IndexOf(' ') + 1);
                    }
                }

                //if (sub_str.Contains(':'))
                //{
                //    if (sub_str.IndexOf(':') < sub_str.Count() - 1)
                //    {
                //        string sub_a = sub_str.Substring(0, sub_str.IndexOf(':') + 1);
                //        ss.Add(sub_a);
                //        sub_str = sub_str.Substring(sub_str.IndexOf(':') + 1);
                //    }
                //}
                ss.Add(sub_str);
            }

        }
        return ss;
    }

    public XmlElement m_xe;
};
