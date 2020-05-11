using MyToolsSetting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIEvent : MonoBehaviour {

    public UnityEngine.UI.Button.ButtonClickedEvent OnClick;
    private Button _button;
	// Use this for initialization
    void Start () {
        this._button = this.GetComponent<Button>();
        this._button.onClick.AddListener(()=>OnClick.Invoke());
    }
	
    public void Play() 
    {
        Debug.Log("UIEvent测试");
    }

}
