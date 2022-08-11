using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.IO;
using System;

public class OctoPrintFiles : MonoBehaviour
{
    /*
    //private const string Url = "http://10.0.1.36";
    //private const string API_KEY = "6B9A6A475C594BF2B5B07338D8CBDF3E"; //Octoprint Access Control API_Key
    //private const string API_KEY = "62B6B9F70FF04EE4A42919D7BA1D305F"; //Octoprint Application API Key
    private const string API_KEY = "CD11D1961E76427BBFD5C7E9F5083BCF"; //Octoprint Global API Key
    private const string Url = "http://c6bcb01d2d3950f4a1cabc8bd1fe8332.balena-devices.com/?#temp";
    //private const string API_KEY = "cyKbMKthwjqf6k6WrZ8RQddJeWi9CSy"; //Balena API Key
    private const string json = "application / json";
    */


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SelectPrint(Url + "/api/files/local/Mouse.gcode", "{\"command\": \"unselect\", \"print\": \"false\"}"));
        //StartCoroutine(Login(Url + "/api/login", "{\"user\": \"MakerVerse\", \"pass\": \"MakerVerse\"}"));
        //StartCoroutine(RetrieveFiles(Url + "/api/files/", "{\"files\": }"));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    IEnumerator RetrieveFiles(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "GET");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("X-Api-Key", API_KEY);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            JSONNode fileInfo = JSON.Parse(uwr.downloadHandler.text);
            //string name = fileInfo["name"];
            //Debug.Log("Tool Temperature:" + name);
            Debug.Log("Info" + uwr.downloadHandler.text);
        }
    }

    IEnumerator Login(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("X-Api-Key", API_KEY);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            //JSONNode fileInfo = JSON.Parse(uwr.downloadHandler.text);
            //string name = fileInfo["name"];
            //Debug.Log("Tool Temperature:" + name);
            Debug.Log("Success" + uwr.downloadHandler.text);
        }
    }*/
}
