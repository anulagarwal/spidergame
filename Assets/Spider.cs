using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] Node currentNode;
    [SerializeField] float moveSpeed;
    [SerializeField] Animator animator;
    [SerializeField] float minDistance;
    [SerializeField] public SpiderState currentState;

    //values that will be set in the Inspector
    public Transform Target;
    public float RotationSpeed;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

   
    public static Spider Instance = null;

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
        transform.position = currentNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

         Vector3 targetPostition = new Vector3(currentNode.transform.position.x,                                        
                                        currentNode.transform.position.y,
                                        this.transform.position.z);
      
        if (Vector3.Distance(transform.position, currentNode.transform.position) <= minDistance)
        {
            if (currentNode == NeighborManager.Instance.playerNode)
            {
                //Lose condition
                Invoke("Lose", 1f);
                UpdateState(SpiderState.Bite);

            }
            else
            {
                UpdateState(SpiderState.Idle);
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, currentNode.transform.position, moveSpeed);
            //find the vector pointing from our position to the target


            //_direction = (targetPostition - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            //_lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            // transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);


            var lookPos = targetPostition - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(lookPos);
            lookRot.eulerAngles = new Vector3(lookRot.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * RotationSpeed);


           // this.transform.LookAt(targetPostition);

        }
    }

    public void MoveToNode(Node n)
    {
        if (n != currentNode)
        {
            if (MapGenerator.Instance.GeneratePathTo(currentNode, n) != null)
            {
                currentNode = MapGenerator.Instance.GeneratePathTo(currentNode, n)[1];
                UpdateState(SpiderState.Walk);

            }
            else
            {
                Invoke("Win", 1f);
            }
        }

        
    }

    public void Win()
    {
        GameManager.Instance.WinLevel();
        NeighborManager.Instance.enabled = false;
        this.enabled = false;
    }
    public void Lose()
    {
        GameManager.Instance.LoseLevel();
        NeighborManager.Instance.enabled = false;
        this.enabled = false;
    }
    public void UpdateState(SpiderState state)
    {
        currentState = state;

        switch (currentState)
        {
            case SpiderState.Idle:
                animator.Play("Idle");
                NeighborManager.Instance.enabled = true;

                break;

            case SpiderState.Walk:
                animator.Play("Walk");
                NeighborManager.Instance.enabled = false;
                break;

            case SpiderState.Bite:
                animator.Play("Bite");
                NeighborManager.Instance.enabled = false;
                break;
        }
    }

    public void UpdateNode(Node n)
    {
        currentNode = n;
    }


    public void MoveToPlayer()
    {
        
        MoveToNode(NeighborManager.Instance.playerNode);
    }
}
