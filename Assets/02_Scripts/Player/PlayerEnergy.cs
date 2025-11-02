using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Energía del jugador")]
    public float maxEnergy = 100f;
    public float currentEnergy;

    [Header("Referencia a la barra de energía")]
    public Image energyBarFill;

  

    void Start()
    {
        currentEnergy = 0f;
   
        ActualizarBarra();
    }

    void ActualizarBarra()
    {
        if (energyBarFill != null)
            energyBarFill.fillAmount = currentEnergy / maxEnergy;
    }

    public void AbsorberEnergia(float cantidad)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + cantidad, 0f, maxEnergy);
        ActualizarBarra();
        Debug.Log("Energía absorbida: " + currentEnergy);
    }

    public bool UsarEnergiaParaCurar(float cantidad)
    {
        if (currentEnergy >= cantidad)
        {
            currentEnergy -= cantidad;
            ActualizarBarra();
            return true;
        }
        return false;
    }

    public bool UsarEnergiaParaMecanismo(float cantidad)
    {
        if (currentEnergy >= cantidad)
        {
            currentEnergy -= cantidad;
            ActualizarBarra();
            Debug.Log("Mecanismo activado usando energía.");
            return true;
        }
        else
        {
            Debug.Log("No hay energía suficiente.");
            return false;
        }
    }
}
