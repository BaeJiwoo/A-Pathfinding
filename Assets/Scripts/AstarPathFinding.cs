using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
public class AstarPathFinding : MonoBehaviour
{
    Grid grid;

    public (int, int) start = (0,0);
    public (int, int) end = (24, 24);
    // Start is called before the first frame update
    void Start()
    {
        grid = transform.GetComponent<Grid>();

        grid.SetGrid(0, 0, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Astar((start), (end));
        }
    }

    public void Astar((int, int) start, (int, int) end)
    {

        grid.SetGrid(start.Item1, start.Item2, false, Color.green);
        grid.SetGrid(end.Item1, end.Item2, false, Color.yellow);
        AstarHelper(grid.GetNode(start.Item1, start.Item2));
    }
    public void AstarHelper(Node node)
    {
        node.reached = true;
        List<Node> adjecentList = new List<Node>();
        grid.GetAdjecentNode(start.Item1, start.Item2, adjecentList);
        Debug.Log(adjecentList.Count);
        for (int i =0; i<adjecentList.Count; i++)
        {
            Node n = adjecentList[i];
            n.color = Color.cyan;
            adjecentList[i] = n;
        }
    }
}
