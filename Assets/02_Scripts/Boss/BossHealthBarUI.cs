using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("Referencias")]
    public Slider healthSlider;
    public BossHealth bossHealth;

    [Header("Opcional - Suavizado")]
    public bool useSmoothTransition = true;
    public float smoothSpeed = 5f;

    [Header("Opcional - Gradient de Color")]
    public bool useColorGradient = true;
    public Gradient healthGradient;
    public Image fillImage;

    private float targetHealth;
    [Header("Visibilidad")]
    public bool hideWhenInactive = true;
    public GameObject healthBarContainer;
    void Start()
    {
        if (bossHealth != null)
        {
            healthSlider.maxValue = bossHealth.maxHealth;
            healthSlider.value = bossHealth.maxHealth;
            targetHealth = bossHealth.maxHealth;
        }

        // Configurar gradient por defecto
        if (useColorGradient && healthGradient.colorKeys.Length == 0)
        {
            SetupDefaultGradient();
        }
    }

    void Update()
    {
        if (bossHealth == null) return;

        // Ocultar si el boss no está activo
        if (hideWhenInactive && healthBarContainer != null)
        {
            BossController controller = bossHealth.GetComponent<BossController>();
            bool bossActive = controller != null && controller.enabled;
            healthBarContainer.SetActive(bossActive);
        }
        if (bossHealth == null) return;

        targetHealth = bossHealth.GetCurrentHealth();

        // Transición suave o instantánea
        if (useSmoothTransition)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealth, Time.deltaTime * smoothSpeed);
        }
        else
        {
            healthSlider.value = targetHealth;
        }

        // Cambiar color según la vida
        if (useColorGradient && fillImage != null)
        {
            float normalizedHealth = healthSlider.value / healthSlider.maxValue;
            fillImage.color = healthGradient.Evaluate(normalizedHealth);
        }
    }

    void SetupDefaultGradient()
    {
        // Rojo (0%) → Amarillo (50%) → Verde (100%)
        GradientColorKey[] colorKeys = new GradientColorKey[3];
        colorKeys[0].color = Color.red;
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = Color.yellow;
        colorKeys[1].time = 0.5f;
        colorKeys[2].color = Color.green;
        colorKeys[2].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        healthGradient.SetKeys(colorKeys, alphaKeys);
    }

}
