using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static System.Net.Mime.MediaTypeNames;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UI.Button;
using UnityEditor;


public class GraphCTRL : MonoBehaviour
{

    Graph graph;
    private GameObject iButton, eButton, aButton;
    public GameObject panel;
    public GameObject preview, instantiatedPreview;
    public Material lineMaterial;
    private GameObject line, lineDiag;
    private Vector3 Elast, Enow, Enext, Ilast, Inow, Inext, Alast, Anow, Anext;
    int count = 0;
    int x = 0;
    int drawCount = 0;
    int eCount, iCount, aCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        iButton = Resources.Load("iButton") as GameObject;
        eButton = Resources.Load("eButton") as GameObject;
        aButton = Resources.Load("aButton") as GameObject;
        line = Resources.Load("LineDown") as GameObject;
        lineDiag = Resources.Load("LineDiag") as GameObject;
        preview = Resources.Load("ScrubScreenBig") as GameObject;
        graph = new Graph();
        Elast = new Vector3(-900, 700, 0);
        Enow = new Vector3(-300, 700, 0);
        Enext = new Vector3(300, 700, 0);
        Ilast = new Vector3(-900, 134, 0);
        Inow = new Vector3(-300, 134, 0);
        Inext = new Vector3(300, 134, 0);
        Alast = new Vector3(-900, -432, 0);
        Anow = new Vector3(-300, -432, 0);
        Anext = new Vector3(300, -432, 0);
        //Pnow = new Vector3(-300, -998, 0);
        //Pnext = new Vector3(300, -998, 0);

        var e1 = new Node() { NodeColor = Color.red, Position = Enow };
        var e2 = new Node() { NodeColor = Color.red, Position = Enext };
        var i1 = new Node() { NodeColor = Color.blue, Position = Inow, Parent = e1 };
        var i2 = new Node() { NodeColor = Color.blue, Position = Inext, Parent = e1 };
        var a1 = new Node() { NodeColor = Color.green, Position = Anow, Parent = i1, };
        var a2 = new Node() { NodeColor = Color.green, Position = Anext, Parent = i1 };
        var a3 = new Node() { Parent = i1 };
        var a4 = new Node() { Parent = i1 };
        var a5 = new Node() { Parent = i2 };
        var a6 = new Node() { Parent = i2 };



        graph.eNodes.Add(e1); graph.eNodes.Add(e2);
        graph.iNodes.Add(i1); graph.iNodes.Add(i2);
        graph.aNodes.Add(a1); graph.aNodes.Add(a2); graph.aNodes.Add(a3); graph.aNodes.Add(a4); graph.aNodes.Add(a5); graph.aNodes.Add(a6);

