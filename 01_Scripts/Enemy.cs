using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    public EnemyType type;
    public float life = 2;
    public float damage = 1;
    public float moveSpeed = 6;
    public float timeBtwShoot = 1.5f;
    float timer = 0;
    public float detectionRange = 5;
    bool targetInRange = false;
    [Header("Referencias")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    Transform target;

    void Start()
    {
        target = GameObject.FindWithTag("Player")?.transform;
        moveSpeed += Random.Range(-2f, 2f);
    }
    void Update()
    {
        switch (type)
        {
            case EnemyType.Normal:
                NormalMovement();
                break;
            case EnemyType.NormalShoot:
                NormalMovement();
                NormalShoot();
                break;
            case EnemyType.Kamikase:
                if (!targetInRange)
                {
                    NormalMovement();
                    SearchTarget();
                }
                else
                {
                    RotateToTarget();
                    ModifiedMovement(2);
                }
                break;
            case EnemyType.Sniper:
                if (!targetInRange)
                {
                    NormalMovement();
                    SearchTarget();
                }
                else
                {
                    RotateToTarget();
                    NormalShoot();
                }
                break;
        }
    }

    void NormalMovement()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
    }

    void ModifiedMovement(float value)
    {
        transform.Translate(Vector2.up * moveSpeed * value * Time.deltaTime);
    }

    void SearchTarget()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            if(distance <= detectionRange)
            {
                targetInRange = true;
            }
            else
            {
                targetInRange = false;
            }
        }
    }

    void RotateToTarget()
    {
        Vector2 dir = target.position - transform.position;
        float angleZ = Mathf.Atan2 (dir.x, dir.y) * Mathf.Rad2Deg - 0;
        transform.rotation = Quaternion.Euler(0, 0, - angleZ);
    }

    void NormalShoot()
    {
        if(timer < timeBtwShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

public enum EnemyType
{
    Normal,
    NormalShoot,
    Kamikase,
    Sniper
}
