using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 1.2f;
    public int damage = 1;

    private Transform player;
    private float nextAttackTime = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
        else if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        Debug.Log($"{gameObject.name} golpea al jugador ⚔️");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
