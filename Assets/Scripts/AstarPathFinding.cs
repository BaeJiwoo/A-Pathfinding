using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPathFinding : MonoBehaviour
{
    Grid grid;

    Vector3
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

        }
    }
}
