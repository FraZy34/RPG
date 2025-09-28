using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    Animator anim;

    [Header("Stat")]
    [SerializeField] float moveSpeed;
    public int currentHealth;
    public int maxHealth;

    [Header("Attack")]
    private float attackTime;
    [SerializeField] float timeBetweenAttack;
    private bool canMove = true;
    [SerializeField] Transform checkEnemy;
    public LayerMask whatIsEnemy;
    public float range;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= attackTime)
            {
                rb.linearVelocity = Vector2.zero;
                anim.SetTrigger("attack");

                StartCoroutine(Delay());

                IEnumerator Delay()
                {
                    canMove = false;
                    yield return new WaitForSeconds(.5f);
                    canMove = true;
                }
                attackTime = Time.time + timeBetweenAttack;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
           
    }

    void Move()
    {
        if (Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1 || Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
        {
            anim.SetFloat("lastInputX", Input.GetAxis("Horizontal"));
            anim.SetFloat("lastInputY", Input.GetAxis("Vertical"));
        }

        if (Input.GetAxis("Horizontal") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x + range, transform.position.y, 0);
        }
        else if (Input.GetAxis("Horizontal") < -0.1)  // aussi corriger le signe ici
        {
            checkEnemy.position = new Vector3(transform.position.x - range, transform.position.y, 0);
        }

        if (Input.GetAxis("Vertical") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y + range, 0);
        }
        else if (Input.GetAxis("Vertical") < -0.1)  // corriger le signe
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y - range, 0);
        }





        float x = Input.GetAxis("Horizontal"); 
        float y = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2(x, y) * moveSpeed * Time.fixedDeltaTime;

        rb.linearVelocity.Normalize();

        if (x !=  0 || y != 0)
        {
            anim.SetFloat("inputX", x);
            anim.SetFloat("inputY", y);
        }
       
    }

    public void OnAttack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(checkEnemy.position, 0.5f, whatIsEnemy);

        foreach (var  enemy_ in enemy)
        {

        }

    }
}
