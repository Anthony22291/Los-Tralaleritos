using UnityEngine;

public class EnergyDoor : MonoBehaviour
{
   
    public float costoEnergia = 5f; 

 
    public Collider2D puertaCollider;
    private bool abierta = false;

    private void Start()
    {
        if (puertaCollider == null)
            puertaCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerEnergy player = other.GetComponent<PlayerEnergy>();

        if (player != null)
        {
            if (!abierta)
            {
                if (player.UsarEnergiaParaMecanismo(costoEnergia))
                {
                    AbrirPuerta();
                }
                else
                {
                    Debug.Log(" No tienes suficiente energía para abrir la puerta.");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (abierta && other.GetComponent<PlayerEnergy>() != null)
        {
            CerrarPuerta();
        }
    }

    private void AbrirPuerta()
    {
        abierta = true;
        if (puertaCollider != null)
            puertaCollider.enabled = false; 
        Debug.Log(" Puerta abierta.");
    }

    private void CerrarPuerta()
    {
        abierta = false;
        if (puertaCollider != null)
            puertaCollider.enabled = true; 
        Debug.Log(" Puerta cerrada.");
    }
}
