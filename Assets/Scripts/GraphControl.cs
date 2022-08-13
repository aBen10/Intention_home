using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GraphControl : MonoBehaviour
{

    Graph graph;
    private GameObject iButton, eButton, aButton, pButton;
    public GameObject panel;
    public Material lineMaterial;
    private GameObject line, lineDiag;
    private Vector3 Enow, Enext, Inow, Inext, Anow, Anext, Pnow, Pnext;
    int count = 0;
    int x = 0;
    int drawCount = 0;
    int eCount, iCount, aCount, pCount = 0;

    // Start is called before the first frame update
    void Start()
    {

        iButton = Resources.Load("iButton") as GameObject;
        eButton = Resources.Load("eButton") as GameObject;
        aButton = Resources.Load("aButton") as GameObject;
        pButton = Resources.Load("pButton") as GameObject;
        line = Resources.Load("LineDown") as GameObject;
        lineDiag = Resources.Load("LineDiag") as GameObject;
        graph = new Graph();
        Enow = new Vector3(-300, 700, 0);
        Enext = new Vector3(300, 700, 0);
        Inow = new Vector3(-300, 134, 0);
        Inext = new Vector3(300, 134, 0);
        Anow = new Vector3(-300, -432, 0);
        Anext = new Vector3(300, -432, 0);
        Pnow = new Vector3(-300, -998, 0);
        Pnext = new Vector3(300, -998, 0);

        var e1 = new Node() { NodeColor = Color.red, Position = Enow };
        var e2 = new Node() { NodeColor = Color.red, Position = Enext};
        var i1 = new Node() { NodeColor = Color.blue, Position = Inow, Parent = e1 };
        var i2 = new Node() { NodeColor = Color.blue, Position = Inext, Parent = e1 };
        var a1 = new Node() { NodeColor = Color.green, Position = Anow, Parent = i1 };
        var a2 = new Node() { NodeColor = Color.green, Position = Anext, Parent = i1 };
        var a3 = new Node() { Parent = i1 };
        var p1 = new Node() { NodeColor = Color.yellow, Position = Pnow, Parent = a1 };
        var p2 = new Node() { NodeColor = Color.yellow, Position = Pnext, Parent = a1 };
        var p3 = new Node() { Parent = a1 };
        var p4 = new Node() { Parent = a2 };
        var p5 = new Node() { Parent = a2 };
        /*var edge1 = new Edge() { From = e1, To = i1, EdgeColor = Color.green };
        var edge12 = new Edge() { From = e1, To = i2, EdgeColor = Color.green };
        var edge2 = new Edge() { From = i1, To = a1, EdgeColor = Color.green };
        var edge22 = new Edge() { From = i1, To = a2, EdgeColor = Color.green };
        var edge3 = new Edge() { From = a1, To = p1, EdgeColor = Color.green };
        var edge32 = new Edge() { From = a1, To = p2, EdgeColor = Color.green };*/

        graph.eNodes.Add(e1);
        graph.eNodes.Add(e2);
        graph.iNodes.Add(i1);
        graph.iNodes.Add(i2);
        graph.aNodes.Add(a1);
        graph.aNodes.Add(a2);
        graph.aNodes.Add(a3);
        graph.pNodes.Add(p1);
        graph.pNodes.Add(p2);
        graph.pNodes.Add(p3);
        graph.pNodes.Add(p4);
        graph.pNodes.Add(p5);

        /*graph.Edges.Add(edge1);
        graph.Edges.Add(edge12);
        graph.Edges.Add(edge2);
        graph.Edges.Add(edge22);
        graph.Edges.Add(edge3);
        graph.Edges.Add(edge32);*/

        Debug.Log("pNodes: " + graph.pNodes.Count + " aNodes: " + graph.aNodes.Count + " iNodes: " + graph.iNodes.Count + " eNodes: " + graph.eNodes.Count);
        Build();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T) && x<1)
        {
            Debug.Log("Next"); 
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
        pCount++;
        
        if(pCount + 1 == graph.pNodes.Count)
        {
            Debug.Log("No more pNodes");
            return;
        }
        
        if (graph.pNodes[pCount].Parent == graph.pNodes[pCount + 1].Parent)
        {
            graph.pNodes[pCount].Position = Pnow;
            graph.pNodes[pCount + 1].Position = Pnext;
        }
        else if(graph.aNodes[aCount].Parent == graph.aNodes[aCount + 1].Parent)
        {
            aCount++;
            graph.aNodes[aCount].Position = Anow;
            graph.aNodes[aCount + 1].Position = Anext;
        }
        /*else if (graph.iNodes[iCount].Parent == graph.iNodes[iCount + 1].Parent)
        {
            graph.iNodes[iCount].Position = Inow;
            graph.iNodes[iCount + 1].Position = Inext;
        }
        else if (graph.eNodes[eCount].Parent == graph.eNodes[eCount + 1].Parent)
        {
            graph.eNodes[eCount].Position = Enow;
            graph.eNodes[eCount + 1].Position = Enext;
        }*/
   /*     else
        {
            graph.pNodes[pCount].Position = Pnow;
            graph.pNodes[pCount + 1].Position = Pnext;
            aCount++;
            if (aCount + 1 == graph.aNodes.Count)
            {
                return;
            }
            else if (graph.aNodes[aCount].Parent == graph.aNodes[aCount + 1].Parent)
            {
                graph.aNodes[aCount].Position = Anow;
                graph.aNodes[aCount + 1].Position = Anext;
            }
            else if (graph.iNodes[iCount].Parent == graph.iNodes[iCount + 1].Parent)
            {
                graph.iNodes[iCount].Position = Inow;
                graph.iNodes[iCount + 1].Position = Inext;
            }
            else if (graph.eNodes[eCount].Parent == graph.eNodes[eCount + 1].Parent)
            {
                graph.eNodes[eCount].Position = Enow;
                graph.eNodes[eCount + 1].Position = Enext;
            }
            else
            {
                graph.aNodes[aCount].Position = Anow;
                graph.aNodes[aCount + 1].Position = Anext;
                iCount++;
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
        while(i<2)
        {
            DrawNode(graph.eNodes[eCount + i]);
            DrawNode(graph.iNodes[iCount + i]);
            DrawNode(graph.aNodes[aCount + i]);
            DrawNode(graph.pNodes[pCount + i]);
            i++;
        }
        /*foreach (var node in graph.eNodes) { DrawNode(node); }
        foreach (var node in graph.iNodes) { DrawNode(node); }
        foreach (var node in graph.aNodes) { DrawNode(node); }
        foreach (var node in graph.pNodes) { DrawNode(node); }*/

        /*foreach (var edge in graph.Edges)
        {
            DrawLine(edge.From, edge.To, edge.EdgeColor);
        }*/
        //Debug.Log("Buid Done");
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
        if (graph.pNodes.Contains(node))
        {
            but = Instantiate(pButton, panel.transform, false);
            if (node == graph.pNodes[pCount])
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "P" + (pCount + 1);
                Debug.Log("Drawing p node: " + pCount + " at position: " + node.Position);
            }
            else
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "P" + (pCount + 2);
                Debug.Log("Drawing p node: " + (pCount + 1) + " at position: " + node.Position);
            }

        }
        else if(node.Position.y < 0)
        {
            but = Instantiate(aButton, panel.transform, false);
            if (node == graph.aNodes[aCount])
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "A" + (aCount + 1);
                Debug.Log("Drawing a node: " + aCount + " at position: " + node.Position);
            }
            else
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "A" + (aCount + 2);
                Debug.Log("Drawing a node: " + (aCount + 1) + " at position: " + node.Position);
            }
        }
        else if(node.Position.y < 500)
        {
            but = Instantiate(iButton, panel.transform, false);
            if (node == graph.iNodes[iCount])
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "I" + (iCount + 1);
                Debug.Log("Drawing i node: " + iCount + " at position: " + node.Position);
            }
            else
            {
                but.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "I" + (iCount + 2);
                Debug.Log("Drawing i node: " + (iCount + 1) + " at position: " + node.Position);
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
    
    /*void DrawLine(Node start, Node end, Color color)
    {
        count++;
        GameObject temp;
        Vector3 loc;
        if (end.Position.x < 0)
        {
            temp = Instantiate(line, panel.transform, false) as GameObject;
            loc = temp.GetComponent<Transform>().localPosition;
            loc.x = end.Position.x;
        }
        else
        {
            temp = Instantiate(lineDiag, panel.transform, false) as GameObject;
            loc = temp.GetComponent<Transform>().localPosition;
            loc.x = 40;
        }

        loc.y = Math.Abs((end.Position.y - start.Position.y) / 2) + end.Position.y;
        Debug.Log(count);
        Debug.Log((end.Position.y - start.Position.y) / 2);
        Debug.Log(((Math.Abs(end.Position.y) + Math.Abs(start.Position.y)) / 2) + end.Position.y);
        temp.GetComponent<Transform>().localPosition = loc;
    }*/

    public class Graph
    {
        public Graph()
        {
            eNodes = new List<Node>();
            iNodes = new List<Node>();
            aNodes = new List<Node>();
            pNodes = new List<Node>();
            Edges = new List<Edge>();
        }

        public List<Node> eNodes { get; set; }
        public List<Node> iNodes { get; set; }
        public List<Node> aNodes { get; set; }
        public List<Node > pNodes { get; set; }

        public List<Edge> Edges { get; set; }
    }

    public class Node
    {
        //private GameObject nodeButton;

        /*public Node(Color color, Vector3 position)
        {
            //GameObject temp = Instantiate(buttons, GameObject.Find("RightPanel").transform, false);
            //temp.GetComponent<RectTransform>().anchoredPosition = position;
            //temp.GetComponent<RectTransform>().localPosition = position;
            //temp.GetComponent<Renderer>().material.color = color;
            NodeColor = color;
            Position = position;
        }
        public Node()
        {
            NodeColor = Color.white;
            Position = Vector3.zero;
        }*/
        public Color NodeColor { get; set; }
        public Vector3 Position { get; set; }
        public Node Parent { get; set; }
    }
/*    public class iNode
    {

        public Color NodeColor { get; set; }
        public Vector3 Position { get; set; }
        public eNode Parent { get; set; }
    }
    public class aNode
    {

        public Color NodeColor { get; set; }
        public Vector3 Position { get; set; }
        public iNode Parent { get; set; }
    }
    public class pNode
    {

        public Color NodeColor { get; set; }
        public Vector3 Position { get; set; }
        public aNode Parent { get; set; }
    }*/

    public class Edge //make generic?
    {
        public Node From { get; set; }
        public Node To { get; set; }
        public Color EdgeColor { get; set; }

    }





}

