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

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                // Intenta con EnemyBase
                EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
                if (enemyBase != null)
                {
                    enemyBase.TakeDamage(damage);
                    Debug.Log($"Golpeó a {enemy.name}, daño: {damage}");
                    continue;
                }

                // Intenta con BossHealth
                BossHealth bossHealth = enemy.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(damage);
                    Debug.Log($"Golpeó al Boss {enemy.name}, daño: {damage}");
                    continue;
                }

                // Intenta con TreeRegen (NUEVO)
                TreeRegen tree = enemy.GetComponent<TreeRegen>();
                if (tree != null)
                {
                    tree.TakeDamage(damage);
                    Debug.Log($"Golpeó al árbol {enemy.name}, daño: {damage}");
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
