using UnityEngine;

public class TreeRegen : MonoBehaviour
{
    [Header("Estado del Árbol")]
    public bool IsAlive = true;

    [Header("Vida")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("Efectos Opcionales")]
    public GameObject destroyEffect; // Opcional: efecto de partículas al destruirse

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive) return; // No recibir más daño si ya está muerto

        currentHealth -= damage;
        Debug.Log($"Árbol recibe {damage} de daño. Vida restante: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        IsAlive = false;
        Debug.Log("🌳 Árbol destruido!");

        // Opcional: instanciar efecto de destrucción
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // Destruir el árbol después de un pequeño delay
        Destroy(gameObject, 0.1f);
    }
}
