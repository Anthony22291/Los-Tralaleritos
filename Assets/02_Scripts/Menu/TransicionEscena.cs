using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TransicionEscena : MonoBehaviour
{
    public Image fadeImage;
    public float duracion = 1f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(FadeOut(nombreEscena));
    }

    IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        for (float t = duracion; t >= 0; t -= Time.deltaTime)
        {
            color.a = t / duracion;
            fadeImage.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOut(string nombreEscena)
    {
        Color color = fadeImage.color;
        for (float t = 0; t <= duracion; t += Time.deltaTime)
        {
            color.a = t / duracion;
            fadeImage.color = color;
            yield return null;
        }
        SceneManager.LoadScene(nombreEscena);
    }
}
