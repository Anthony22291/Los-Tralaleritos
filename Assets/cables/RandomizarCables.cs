using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizarCables : MonoBehaviour
{
    private void Start() // Se usa Start en lugar de Awake para asegurar el orden.
    {
        // Lista para guardar las posiciones iniciales de todos los cables.
        List<Vector3> posiciones = new List<Vector3>();

        // 1. Guardar todas las posiciones de los hijos.
        for (int i = 0; i < transform.childCount; i++)
        {
            posiciones.Add(transform.GetChild(i).position);
        }

        // 2. Asignar las posiciones de forma aleatoria a los hijos.
        // Usamos un bucle for invertido para poder remover elementos de la lista.
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject cableActual = transform.GetChild(i).gameObject;

            // Elegir una posición aleatoria de la lista de posiciones disponibles.
            int indiceAleatorio = Random.Range(0, posiciones.Count);

            // Asignar la posición aleatoria al cable.
            cableActual.transform.position = posiciones[indiceAleatorio];

            // Remover la posición de la lista para no asignarla de nuevo.
            posiciones.RemoveAt(indiceAleatorio);
        }
    }
}