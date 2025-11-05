using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaCables : MonoBehaviour
{
    // Esta variable debe ser asignada en el Inspector a 0, 
    // y se incrementará cada vez que se haga una conexión correcta.
    public int conexionesActuales;

    public void ComprobarVictoria()
    {
        // El juego termina cuando hay 4 conexiones correctas.
        if (conexionesActuales == 4)
        {
            // Destruye el objeto raíz de la tarea después de 1 segundo.
            Destroy(this.gameObject, 1f);
        }
    }
}