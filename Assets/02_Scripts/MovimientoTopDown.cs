using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoTopDown : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento = 5f;
    [SerializeField] private Vector2 direccion;
    private Rigidbody2D rb2D;

    private bool estaMontado = false;
    private AnimalController animalControlador;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && animalControlador != null)
        {
            estaMontado = !estaMontado;

            if (estaMontado)
            {
                animalControlador.Montar(transform);
            }
            else
            {
                animalControlador.Desmontar();
            }
        }

        if (!estaMontado)
        {
            direccion = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
    }

    private void FixedUpdate()
    {
        if (!estaMontado)
        {
            rb2D.MovePosition(rb2D.position + direccion * velocidadMovimiento * Time.fixedDeltaTime);
        }

        if (estaMontado && animalControlador != null) // Doble chequeo para prevenir NullReferenceException
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            animalControlador.Mover(horizontalInput);
        }
    }

    public void AsignarAnimalControlador(AnimalController controlador)
    {
        animalControlador = controlador;

        // Desmontar automáticamente si la referencia se pierde mientras estás montado
        if (controlador == null && estaMontado)
        {
            estaMontado = false;
        }
    }
}