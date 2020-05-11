using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler {
    [SerializeField]
    private float Enter = 1.1f;
    [SerializeField]
    private float Exit = 1;
    [SerializeField]
    private float Down = .9f;
    [SerializeField]
    private float Up = 1;
    [SerializeField]
    private float timer = .1f;
    //
    // 参数:
    //   eventData:
    //     Current event data.
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, Vector3.one * Enter, timer);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, Vector3.one * Exit, timer);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, Vector3.one * Down, timer);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, Vector3.one * Up, timer);
    }
}
