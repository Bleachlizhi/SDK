using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyToolsSetting
{
    public static class RayTools
    {
        /// <summary>
        /// 相机射线返回物体
        /// </summary>
        /// <returns></returns>
        public static GameObject GetObjectFromCamera()
        {
            Ray ray = new Ray();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                DrawLine(Camera.main.transform.position, hitInfo.point);
                return hitInfo.collider.gameObject;
            }
            return null;
        }
        /// <summary>
        /// 相机射线返回位子
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetVector3FromCamera()
        {
            Ray ray = new Ray();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                DrawLine(Camera.main.transform.position, hitInfo.point);
                return hitInfo.point;
            }
            return Vector3.zero;
        }
        /// <summary>
        /// 物体射线返回位子
        /// </summary>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 GetVector3FromTransform(Vector3 target, Vector3 direction)
        {
            Ray ray = new Ray(target, direction);
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                DrawLine(target, hitInfo.point);
                return hitInfo.point;
            }
            return Vector3.zero;
        }
        /// <summary>
        /// 物体射线返回物体
        /// </summary>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static GameObject GetGameObjectFromTransform(Vector3 target, Vector3 direction)
        {
            Ray ray = new Ray(target, direction);
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                DrawLine(target, hitInfo.point);
                return hitInfo.collider.gameObject;
            }
            return null;
        }
        private static void DrawLine(Vector3 start,Vector3 end)
        {
            Debug.DrawLine(start,end,Color.red);
        }
    }
}

