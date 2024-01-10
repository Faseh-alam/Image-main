using System;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class OpenCam : MonoBehaviour
{
    // Start is called before the first frame update
    private WebCamTexture webCamTexture;
    private bool isCapturing = false;
    // Start is called before the first frame update
    void Start() {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
        // Start the camera feed
        webCamTexture = new WebCamTexture();
        GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
        webCamTexture.Play();
    }
    public void Photo() {
        CapturePhoto();
    }

    // Update is called once per frame
    //void Update() {
    //    // Check for input to capture a photo
    //    if (Input.GetKeyDown(KeyCode.Space) && !isCapturing) {
    //        isCapturing = true;
    //        CapturePhoto();
    //    }
    //}

    void CapturePhoto() {
        // Capture a frame from the camera
        SceneManager.Instance.debugTextFile.text = "Capturing photo";
        Texture2D photoTexture = new Texture2D(webCamTexture.width, webCamTexture.height);
        photoTexture.SetPixels(webCamTexture.GetPixels());
        photoTexture.Apply();
        // Save the photo to the device's storage
        byte[] photoBytes = photoTexture.EncodeToPNG();
        SceneManager.Instance. path = Application.dataPath + "/capturedPhoto.png";
        File.WriteAllBytes(SceneManager.Instance.path, photoBytes);
        Debug.Log(SceneManager.Instance.path);
       // imageModeration.imageUrl = hotoBytes;
        // Import the photo as a Texture or Sprite
        // Here, you can use photoTexture as a material's mainTexture
        Renderer renderer = GetComponent<Renderer>();
        SceneManager.Instance.debugTextFile.text = "Sending Photon";
        renderer.material.mainTexture = photoTexture;
       //SceneManager.Instance.imageModeration.ChkImage();
        isCapturing = false;
    }

}

