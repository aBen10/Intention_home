using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //create a node
        Node node1 = new Node(1);

        //create a child of node1

    }

    // Update is called once per frame
    void Update()
    {

    }

    //create generic tree data structure
    public class Node
    {
        public int value;
        public List<Node> children;
        public Node(int value)
        {
            this.value = value;
            children = new List<Node>();
        }
    }

    //create node class with siblings   
    




}

