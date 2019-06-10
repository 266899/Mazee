using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knight : MonoBehaviour
{

    public float followRadius = 10f; //Default
    public Transform playerPosition;
    public NavMeshAgent agent;
    public float stoppingDistance;
    public float health;
    public float damage;

    public float smoothTime = 0.1f; // Transition between walking animations time
    [SerializeField] private Animator animator;
    public Material knightMaterial;
    public CapsuleCollider knightCollider;
    private Rigidbody rb;
    public Player player;
    private bool running;
    public AudioSource knightWalking;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        running = false;
        knightMaterial.color = Color.white;
        agent.enabled = false;
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        AnimateWalking();
    }

    public void FollowPlayer()
    {
        // Distance between player and enemy
        float distance = Vector3.Distance(playerPosition.position, transform.position);

        // If player is close, start following
        if (distance <= followRadius)
        {
            
            agent.enabled = true;
            animator.enabled = true;
            agent.SetDestination(playerPosition.position);
            agent.stoppingDistance = stoppingDistance;

            if (!knightWalking.isPlaying)
            {
                knightWalking.Play();
            }

            if (distance <= agent.stoppingDistance + .2f)
            {
                knightWalking.Stop();
                // Face the player
                FacePlayer();
            }

            if (distance <= agent.stoppingDistance +.2f && !running)
            {
                //Attack player
                Attack();
            }
        }
    }

    private void AnimateWalking()
    {
        float movSpeed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("movSpeed", movSpeed, smoothTime, Time.deltaTime);
        

    }

    public void Attack()
    {
        StartCoroutine(WaitSecond());
        animator.SetTrigger("attack");
        FindObjectOfType<AudioManager>().Play("PlayerHit");
        player.playerHealth -= damage;
        Debug.Log(player.playerHealth);
    }

    IEnumerator WaitSecond()
    {
        running = !running;
        yield return new WaitForSeconds(3.2f);
        running = !running;
    }

    public void FacePlayer()
    {
        Vector3 direction = (playerPosition.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.Equals("Weapon"))
        {
            knightMaterial.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag.Equals("Weapon"))
        {
            knightMaterial.color = Color.white;
            Debug.Log("-" + damage);
            health -= damage;
            FindObjectOfType<AudioManager>().Play("KnightHit");

            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Die()
    {
        animator.SetTrigger("dying");
        FindObjectOfType<AudioManager>().Play("KnightDeath");
        knightCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
