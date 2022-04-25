using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class ConnectionDisplayer : MonoBehaviour

{
    [SerializeField] public List<NeighborManager.Neighbor> neighbors;

    [SerializeField] public GameObject lineRendererPool;
    [SerializeField] public List<GameObject> lines;



    // Start is called before the first frame update
    void Start()
    {

        foreach (Node n in MapGenerator.Instance.nodes)
        {
            foreach (Node a in n.neighbours)
            {
                if (neighbors.Find(x => x.n1 == a && x.n2 == n) == null && neighbors.Find(x => x.n1 == n && x.n2 == a) == null)
                {
                    AddNeighbor(n, a);
                    GameObject l = Instantiate(lineRendererPool, this.transform);
                    l.GetComponent<LineRenderer>().positionCount = 2;
                    l.GetComponent<LineRenderer>().SetPosition(0, n.transform.position);
                    l.GetComponent<LineRenderer>().SetPosition(1, a.transform.position);

                    lines.Add(l);

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {


        for (int i = 0; i < neighbors.Count; i++)
        {
           lines[i].GetComponent<LineRenderer>().positionCount = 2;
           lines[i].GetComponent<LineRenderer>().SetPosition(0, neighbors[i].n1.transform.position);
           lines[i].GetComponent<LineRenderer>().SetPosition(1, neighbors[i].n2.transform.position);
        }

    }
    public void AddNeighbor(Node n1, Node n2)
    {
        NeighborManager.Neighbor n = new NeighborManager.Neighbor();
        n.n1 = n1;
        n.n2 = n2;

        neighbors.Add(n);

    }
}
