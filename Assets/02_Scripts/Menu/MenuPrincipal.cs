using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void BtnJugar()
    {
        SceneManager.LoadScene("El-Despertar");
    }

 
   public void BtnSalir()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
            
    }
}
