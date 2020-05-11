using UnityEngine;
using System.Collections;
/// <summary>
/// 鼠标视角操作
/// </summary>
public class MouseOrbit : MonoBehaviour
{
    //目标
    public GameObject target;
    //摄像机初始角度
    public Vector3 cameraPos;
    //XY轴的速度
    public float xSpeed;   
    public float ySpeed; 
    //XY轴角度限制
    public float yMinLimit;  
    public float yMaxLimit;  
    public float xMinLimit;
    public float xMaxLimit;
    //鼠标缩放速度，距离限制
    public float scrollSpeed;  
    public float zoomMin;  
    public float zoomMax;  


    private float distance;
    private float distanceLerp;
    private Vector3 position;
    /// <summary>
    /// 是否旋转摄像机
    /// </summary>
    private bool isRota;
    private float x;
    private float y;

    // Use this for initialization  
    void Start()
    { 
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");

            if (target == null)
            {
                Debug.LogWarning("找不到目标标签，请更换标签");
            }
        }
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        distance = zoomMax;
    }
    void Awake()
    {
        setInitPos();
    }
    void LateUpdate()
    {
        ScrollMouse();
        RotateCamera();
    }

    //摄像机旋转 
    void RotateCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRota = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRota = false;
        }

        if (isRota)
        {
            y -= Input.GetAxis("Mouse Y") * ySpeed;
           
            x += Input.GetAxis("Mouse X") * xSpeed;
           
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            x = ClampAngle(x, xMinLimit, xMaxLimit);
        }
        CalDistance();
    }
    void CalDistance()
    {
        distanceLerp = distance;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 calPos = new Vector3(0, 0, -distanceLerp);
        position = rotation * calPos + target.transform.position;
        transform.rotation = rotation;
        transform.position = position;
    }
    //重置摄像机的位置
    public void setInitPos() {
        distance = zoomMax;
        Vector3 angles = transform.eulerAngles;
        transform.position = cameraPos;
        x = angles.y;
        y = angles.x;

    }
    //摄像机缩放
    void ScrollMouse()
    {
        distanceLerp = Mathf.Lerp(distanceLerp, distance, Time.deltaTime * 5);

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            distance = Mathf.Clamp(distance, zoomMin, zoomMax);
            Vector3 position = transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.position = position;

        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
