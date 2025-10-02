using System.Collections;
using UnityEngine;
using Pathfinding;

public class Enemy : EnemyStats
{

    [Header("Stats")]
    [SerializeField] float speed;
    private float playerDetectTime;
    public float playerDectectRate = 0.2f;
    public float chaseRange;
    bool lookRight;

    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    private float lastAttackTime;
    public Transform attackPoint;
    public LayerMask playerLayerMask;

    [Header("Component")]
    Rigidbody2D rb;
    private PlayerController targetPlayer;
    Animator anim;

    [Header("Pathfinding")]
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWayPoint = 0;
    bool reachEndPath = false;
    Seeker seeker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        InitializeBar();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && targetPlayer != null)
        {
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }
    }

    private void Update()
    {
        if (rb.linearVelocity.x > 0 && lookRight || rb.linearVelocity.x < 0 && !lookRight)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {

        if (targetPlayer != null)
        {
            float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);

            if (dist < attackRange && Time.time - lastAttackTime >= attackRate)
            {
                StartCoroutine(Delay());

                IEnumerator Delay()
                {
                    yield return new WaitForSeconds(Random.Range(0.1f, 1f));
                    if (dist < attackRange)
                    {
                        anim.SetTrigger("attack");
                        rb.linearVelocity = Vector2.zero;
                    }
                }
                lastAttackTime = Time.time;
               
               

            }
            else if (dist > attackRange)
            {
                if (path == null)
                    return;

                if (currentWayPoint >= path.vectorPath.Count)
                {
                    reachEndPath = true;
                    return;
                }
                else
                {
                    reachEndPath = false;
                }

                Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
                Vector2 force = direction * speed * Time.fixedDeltaTime;

                rb.linearVelocity = force;

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWayPoint++; 
                }
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        

        DetectPlayer();

    }

    void DetectPlayer()
    {
        if (Time.time - playerDetectTime > playerDectectRate)
        {
            playerDetectTime = Time.time;

            foreach (PlayerController player in Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None))

            {
                if (player != null)
                {
                    float dist = Vector2.Distance(transform.position, player.transform.position);

                    if (player == targetPlayer)
                    {
                        if (dist > chaseRange)
                        {
                            targetPlayer = null;
                            rb.linearVelocity = Vector2.zero;
                            anim.SetBool("onMove", false);
                        }
                    }else if (dist < chaseRange)
                    {
                        if (targetPlayer == null)
                            targetPlayer = player;
                        anim.SetBool("onMove", true);
                    }
                }
            }
        }
    }

    void Flip()
    {
        lookRight = !lookRight;

        transform.Rotate(0, 180f,  0);
    }

    void Attack()
    {
        Collider2D player = Physics2D.OverlapCircle(attackPoint.transform.position, 0.5f, playerLayerMask);

        if (player != null && player.tag == "Player")
        {
            player.GetComponent<PlayerController>().TakeDamage(damage);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position, 0.5f);
    }
}
