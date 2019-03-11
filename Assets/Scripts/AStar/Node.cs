using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public bool walkable;
    public Vector2 worldPos;

    public int gridX, gridY;
    public int penalty;

    public int gCost;
    public int hCost;
    public int fCost;

    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        penalty = _penalty;
    }

    public int getFCost()
    {
        return gCost * hCost;
    }
	
}
