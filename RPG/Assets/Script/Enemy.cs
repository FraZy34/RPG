using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] float speed;
    private float playerDetectTime;
    public float playerDectectRate;
    public float chaseRange;

    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] int damage;
    [SerializeField] float attackRate;
    private float lastAttackTime;

    [Header("Component")]
    Rigidbody2D rb;
    private PlayerController targetPlayer;

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

    private void FixedUpdate()
    {

        if (targetPlayer != null)
        {
            float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);

            if (dist < attackRange && Time.time - lastAttackTime >= attackRate)
            {
                //attack
                rb.linearVelocity = Vector2.zero;
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
                        }
                    }else if (dist < chaseRange)
                    {
                        if (targetPlayer == null)
                            targetPlayer = player;
                    }
                }
            }
        }
    }
}
