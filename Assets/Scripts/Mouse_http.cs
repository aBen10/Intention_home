using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 
using SimpleJSON;
using System.IO;

public class Mouse_http : MonoBehaviour
{
    private const string Url = "http://10.0.1.36";
    private const string API_KEY = "6B9A6A475C594BF2B5B07338D8CBDF3E";
    private const string json = "application / json";
    GameObject mouse;
    private Vector3 pos;
    private float pos_y;
    private float last_y;
    private string valueOfPoition;
    private GameObject gcode;
    private string fileName = "Mouse.gcode";
    bool selected;

    //private bool toggle=false;  

    // Start is called before the first frame update
    void Start()
    {
        mouse = GameObject.Find("mouse");  
        selected=false;     
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckSelect(Url + "/api/job", "{\"job\""));
    }



    IEnumerator CheckSelect(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "GET");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("X-Api-Key", API_KEY);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        //if (uwr.isNetworkError) - Depreciated
        if(uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            //Debug.Log("Received: Printing" + uwr.downloadHandler.text);
            JSONNode status = JSON.Parse(uwr.downloadHandler.text);
            string name = status["job"]["file"]["name"];
            Debug.Log(name);
            if(name==fileName)
            {
                Debug.Log(name + " is selected");
                selected=true;
                mouse.SetActive(true);
            }
            else
            {
                Debug.Log("Not selected");
                selected=false;
                mouse.SetActive(false);
            }
        }
    }
}
