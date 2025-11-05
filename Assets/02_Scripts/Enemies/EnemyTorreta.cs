using UnityEngine;

public class EnemyTorreta : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject bulletPrefab; // ¡Arrastra el prefab de la Bala aquí!
    public Transform firePoint;     // Punto desde donde sale la bala (objeto vacío)

    // 💥 ¡CAMBIO! Dispara cada 0.5 segundos (dos veces por segundo)
    public float fireRate = 0.5f;

    private float nextFireTime;

    void Start()
    {
        // El primer disparo ocurre inmediatamente o después del primer intervalo.
        nextFireTime = Time.time;
    }

    void Update()
    {
        // Comprueba si ya ha pasado el tiempo desde el último disparo
        if (Time.time > nextFireTime)
        {
            Shoot();
            // Establece el tiempo del próximo disparo
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Instancia la bala en la posición y rotación del 'firePoint'
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}