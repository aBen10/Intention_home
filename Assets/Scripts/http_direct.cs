using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 
using SimpleJSON;
using System.IO;
using System;

public class http_direct : MonoBehaviour
{
    private const string Url = "http://10.0.1.36";
    private const string API_KEY = "6B9A6A475C594BF2B5B07338D8CBDF3E";
    private const string json = "application / json";
    GameObject print_bed;
    private Vector3 pos;
    private float pos_y;
    private float last_y;
    private string valueOfPoition;
    private GameObject gcode;

    //private bool toggle=false;  

    // Start is called before the first frame update
    void Start()
    {
        print_bed = GameObject.Find("bed");
        StartCoroutine(HomeTools(Url + "/api/printer/printhead", "{\"command\": \"home\", \"axes\": \"z\"}"));
        //StartCoroutine(SelectPrint(Url + "/api/files/sdcard/SLU~1.GCO", "{\"command\": \"select\", \"print\": \"false\"}"));
        StartCoroutine(SelectPrint(Url + "/api/files/local/Mouse.gcode", "{\"command\": \"unselect\", \"print\": \"false\"}"));

        //Debug.Log("Trigger is pressed");
        pos = print_bed.transform.position;
        pos_y = pos.y;
        pos_y = pos_y * 10;
        pos_y = limit(pos_y);
        last_y=pos_y;
        LoadFile();
    }

    // Update is called once per frame
    void Update()
    {
        pos = print_bed.transform.position;
        pos_y = pos.y;
        pos_y = pos_y * 10;
        pos_y = limit(pos_y);
        if(pos_y != last_y)
        {
            Debug.Log(pos_y);
            last_y=pos_y;
        }
        OnPress(pos_y);
    }

    float limit(float y)
    {
        if (y > 200.0f)
        {
            y = 200.0f;
        }
        if (y < 0.0f)
        {
            y = 0.1f;
        }
        return y;
    }

    void LoadFile()
    {
        //public static string[] GetFiles (string path, string searchPattern);
        List<string> files = new List<string> (Directory.GetFiles(@"/home/moiz/Downloads","*.STL")); 
        List<string> files2 = new List<string> (Directory.GetFiles(@"/home/moiz/Downloads","*.stl"));
        List<string> files3 = new List<string> (Directory.GetFiles(@"/home/moiz/Downloads","*.gcode"));
        List<string> files4 = new List<string> (Directory.GetFiles(@"/home/moiz/Downloads","*.GCO"));
        files.AddRange(files2);
        files.AddRange(files3);
        files.AddRange(files4);
        foreach(string file in files)
        {
            Debug.Log("Available file: " + Path.GetFileName(file)); 
        }
    }

