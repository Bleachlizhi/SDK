using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraContro : MonoBehaviour {

    WebCamTexture _webCamera;
    public string DeviceName;
    [SerializeField]
    private RawImage Plane;
    private void Start()
    {
        StartCoroutine(InitCameraCor());
    }
    public IEnumerator InitCameraCor()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].name == "Creative GestureCam")
                    DeviceName = devices[i].name;
            }

            _webCamera = new WebCamTexture(DeviceName, 640, 480, 30);

            Plane.texture = _webCamera;

            _webCamera.Play();

        }
    } 
}
