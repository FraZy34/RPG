using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;

    [Header("Stat")]
    [SerializeField]
    float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float y = Input.GetAxisRaw("Vertical") * moveSpeed * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(x, y);

        rb.linearVelocity.Normalize();
    }
}
