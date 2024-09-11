using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Properties;
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

        Debug.Log(start + "  " + end);
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
        AstarHelper(start.Item1, start.Item2, end.Item1, end.Item2);

        grid.SetGrid(start.Item1, start.Item2, true, Color.green);
        grid.SetGrid(end.Item1, end.Item2, true, Color.red);
    }

    
    public void AstarHelper(int x, int y, int targetX, int targetY)
    {
        while ((x != targetX) || (y != targetY))
        {
            grid.SetGrid(x, y, true, Color.blue);
            //인접 노드 찾기
            List<(int, int)> adjecentNodeList = new List<(int, int)>();
            //대각선 후보에서 제외
            for (int i = -1; i < 2; i++)
            {
                if ((x + i < gridSize.Item1 && x + i >= 0) && grid.GetNode(x + i, y).visited == false && grid.GetNode(x + i, y).walkable == true)
                {
                    adjecentNodeList.Add((x + i, y));
                }
            }
            for (int j = -1; j < 2; j++)
            {
                if ((y + j < gridSize.Item1 && y + j >= 0) && grid.GetNode(x, y + j).visited == false && grid.GetNode(x, y + j).walkable == true)
                {
                    adjecentNodeList.Add((x, y + j));
                }
            }

            //대각선도 후보에 넣기
            //for (int i = -1; i < 2; i++)
            //{
            //    for (int j = -1; j < 2; j++)
            //    {
            //        if ((Node + i < gridSize.Item1 && Node + i >= 0) && (y + j < gridSize.Item2 && y + j >= 0) && grid.GetNode(Node + i, y + j).visited == false && grid.GetNode(Node+i, y + j).walkable == true)
            //        {
            //            adjecentNodeList.Add((Node+i, y + j));
            //        }
            //    }
            //}


            //Debug.Log(adjecentNodeList.Count);
            //for (int i = 0; i < adjecentNodeList.Count; i++)
            //{
            //    Debug.Log(adjecentNodeList[i]);
            //}

            //인접 노드 평가(맨해탄 거리 방식으로 평가)
            int minIndex = 0;
            int minDistance = ChebshevDistance(adjecentNodeList[minIndex].Item1, adjecentNodeList[minIndex].Item2);
            int[] hueristicCostList = new int[adjecentNodeList.Count];
            for (int i = 0; i < adjecentNodeList.Count; i++)
            {
                int adjecentX = adjecentNodeList[i].Item1;
                int adjecentY = adjecentNodeList[i].Item2;
                if (minDistance > ChebshevDistance(adjecentX, adjecentY))
                {
                    minIndex = i;
                }
            }
            x = adjecentNodeList[minIndex].Item1;
            y = adjecentNodeList[minIndex].Item2;


            Debug.Log(x + ", " + y);
        }




    }

    private int ManhattanDistance(int x, int y)
    {
        return Mathf.Abs(end.Item1 - x) + Mathf.Abs(end.Item2 - y);
    }

    private int ChebshevDistance(int x, int y)
    {
        return Mathf.Max(Mathf.Abs(end.Item1 - x), Mathf.Abs(end.Item2 - y));
    }
}
