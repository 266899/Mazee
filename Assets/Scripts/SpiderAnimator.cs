using UnityEngine;
using UnityEngine.AI;

public class SpiderAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    public float smoothTime = 0.1f; // Transition between animations time

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        float movSpeed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("movSpeed", movSpeed, smoothTime, Time.deltaTime);
        //fix
        animator.SetBool("inRange", true);
    }
}
