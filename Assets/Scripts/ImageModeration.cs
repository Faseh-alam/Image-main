using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using UnityEngine.UI;
using System.IO;
[System.Serializable]
public class SafeSearchAnnotation
{
    public string adult;
    public string spoof;
    public string medical;
    public string violence;
    public string racy;
}

[System.Serializable]
public class Response
{
    public SafeSearchAnnotation safeSearchAnnotation;
}

[System.Serializable]
public class ApiResponse
{
    public Response[] responses;
}
public class ImageModeration : MonoBehaviour
{
    private string apiKey = "AIzaSyD5SpH6agDBqsSAUflLqZ-RaEiWVwCKdZ4";
    private string visionApiUrl = "https://vision.googleapis.com/v1/images:annotate?key=";
    public Text Output;


    public void ChkImage()
    {
        StartCoroutine(MakeVisionApiRequest(SceneManager.Instance.path));
    }

    private IEnumerator MakeVisionApiRequest(string imagePath)
    {
        Debug.Log("Started");
        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
        string base64Image = System.Convert.ToBase64String(imageBytes);

        string jsonRequest = @"
        {
            'requests': [
                {
                    'image': {
                        'content': '" + base64Image + @"'
                    },
                    'features': [
                        {
                            'type': 'SAFE_SEARCH_DETECTION'
                        }
                    ]
                }
            ]
        }";

        using (UnityWebRequest www = new UnityWebRequest(visionApiUrl + apiKey, "POST"))
        {
            byte[] jsonRaw = new System.Text.UTF8Encoding().GetBytes(jsonRequest);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(jsonResponse);
                // Access values
                SafeSearchAnnotation safeSearchAnnotation = apiResponse.responses[0].safeSearchAnnotation;
                
                Debug.Log(jsonResponse);

                switch (safeSearchAnnotation.racy)
                {
                    case "LIKELY":
                        Debug.Log("Adult Content Found!!!!");

                        Output.text = "Adult Content Found!!!!";
                        Output.color = Color.red;
                        // Code for likely case
                        break;

                    case "VERY_LIKELY":
                        Debug.Log("Adult Content Found!!!!");
                        Output.text = "Adult Content Found!!!!";
                        Output.color = Color.red;
                        // Code for very likely case
                        break;
                    case "POSSIBLE":
                        Debug.Log("Adult Content Found!!!!");
                        Output.text = "Adult Content Found!!!!";
                        Output.color = Color.red;
                        // Code for very likely case
                        break;



                    default:
                        Debug.Log("content is safe for kids");
                        Output.text = "Content is safe for Kids";
                        Output.color = Color.green;
                        // Code for handling other cases
                        break;
                }
               
            }
        }
    }
    
}
