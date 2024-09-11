using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    //grid generation data
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Node[,] grid;
    int nodeCount;

    //adjecent node 
    //List<List<(int, int)>> adjecencyList = new List<List<(int, int)>>();

    int[] xDirection = { -1, 1, 0, 0 };
    int[] yDirection = { 0, 0, 1, -1 };



    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        List<Node> path = BFSpathFinder(grid[0, 0], grid[gridSizeX - 1, gridSizeY - 1]);
        Debug.Log(path.Count);
        for (int i = 0; i < path.Count; i++)
        {
            path[i].color = Color.blue;
        }
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
                grid[x, y].x = x;
                grid[x, y].y = y;
            }
        }
        nodeCount = gridSizeX * gridSizeY;
        //Set AdjecencyList
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int k = 0; k < 4; k++)
                {
                    int xx = x + xDirection[k];
                    int yy = y + yDirection[k];
                    if (xx < 0 || xx >= gridSizeX) continue;
                    if (yy < 0 || yy >= gridSizeY) continue;
                    //Debug.Log(Node + " " + y + " : " + xx + " " +  yy);
                    grid[x, y].adjecentNodes.Add(grid[xx, yy]);
                }
            }
        }

        //adjecencyList.Capacity = gridSizeX * gridSizeY;
        //for (int i = 0; i < adjecencyList.Capacity; i++)
        //{
        //    adjecencyList.Add(new List<(int, int)>());
        //    int r = i / gridSizeX;//the Index of y
        //    int c = i % gridSizeX;//the Index of Node
        //    for (int j = 0; j < 4; j++)
        //    {
        //        int rr = r + rowDirection[j];
        //        int cc = c + colDirection[j];
        //        if (rr < 0 || rr >= gridSizeY) continue;
        //        if (cc < 0 || cc >= gridSizeX) continue;
        //        adjecencyList[i].Add((rr, cc));
        //    }
        //}

        //Print Adjecency List
        //for (int x = 0; x < gridSizeX; x++)//y, row
        //{
        //    for (int y = 0; y < gridSizeY; y++)//Node, col
        //    {
        //        string s = "";
        //        s += x + ", " + y + " => ";
        //        for (int k = 0; k < grid[x, y].adjecentNodes.Count; k++)
        //        {
        //            s += " ( " + grid[x, y].adjecentNodes[k].x.ToString() + ", " + grid[x, y].adjecentNodes[k].y.ToString() + " ) ";
        //        }
        //        Debug.Log(s);

        //    }
        //}
    }

    public List<Node> BFSpathFinder(Node start, Node end)
    {
        //int Node = adjecencyList.Count;
        List<Node> path = new List<Node>();

        //reset visited data;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                grid[x, y].visited = false;
            }
        }

        return BFSpathFinderHelper(start, end, path);

    }

    public List<Node> BFSpathFinderHelper(Node start, Node end, List<Node> path)
    {
        Queue<Node> q = new Queue<Node>();
        q.Enqueue(start);

        Node n = q.Peek();
        while (q.Count > 0)
        {
            n = q.Peek();
            //n.visited = true;
            q.Dequeue();
            if(n == end)
            {
                break;
            }
            for (int i = 0; i < n.adjecentNodes.Count; i++)
            {
                if (!n.adjecentNodes[i].visited && n.adjecentNodes[i].walkable)
                {
                    n.adjecentNodes[i].visited = true;
                    q.Enqueue(n.adjecentNodes[i]);
                    n.adjecentNodes[i].prevNode = n;
                }
            }
        }
        
        while(n != start)
        {
            path.Add(n);
            n = n.prevNode;
        }
        return path;
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

public class Node
{
    public int x, y;
    public bool walkable;
    public bool visited;
    public Vector3 worldPosition;
    public Color color;
    public List<Node> adjecentNodes = new List<Node>();
    public Node prevNode = null;
    public Node(bool _walkable, Vector3 _worldPos, bool _reached = false, Color? _color = null)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        visited = _reached;
        color = _color ?? Color.white;
    }
}
