using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class n
{
    public string Name;
    public List<n> Neighbors;

    public n(string name)
    {
        Name = name;
        Neighbors = new List<n>();
    }
}

public class GraphTravelsal : MonoBehaviour
{
    void Start()
    {
        // 노드 생성
        n nodeA = new n("A");
        n nodeB = new n("B");
        n nodeC = new n("C");
        n nodeD = new n("D");
        // 노드 연결 (참조 타입 사용)
        nodeA.Neighbors.Add(nodeB);
        nodeB.Neighbors.Add(nodeC);
        nodeC.Neighbors.Add(nodeD);

        // 탐색 시작
        TraverseGraph(nodeA);
    }

    void TraverseGraph(n currentNode)
    {
        Debug.Log("Visiting Node: " + currentNode.Name);
        foreach (n neighbor in currentNode.Neighbors)
        {
            TraverseGraph(neighbor); // 재귀적 탐색
        }
    }
}