using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    // Asignar en el Inspector: el SpriteRenderer del extremo del cable (final-cable).
    public SpriteRenderer finalCable;
    // Asignar en el Inspector: el GameObject de la luz (luz-activada).
    public GameObject luz;

    private Vector2 posicionOriginal;
    private Vector2 tamañoOriginal;
    private TareaCables tareaCables;

    void Start()
    {
        // Guardamos la posición y tamaño iniciales para el reset.
        posicionOriginal = transform.position;
        tamañoOriginal = finalCable.size;

        // Buscamos el componente TareaCables en el objeto raíz (TareasCables).
        tareaCables = transform.root.gameObject.GetComponent<TareaCables>();
    }

    // Usamos OnMouseUp() en lugar de Update() + Input.GetMouseButtonUp(0)
    // para asegurarnos de que solo se llama si el objeto fue el que se arrastró.
    private void OnMouseUp()
    {
        // Si el componente 'Cable' aún existe (es decir, no se ha conectado), 
        // reiniciamos el cable a su posición original.
        Reiniciar();
    }

    private void OnMouseDrag()
    {
        // 1. Mueve el cable a la posición del ratón.
        ActualizarPosicion();
        // 2. Comprueba si está cerca de un conector compatible.
        ComprobarConexion();
        // 3. Rota el cable para apuntar al ratón.
        ActualizarRotacion();
        // 4. Estira el sprite del cable.
        ActualizarTamaño();
    }

    private void ActualizarPosicion()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // El conector 'móvil' es el que se mueve.
        transform.position = mousePosition;
    }

    private void ActualizarRotacion()
    {
        Vector2 posicionActual = transform.position;
        // La posición de origen es el padre (el cable en el lado izquierdo o derecho).
        Vector2 puntoOrigen = transform.parent.position;

        Vector2 direccion = posicionActual - puntoOrigen;

        // Calcula el ángulo de rotación. Multiplicamos por la escala local para que funcione correctamente.
        float angulo = Vector2.SignedAngle(Vector2.right * transform.lossyScale.x, direccion);

        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }

    private void ActualizarTamaño()
    {
        Vector2 posicionActual = transform.position;
        Vector2 puntoOrigen = transform.parent.position;

        // Distancia entre el origen y la posición actual del conector.
        float distancia = Vector2.Distance(posicionActual, puntoOrigen);

        // Asigna la distancia como el nuevo ancho del SpriteRenderer 'finalCable'.
        finalCable.size = new Vector2(distancia, finalCable.size.y);
    }

    private void Reiniciar()
    {
        // Restaura la posición, rotación y tamaño originales.
        transform.position = posicionOriginal;
        transform.rotation = Quaternion.identity;
        finalCable.size = tamañoOriginal;
    }

    private void ComprobarConexion()
    {
        // Busca todos los colliders dentro de un radio de 0.2 alrededor del conector móvil.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

        foreach (Collider2D col in colliders)
        {
            // Asegúrate de que no estás colisionando con el conector del mismo cable que mueves.
            if (col.gameObject != gameObject)
            {
                // Intenta obtener el componente Cable del objeto con el que colisionamos.
                Cable otroCable = col.gameObject.GetComponent<Cable>();

                // Si colisionamos con otro conector (tiene el componente Cable)
                if (otroCable != null)
                {
                    // Comprobamos si los colores de los extremos del cable coinciden.
                    if (finalCable.color == otroCable.finalCable.color)
                    {
                        // *** CONEXIÓN CORRECTA ***

                        // Fija la posición del cable en el punto de conexión.
                        transform.position = col.transform.position;
                        // Forzamos la rotación y el tamaño para que se vea perfectamente conectado.
                        ActualizarRotacion();
                        ActualizarTamaño();

                        // Conectamos ambos cables (activamos luz y destruimos el script Cable de ambos).
                        Conectar();
                        otroCable.Conectar();

                        // Actualizamos el contador de la tarea principal.
                        tareaCables.conexionesActuales++;
                        // Comprobamos si ya hemos ganado.
                        tareaCables.ComprobarVictoria();
                    }
                }
            }
        }
    }

    public void Conectar()
    {
        // 1. Activa la luz de conexión.
        luz.SetActive(true);
        // 2. Desactiva el script Cable (para que no se pueda mover o reiniciar).
        Destroy(this);
    }
}