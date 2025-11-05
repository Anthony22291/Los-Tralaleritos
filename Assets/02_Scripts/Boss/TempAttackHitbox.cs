using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAttackHitbox : MonoBehaviour
{
    public int damage = 5;
    public float lifeTime = 0.3f;
    private bool hasHitPlayer = false;

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitPlayer) return;
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage);
            hasHitPlayer = true;
        }
    }
}

