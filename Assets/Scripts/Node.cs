using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour {
	public List<Node> neighbours;
	public int x;
	public int y;
	
	public Node() {
		neighbours = new List<Node>();
	}
	
	public float DistanceTo(Node n) {
		if(n == null) {
			Debug.LogError("WTF?");
		}
		
		return Vector2.Distance(
			new Vector2(x, y),
			new Vector2(n.x, n.y)
			);
	}

    private void OnMouseDown()
    {
		MapGenerator.Instance.GeneratePathTo(MapGenerator.Instance.nodes[2], this)[0].GetComponent<MeshRenderer>().material.color = Color.red;
		//print(MapGenerator.Instance.nodes[0]);
    }
}
