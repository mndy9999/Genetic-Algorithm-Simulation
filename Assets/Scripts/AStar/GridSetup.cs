using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSetup : MonoBehaviour {

    
    [System.Serializable]
    public struct Terrain{
        public LayerMask mask;
        public int penalty;
    }

    
    public Transform player;
    public LayerMask unwalkableMask;

    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float nodeDiameter;

    public List<Node> path;
    public GameObject map;
    //public TileBase pathTile;

    Node[,] grid;
    
    public Terrain[] walkableRegions;
    Dictionary<int, int> walkableDic = new Dictionary<int, int>();
    LayerMask walkableMask;

    int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach(Terrain terrain in walkableRegions)
        {
            walkableMask.value |= terrain.mask.value;
            walkableDic.Add((int)Mathf.Log(terrain.mask.value, 2), terrain.penalty);
        }
        drawGrid();
        
    }
    private void Update()
    {
        drawGrid();
        drawPath();        
    }

    void drawGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 bottomLeft = new Vector2(transform.position.x, transform.position.y) - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = bottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));

                int moveCost = 0;
                if (walkable)
                {
                    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.down, 100, walkableMask);
                    if(hit)
                    {
                        walkableDic.TryGetValue(hit.collider.gameObject.layer, out moveCost);
                    }
                }
                grid[x, y] = new Node(walkable, worldPoint, x, y, moveCost);
            }
        }
    }

    void drawPath()
    {
        int totalCost = 0;
        if (path != null)
        {
            foreach(Node node in path)
            {
                //map.SetTile(new Vector3Int((int)node.worldPos.x-1, (int)node.worldPos.y-1, 1), pathTile);
                totalCost += node.penalty;
            }
        }
    }

    public List<Node> getNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) { continue; }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


    public Node getNodeFromWorldPoint(Vector2 worldPos)
    {

        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    
    private void OnDrawGizmos()
    {
        
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
        if (grid != null)
        {
            Node playerNode = getNodeFromWorldPoint(player.position);
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (node == playerNode) { Gizmos.color = Color.cyan; }
                
                Gizmos.DrawWireCube(node.worldPos, Vector2.one * (nodeDiameter - 0.1f));
                if (path != null && path.Contains(node)) { Gizmos.color = Color.yellow; Gizmos.DrawCube(node.worldPos, Vector2.one * (nodeDiameter - 0.1f)); }
            }
        }

    }

}
