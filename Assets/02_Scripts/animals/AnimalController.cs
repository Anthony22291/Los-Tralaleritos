using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private bool isMounted = false;
    private Rigidbody2D rb;
    private Transform player;

    public float moveSpeed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isMounted)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }

    public void Montar(Transform jugador)
    {
        if (!isMounted)
        {
            isMounted = true;
            player = jugador;
            player.SetParent(transform);
            player.localPosition = Vector3.zero;
        }
    }

    public void Desmontar()
    {
        isMounted = false;
        if (player != null)
        {
            player.SetParent(null);
        }
    }

    public void Mover(float horizontalInput)
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }
}