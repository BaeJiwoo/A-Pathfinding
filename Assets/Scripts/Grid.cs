using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    //
    private int nodeCountX;
    private int nodeCountY;
    public (int, int) GetNodeCount()
    {
        return (nodeCountX, nodeCountY);
    }
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        nodeCountX = (int)(gridWorldSize.x / (nodeRadius * 2));
        nodeCountY = (int)(gridWorldSize.y / (nodeRadius * 2));
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    public void SetGrid(int x, int y, bool _reached, Color? _color = null)
    {
        grid[x, y].visited = _reached;
        grid[x, y].color = _color ?? Color.white;
    }

    public Node GetNode(int x, int y)
    {
        return grid[x, y];
    }

    public int Size()
    {
        return 0;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));


        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? n.color : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}

//public class Node
//{
//    public bool walkable;
//    public bool visited;
//    public Vector3 worldPosition;
//    public Color color;
//    public Node(bool _walkable, Vector3 _worldPos, bool _reached = false, Color? _color = null)
//    {
//        walkable = _walkable;
//        worldPosition = _worldPos;
//        visited = _reached;
//        color = _color ?? Color.white;
//    }
//}

//public class Node
//{

//    public bool walkable;
//    public Vector3 worldPosition;

//    public Node(bool _walkable, Vector3 _worldPos)
//    {
//        walkable = _walkable;
//        worldPosition = _worldPos;
//    }
//}