using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    public float moveSpeed = 4;
    public float life = 3;
    public float damage = 1;
    [Header("Referencias")]
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Start()
    {
        
    }

    void Update()
    {
        Movement();
        Shoot();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(x, y) * moveSpeed;
    }

    void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        if(life <= 0)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