        Debug.Log(" aNodes: " + graph.aNodes.Count + " iNodes: " + graph.iNodes.Count + " eNodes: " + graph.eNodes.Count);
        Build();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T) && x < 1)
        {
            Debug.Log("Next");
            GameObject.Find("BotLineDiag").GetComponent<Image>().enabled = true;
            //GameObject.Find("BotLineDiag").SetActive(true);
            Next();
            x++;

        }
        if (Input.GetKey(KeyCode.R))
        {
            x = 0;
        }
    }

    private void Next()
    {
        if (aCount + 1 == graph.aNodes.Count)
        {
            Debug.Log("No more aNodes");
            return;
        }

        if (graph.aNodes[aCount].Parent == graph.aNodes[aCount + 1].Parent)
        {
            //tracks index in aNodes of the current node (the bottom left button)
            aCount++;
            graph.aNodes[aCount - 1].Position = Alast;
            graph.aNodes[aCount].Position = Anow;
            graph.aNodes[aCount + 1].Position = Anext;
            if (graph.aNodes[aCount + 1] != null && graph.aNodes[aCount].Parent != graph.aNodes[aCount + 1].Parent)
            {
                GameObject.Find("BotLineDiag").GetComponent<Image>().enabled = false;
                //GameObject.Find("BotLineDiag").SetActive(false);
            }
            //else GameObject.Find("BotLineDiag").SetActive(true);
        }
/*        else if (graph.aNodes[aCount].Parent == graph.aNodes[aCount + 1].Parent)
        {
            aCount++;
            pCount++;
            graph.aNodes[aCount].Position = Anow;
            graph.aNodes[aCount + 1].Position = Anext;
            graph.pNodes[pCount].Position = Pnow;
            graph.pNodes[pCount + 1].Position = Pnext;
        }
        else if (graph.iNodes[iCount].Parent == graph.iNodes[iCount + 1].Parent)
        {
            iCount++;
            pCount++;
            graph.iNodes[iCount].Position = Inow;
            graph.iNodes[iCount + 1].Position = Inext;
            graph.pNodes[pCount].Position = Pnow;
            graph.pNodes[pCount + 1].Position = Pnext;
        }
        else if (graph.eNodes[eCount].Parent == graph.eNodes[eCount + 1].Parent)
        {
            eCount++;
            pCount++;
            graph.eNodes[eCount].Position = Enow;
            graph.eNodes[eCount + 1].Position = Enext;
            graph.pNodes[pCount].Position = Pnow;
            graph.pNodes[pCount + 1].Position = Pnext;
        }*/
        else
        {
            Debug.Log("End of graph");
        }
        Build();
    }

    private void Build()
    {
        int i = 0;
        //drawCount = 0;
        ClearNodes();
        while (i < 2)
        {
            DrawNode(graph.eNodes[eCount + i]);
            DrawNode(graph.iNodes[iCount + i]);
            DrawNode(graph.aNodes[aCount + i]);
            i++;
        }
        if (aCount>0) { DrawNode(graph.aNodes[aCount - 1]); }
    }

    private void ClearNodes()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("GraphButton");
        foreach (var node in nodes)
        {
            Destroy(node);
        }
    }

    private void DrawNode(Node node)
    {

        GameObject but;
/*        if (graph.pNodes.Contains(node))
        {
            but = Instantiate(pButton, panel.transform, false);
            if (node == graph.pNodes[pCount])
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "P" + (pCount + 1);
                but.gameObject.GetComponent<Button>().onClick.AddListener(() => HighlightObj(node));
                Debug.Log("Drawing p node: " + pCount + " at position: " + node.Position);
            }
            else
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "P" + (pCount + 2);
                Debug.Log("Drawing p node: " + (pCount + 1) + " at position: " + node.Position);
            }

        }*/
        if (node.Position.y < 0)
        {
            but = Instantiate(aButton, panel.transform, false);
            if (node == graph.aNodes[aCount])
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "A" + (aCount + 1);
                but.gameObject.GetComponent<Button>().onClick.AddListener(() => InstantiateVid(node));
                Debug.Log("Drawing a node: " + aCount + " at position: " + node.Position);
            }
            else if (node == graph.aNodes[aCount + 1])
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "A" + (aCount + 2);
                Debug.Log("Drawing a node: " + (aCount + 1) + " at position: " + node.Position);
            }
            else
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "A" + (aCount);
                Debug.Log("Drawing a node: " + (aCount - 1) + " at position: " + node.Position);
            }
        }
        else if (node.Position.y < 500)
        {
            but = Instantiate(iButton, panel.transform, false);
            if (node == graph.iNodes[iCount])
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "I" + (iCount + 1);
                //Debug.Log("Drawing i node: " + iCount + " at position: " + node.Position);
            }
            else
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "I" + (iCount + 2);
                //Debug.Log("Drawing i node: " + (iCount + 1) + " at position: " + node.Position);
            }
        }
        else
        {
            but = Instantiate(eButton, panel.transform, false);
            //but.GetComponentInChildren<TextMeshPro>().text = "test";

        }
        but.GetComponent<Transform>().localPosition = panel.transform.position + node.Position;
        but.GetComponent<MeshRenderer>().material.SetColor("_Color", node.NodeColor);
        drawCount++;
        //Debug.Log("Drawing");
    }

    private void InstantiateVid(Node node)
    {
        if (GameObject.Find("ScrubScreenBig(Clone)"))
        {
            Destroy(GameObject.Find("ScrubScreenBig(Clone)"));
            return;
        }
        Debug.Log("Instantiate Video");
        Vector3 instantiatePoint = node.Position;
        instantiatePoint.y = instantiatePoint.y + 400;
        //GameObject go = Instantiate(preview, instantiatePoint, Quaternion.identity, GameObject.Find("Progress").transform);
        GameObject go = Instantiate(preview, GameObject.Find("aButton(Clone)").transform, false);

        Debug.Log("Initial POS: " + go.GetComponent<Transform>().position);
        instantiatedPreview = go;
        instantiatedPreview.GetComponent<Transform>().localPosition = instantiatePoint;
    }

    public class Graph
    {
        public Graph()
        {
            eNodes = new List<Node>();
            iNodes = new List<Node>();
            aNodes = new List<Node>();
            pNodes = new List<Node>();
            //Edges = new List<Edge>();
        }

        public List<Node> eNodes { get; set; }
        public List<Node> iNodes { get; set; }
        public List<Node> aNodes { get; set; }
        public List<Node> pNodes { get; set; }

        //public List<Edge> Edges { get; set; }
    }

    public class Node
    {
        //private GameObject nodeButton;

        public Node()
        {
            NodeColor = Color.white;
            Position = Vector3.zero;
            Highlights = new List<GameObject>();
        }
        public Color NodeColor { get; set; }
        public Vector3 Position { get; set; }
        public Node Parent { get; set; }
        public String File { get; set; }
        public List<GameObject> Highlights { get; set; }
    }

}

