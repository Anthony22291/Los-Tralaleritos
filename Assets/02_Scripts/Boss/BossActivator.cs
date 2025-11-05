using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [Header("Referencias")]
    public BossController boss;

    [Header("Configuración")]
    public bool activateOnce = true; // Si solo se activa una vez

    private bool hasActivated = false;

    void Start()
    {
        // Desactivar el boss al inicio
        if (boss != null)
        {
            boss.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            ActivateBoss();

            if (activateOnce)
            {
                hasActivated = true;
            }
        }
    }

    void ActivateBoss()
    {
        if (boss != null)
        {
            boss.enabled = true;
            Debug.Log("🔥 Boss activado!");

            // Opcional: Destruir el trigger después de activar
            if (activateOnce)
            {
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
