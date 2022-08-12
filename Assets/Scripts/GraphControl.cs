using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Video;

public class GraphControl : MonoBehaviour
{

    Graph graph;
    private GameObject button, instantiatedButton;
    public GameObject panel;
    public Material lineMaterial;
    private GameObject line;

    // Start is called before the first frame update
    void Start()
    {

        button = Resources.Load("GraphButton") as GameObject;
        line = Resources.Load("Line") as GameObject;
        graph = new Graph();
        var node1 = new Node() { NodeColor = Color.red, Position = new Vector3(-250, 700, 0) };
        var node2 = new Node() { NodeColor = Color.blue, Position = new Vector3(-250, -300, 1) };
        var edge1 = new Edge() { From = node1, To = node2, EdgeColor = Color.green };
        graph.Nodes.Add(node1);
        graph.Nodes.Add(node2);
        graph.Edges.Add(edge1); 
        Debug.Log("Starting");
        Build();
        DrawLine(node1, node2, edge1.EdgeColor);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void Build()
    {
        foreach (var node in graph.Nodes)
        {
            DrawNode(node);
            Debug.Log("BuildingNode");
        }
        Debug.Log("Buid Done");
    }

    private void DrawNode(Node node)
    {
        GameObject temp = Instantiate(button, panel.transform, false) as GameObject;
        temp.GetComponent<Transform>().localPosition = panel.transform.position + node.Position;
        temp.GetComponent<MeshRenderer>().material.SetColor("_Color", node.NodeColor);
        Debug.Log("Drawing");
    }
    
    public class Graph
    {
        public Graph()
        {
            Nodes = new List<Node>();
            Edges = new List<Edge>();
        }

        public List<Node> Nodes { get; set; }

        public List<Edge> Edges { get; set; }
    }

    public class Node
    {
        private GameObject nodeButton;

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
    }

    public class Edge
    {
        public Node From { get; set; }
        public Node To { get; set; }
        public Color EdgeColor { get; set; }

    }

    void DrawLine(Node start, Node end, Color color)
    {
        GameObject temp = Instantiate(line, panel.transform, false) as GameObject;
        Vector3 loc = temp.GetComponent<Transform>().localPosition;
        loc.x = end.Position.x;
        loc.y = ((Math.Abs(end.Position.y) + Math.Abs(start.Position.y)) / 2) + end.Position.y;
        Debug.Log((end.Position.y - start.Position.y) / 2);
        Debug.Log(((Math.Abs(end.Position.y) + Math.Abs(start.Position.y)) / 2) + end.Position.y);
        temp.GetComponent<Transform>().localPosition = loc;
        
        

        /*GameObject myLine = new GameObject();
        RectTransform rect = myLine.AddComponent<RectTransform>();
        
        myLine.transform.SetParent(panel.transform);
        myLine.transform.localPosition = start.Position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.material = new Material(lineMaterial);
        lr.startColor = color;
        lr.startWidth = 0.1f;
        lr.SetPosition(0, start.Position);
        lr.SetPosition(1, end.Position);*/
    }




}