    void OnPress(float y)
    {
        
        string value_y = y.ToString();
        //if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && toggle==false)
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            //toggle = true;
            StartCoroutine(PostBedLevel(Url + "/api/printer/printhead", "{\"command\": \"jog\",\"z\":-" + value_y + "}"));
            Debug.Log("Trigger is pressed");
        }
        //else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        else if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            //toggle = false;            
            Debug.Log("Trigger is released");
        }
        if (Input.GetKey(KeyCode.T))
        {
            //toggle = true;
            StartCoroutine(GetTemp(Url + "/api/printer/tool?history=true&limit=2", "{\"tool0\": { \"actual\":- }"));
        }
        if (Input.GetKey(KeyCode.M))
        {
            StartCoroutine(GetSDStatus(Url + "/api/printer?exclude=temperature", "sd"));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            StartCoroutine(GetPrinterStatus(Url + "/api/printer?exclude=temperature,sd", "{\"state\": "));
        }
        if (Input.GetKey(KeyCode.S))
        {
            //should have popup for person to select file then passs file name into URL below
            //can retrieve all files with /api/files then prompt user with anems of files? Will have to forward fole location as well (sd or local)
            StartCoroutine(SelectPrint(Url + "/api/files/local/Mouse.gcode", "{\"command\": \"select\", \"print\": \"false\"}"));
        }
        if (Input.GetKey(KeyCode.D))
        {
            StartCoroutine(SelectPrint(Url + "/api/files/sdcard/SLU~1.GCO", "{\"command\": \"select\", \"print\": \"false\"}"));
        }
        if (Input.GetKey(KeyCode.P))
        {
            StartCoroutine(StartSelectedPrint(Url + "/api/job", "{\"command\": \"start\"}"));
        }
        if (Input.GetKey(KeyCode.U))
        {
            /*gcode = GameObject.FindGameObjectWithTag("Player");
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
            print("Streaming Assets Path: " + Application.streamingAssetsPath);
            FileInfo[] allFiles = directoryInfo.GetFiles("*.*");
            foreach (FileInfo file in allFiles)
            {
                if (file.Name.Contains("Mouse"))
                {
                    StartCoroutine("LoadPlayerUI", file);
                }
            }*/
            //System.IO.File.
            
            
            StartCoroutine(UploadPrint(Url + "/api/files/local"));
            //"{\"file\"=@" + modelPath + "}"
        }
    }

    IEnumerator UploadPrint(string url)
    {
        //var uwr = new UnityWebRequest(url, "POST");
        /*byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();*/
        //uwr.SetRequestHeader("Content-Type", "multipart/form-data; boundary=------------------------bd4770df75752689");
        string modelPath = Application.dataPath + "/Mouse.gcode";
        Debug.Log(modelPath);
        Debug.Log(Url + "/api/files/local" + "{file =@" + modelPath + "}");

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        byte[] gcode1 = new System.Text.UTF8Encoding().GetBytes(modelPath);
        byte[] gcode2 = File.ReadAllBytes(modelPath);
        Debug.Log(gcode2);
        //formData.Add(new MultipartFormDataSection("file", gcode1));
        
        formData.Add(new MultipartFormFileSection("file", gcode2, "Mouse.gcode", "gcode"));

        
        //uwr.SetRequestHeader("Content-Length", "6964675");

        UnityWebRequest uwr = UnityWebRequest.Post(url, formData);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

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
            Debug.Log(" " + uwr.downloadHandler.text);         
        }
    }

    IEnumerator PostBedLevel(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
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
            Debug.Log("Received Level Change:" + uwr.downloadHandler.text);         
        }
    }

    IEnumerator SelectPrint(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
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
            Debug.Log("Received: Selected " + uwr.downloadHandler.text);         
        }
    }

    IEnumerator StartSelectedPrint(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
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
            Debug.Log("Received: Printing" + uwr.downloadHandler.text);         
        }
    }

    IEnumerator HomeTools(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("X-Api-Key", API_KEY);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        //if (uwr.isNetworkError) - Depreciated
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: Homing");
        }
    }

    IEnumerator GetTemp(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "GET");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("X-Api-Key", API_KEY);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if(uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            JSONNode temperature = JSON.Parse(uwr.downloadHandler.text);
            string temp = temperature["tool0"]["actual"];
            Debug.Log("Tool Temperature:" + temp);         
        }
    }

    IEnumerator GetSDStatus(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "GET");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("X-Api-Key", API_KEY);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if(uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            JSONNode status = JSON.Parse(uwr.downloadHandler.text);
            string sdStatus = status["sd"]["ready"];
            Debug.Log("SD status:" + sdStatus);
        }
    }

    IEnumerator GetPrinterStatus(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "GET");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("X-Api-Key", API_KEY);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if(uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            JSONNode pstatus = JSON.Parse(uwr.downloadHandler.text);
            Debug.Log(pstatus);
            string printerStatus = pstatus["state"]["flags"]["operational"];
            string printerPaused = pstatus["state"]["flags"]["paused"];
            string printerError = pstatus["state"]["flags"]["error"];
            string printerReady = pstatus["state"]["flags"]["true"];

            Debug.Log("Printer operational:" + printerStatus);  
            Debug.Log("Printer paused:" + printerPaused);
            Debug.Log("Printer Error:" + printerError);
            Debug.Log("Printer Ready:" + printerReady);
        }
    }
}
