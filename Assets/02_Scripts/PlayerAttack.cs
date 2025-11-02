using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuración de ataque")]
    public float attackRange = 3f;
    public int damage = 1;
    public float attackCooldown = 0.6f;
    public Transform attackPoint;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            // Ataque con clic izquierdo o tecla J
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void Attack()
    {
        Debug.Log("🪓 Golpe con tubo!");

        // Detección de enemigos cercanos
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
                if (enemyBase != null)
                {
                    enemyBase.TakeDamage(damage);
                    Debug.Log($"Golpeó a {enemy.name}, daño: {damage}");
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
