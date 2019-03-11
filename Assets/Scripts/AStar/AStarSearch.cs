using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;


public class AStarSearch : MonoBehaviour {

    List<Node> openSet;
    List<Node> closedSet;

    public Transform startPos;
    public Transform endPos;

    public GridSetup grid;

    public void Update()
    {
        search(startPos.position, endPos.position);
    }

    void search(Vector2 startPos, Vector2 endPos)
    {
        openSet = new List<Node>();
        closedSet = new List<Node>();

        Node startNode = grid.getNodeFromWorldPoint(startPos);
        Node endNode = grid.getNodeFromWorldPoint(endPos);

        openSet.Add(startNode);
        
        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i=1; i < openSet.Count;i++)
            {
                if(openSet[i].getFCost() < currentNode.getFCost() || openSet[i].getFCost() == currentNode.getFCost() )
                {
                    
                    if(openSet[i].hCost < currentNode.hCost){ currentNode = openSet[i]; }
                      
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode) { getPath(startNode, endNode); return; }

            foreach(Node node in grid.getNeighbours(currentNode))
            {
                if (!node.walkable || closedSet.Contains(node)) { continue; }
                int moveCost = currentNode.gCost + getDistance(currentNode, node) + node.penalty;
                if(moveCost < node.gCost || !openSet.Contains(node))
                {
                    node.gCost = moveCost;
                    node.hCost = getDistance(node, endNode);
                    node.parent = currentNode;
                    if (!openSet.Contains(node)) { openSet.Add(node); }
                }
            }
        }
    }

    void getPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node node = endNode;
        while(node != startNode)
        {
            path.Add(node);
            node = node.parent;
        }
        path.Reverse();

        grid.path = path;

       
    }

    int getDistance(Node a, Node b)
    {
        int x = Mathf.Abs(a.gridX - b.gridX);
        int y = Mathf.Abs(a.gridY - b.gridY);
        
        if(x > y) { return 14 * y + 10 * (x - y); }
        else { return 14 * x + 10 * (y - x); }
    }


}
