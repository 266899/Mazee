using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TitanBoss : MonoBehaviour
{

    public float followRadius = 10f; //Default
    public Transform playerPosition;
    public NavMeshAgent agent;
    public float stoppingDistance;
    public float health;
    public float damage;
    public float smoothTime = 0.1f; // Transition between walking animations time
    [SerializeField] private Animator animator;

    public Material titanMaterial;
    public CapsuleCollider titanCollider;
    private Rigidbody rb;
    public Player player;
    private bool running;
    public static bool alive;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        running = false;
        titanMaterial.color = Color.white;
        alive = true;
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
            agent.SetDestination(playerPosition.position);
            agent.stoppingDistance = stoppingDistance;

            if (distance <= agent.stoppingDistance)
            {
                // Face the player
                FacePlayer();
            }

            if (distance <= agent.stoppingDistance && !running)
            {
                //Attack player
                Debug.Log("atatcked");
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
        int randomAttack = (int)Mathf.Round(Random.Range(1f, 2f));
        Debug.Log("asd"  + randomAttack);
        animator.SetTrigger("attack" +  randomAttack);
        player.playerHealth -= damage;
        FindObjectOfType<AudioManager>().Play("PlayerHit");
        Debug.Log(player.playerHealth);
    }

    IEnumerator WaitSecond()
    {
        running = !running;
        yield return new WaitForSeconds(3f);
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
            titanMaterial.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag.Equals("Weapon"))
        {
            titanMaterial.color = Color.white;
            Debug.Log("-" + "damage");
            health -= damage;
            FindObjectOfType<AudioManager>().Play("BossHit");

            if (health <= 0 && alive)
            {
                StartCoroutine(Die());
            }
        }
    }
    
    IEnumerator Die()
    {
        animator.SetTrigger("dying");
        titanCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        alive = false;
    }

}
