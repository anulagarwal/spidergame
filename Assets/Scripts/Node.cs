using UnityEngine;
using System.Collections.Generic;


[ExecuteInEditMode]
public class Node : MonoBehaviour {
	public List<Node> neighbours;
	[SerializeField] LineRenderer lr;

    private void Start()
    {
		
       
    }

    private void Update()
    {
        lr.positionCount = neighbours.Count * 3 ;

        for (int i = 0; i < neighbours.Count; i++)
        {
            lr.SetPosition(i * 3, transform.position);
            lr.SetPosition(i * 3 + 1, neighbours[i].transform.position);
            lr.SetPosition(i * 3 + 2, transform.position);
        }
    }
    public Node() {
		neighbours = new List<Node>();
	}
	


    private void OnMouseDown()
    {
       // Spider.Instance.MoveToNode(this);
		//print(MapGenerator.Instance.nodes[0]);
    }
}
