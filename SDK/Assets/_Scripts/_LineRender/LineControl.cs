using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineControl : MonoBehaviour {
    private Color paintColor = Color.red;
    private float paintSize = 0.1f;
    private LineRenderer currentLine;
    public Material lineMaterial;
    private List<Vector3> positions = new List<Vector3>();
    private bool isMouseDown = false;
    private Vector3 lastMousePostion = Vector3.zero;
    private float lineDistance = 0.02f;

    private void Start()
    {
        OnPoint4Changed(true);
        OnGreenColorChanged(true);
    }
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            GameObject go = new GameObject();
            go.transform.SetParent(this.transform);
            currentLine = go.AddComponent<LineRenderer>();
            currentLine.material = lineMaterial;
            currentLine.startWidth = paintSize;
            currentLine.endWidth = paintSize;
            currentLine.startColor = paintColor;
            currentLine.endColor = paintColor;
            currentLine.numCornerVertices = 5;
            currentLine.numCapVertices = 5;
            Vector3 position = GetMousePoint();

            AddPosition(position);
            isMouseDown = true;
            lineDistance += 0.02f;
        }
        if (isMouseDown)
        {
            Vector3 position = GetMousePoint();
            if (Vector3.Distance(position, lastMousePostion) > 0.1f)
                AddPosition(position);
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentLine = null;
            positions.Clear();
            isMouseDown = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Reset();
        }
    }
    #region 添加路径点
    void AddPosition(Vector3 position)
    {
        position.z -= lineDistance;
        positions.Add(position);
        currentLine.positionCount = positions.Count;
        currentLine.SetPositions(positions.ToArray());
        lastMousePostion = position;
    }
    #endregion
    #region 获取鼠标点击位子
    Vector3 GetMousePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isCollider = Physics.Raycast(ray, out hit);
        if (isCollider)
        {
            return hit.point;
        }
        return Vector3.zero;
    }
    #endregion
    #region 画笔设置
    public void OnRedColorChanged(bool isOn)
    {
        if (isOn)
        {
            paintColor = Color.red;
        }
    }
    public void OnGreenColorChanged(bool isOn)
    {
        if (isOn)
        {
            paintColor = Color.green;
        }
    }
    public void OnBlueColorChanged(bool isOn)
    {
        if (isOn)
        {
            paintColor = Color.blue;
        }
    }
    #endregion
    /// <summary>
    /// 切换画笔的大小
    /// </summary>
    /// <param name="isOn"></param>
    #region
    public void OnPoint1Changed(bool isOn)
    {
        if (isOn)
        {
            paintSize = 0.1f;
        }
    }
    public void OnPoint2Changed(bool isOn)
    {
        if (isOn)
        {
            paintSize = 0.2f;
        }
    }
    public void OnPoint4Changed(bool isOn)
    {
        if (isOn)
        {
            paintSize = 0.4f;
        }
    }
    #endregion
    #region 重新签名
    public void Reset()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }
    #endregion
}
