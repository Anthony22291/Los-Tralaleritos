using UnityEngine;

public class EnergySource : MonoBehaviour
{
    [Header("Configuración de Energía")]
    public float energiaOtorgada = 25f; // Cantidad de energía que otorga

    private bool jugadorCerca = false;
    private PlayerEnergy energiaJugador; // Referencia al script del jugador

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            energiaJugador = other.GetComponent<PlayerEnergy>();

            Debug.Log("🔵 Jugador entró en el rango de energía."); // DEBUG
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            energiaJugador = null;

            Debug.Log("🔴 Jugador salió del rango de energía."); // DEBUG
        }
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (energiaJugador != null)
            {
                energiaJugador.AbsorberEnergia(energiaOtorgada);
                Debug.Log("⚡ Energía absorbida: " + energiaOtorgada);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("⚠️ No se encontró el componente PlayerEnergy en el jugador.");
            }
        }
    }
}
