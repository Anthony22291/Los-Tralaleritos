using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPlayer : MonoBehaviour
{
    public float velocidad = 5f;       // Velocidad horizontal
    public float fuerzaSalto = 10f;    // Potencia del salto
    private bool enSuelo = false;      // Saber si está tocando el suelo

    private Rigidbody2D rb;
    private SpriteRenderer sr;         // Para voltear el sprite

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movimiento horizontal
        float mover = Input.GetAxisRaw("Horizontal"); // A/D o Flechas
        rb.velocity = new Vector2(mover * velocidad, rb.velocity.y);

        // Voltear sprite según dirección
        if (mover > 0) sr.flipX = false;
        if (mover < 0) sr.flipX = true;

        // Salto
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            enSuelo = false; // Evita doble salto
        }
    }

    // Detectar si está tocando el suelo (Tilemap Collider)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }
}
