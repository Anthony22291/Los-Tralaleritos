using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Vida del Boss")]
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Boss recibe {damage} de daño. Vida restante: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // No superar la vida máxima
        Debug.Log($"Boss curado {amount}. Vida actual: {currentHealth}/{maxHealth}");
    }

    void Die()
    {
        Debug.Log("?? Boss ha sido derrotado");
        // Aquí puedes agregar efectos, animaciones de muerte, drops, etc.
        Destroy(gameObject);
    }

    // Método opcional para mostrar la vida en Gizmos
    void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(transform.position + Vector3.up * 2f,
            $"Vida: {currentHealth}/{maxHealth}");
    }
}
