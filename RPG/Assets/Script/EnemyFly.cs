using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyFly : EnemyStats
{

    public Transform attackPoint;
    public LayerMask playerLayerMask;


    void Start()
    {
        InitializeCharacter(OnAttack);
    }

    void OnAttack()
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
