using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MovimientoTopDown movimientoTopDown;

    void Start()
    {
        movimientoTopDown = GetComponent<MovimientoTopDown>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Animal"))
        {
            AnimalController animalController = collision.gameObject.GetComponent<AnimalController>();
            if (animalController != null)
            {
                movimientoTopDown.AsignarAnimalControlador(animalController);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Animal"))
        {
            movimientoTopDown.AsignarAnimalControlador(null);
        }
    }
}