using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NeighborManager : MonoBehaviour
{

    [System.Serializable]
    public class Neighbor
    {
        public Node n1;
        public Node n2;
        public int health = 1;
        public TextMeshPro text;
    }


    [SerializeField] public List<Neighbor> neighbors;
    [SerializeField] public Node playerNode;
    [SerializeField] public GameObject lineRendererPool;
    [SerializeField] public List<GameObject> lines;

    public bool hasCutInSwipe = false;
    public static NeighborManager Instance = null;

    private void Awake()
    {
        Application.targetFrameRate = 100;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(Node n in MapGenerator.Instance.nodes)
        {
            foreach (Node a in n.neighbours)
            {
                if (neighbors.Find(x => x.n1 == a && x.n2 == n) == null && neighbors.Find(x => x.n1 == n && x.n2 == a) == null)
                {
                    GameObject l = Instantiate(lineRendererPool, this.transform);
                    l.transform.position = (n.transform.position + a.transform.position) / 2;
                    l.GetComponent<LineRenderer>(). positionCount = 2;
                    l.GetComponent<LineRenderer>().SetPosition(0, n.transform.position);
                    l.GetComponent<LineRenderer>().SetPosition(1, a.transform.position);
                    AddNeighbor(n, a, l.GetComponentInChildren<TextMeshPro>());

                    lines.Add(l);

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasCutInSwipe = false;
        }
        //to draw all raycast
        if (Input.GetMouseButton(0))
        {
            foreach (Neighbor n in neighbors)
            {
                Vector3 fromPosition = n.n1.transform.position;
                Vector3 toPosition = n.n2.transform.position;
                Vector3 direction = toPosition - fromPosition;

                RaycastHit hit;
                if (Physics.Linecast(fromPosition, toPosition, out hit))
                {

                    if (hit.collider.tag == "Knife" && !hasCutInSwipe)
                    {
                        DamageNeighbour(n);
                        hasCutInSwipe = true;
                        Invoke("SendSpiderToPlayer", 0.5f);
                        return;
                    }
                }
            }
        }
        

    }

    public void SendSpiderToPlayer()
    {
        Spider.Instance.MoveToPlayer();
    }

    public void DamageNeighbour(Neighbor n)
    {
        n.health--;
        n.text.text = n.health + "";
        if (n.health <= 0)
        {
            RemoveNeighbor(n);
        }
    }
    public void RemoveNeighbor(Neighbor n)
    {
        n.n1.neighbours.Remove(n.n2);
        n.n2.neighbours.Remove(n.n1);
        Destroy(lines[neighbors.FindIndex(x => x == n)]);
        lines.Remove(lines[neighbors.FindIndex(x => x == n)]);
        neighbors.Remove(n);
    }
    public void AddNeighbor(Node n1, Node n2, TextMeshPro tp)
    {
        Neighbor n = new Neighbor();
        n.n1 = n1;
        n.n2 = n2;
        n.text = tp;
        neighbors.Add(n);

    }
}
