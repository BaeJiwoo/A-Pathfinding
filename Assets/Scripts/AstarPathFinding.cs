using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
public class AstarPathFinding : MonoBehaviour
{
    Grid grid;

    private (int, int) start;
    private (int, int) end;

    private (int, int) gridSize;

    // Start is called before the first frame update
    void Start()
    {
        grid = transform.GetComponent<Grid>();
        gridSize = grid.GetNodeCount();
        start = (0, 0);
        end = (gridSize.Item1 - 1, gridSize.Item2 - 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Astar((start), (end));
        }
    }

    public void Astar((int, int) start, (int, int) end)
    {
        AstarHelper(start.Item1, start.Item2);
    }
    public void AstarHelper(int x, int y)
    {
        //인접 노드 찾기

        List<(int, int)> adjecentNodeList = new List<(int, int)>();
        grid.GetNode(x, y).reached = true;
        for (int i = -1; i < 2; i++)
        {
            if ((x + i < gridSize.Item1 && x + i >= 0) && grid.GetNode(x + i, y).reached == false)
            {
                adjecentNodeList.Add((x + i, y));
            }
        }
        for (int j = -1; j < 2; j++)
        {
            if ((y + j < gridSize.Item1 && y + j >= 0) && grid.GetNode(x, y + j).reached == false)
            {
                adjecentNodeList.Add((x, y + j));
            }
        }

        //Debug.Log(adjecentNodeList.Count);
        //for (int i = 0; i < adjecentNodeList.Count; i++)
        //{
        //    Debug.Log(adjecentNodeList[i]);
        //}

        //인접 노드 평가

    }
}
