using UnityEngine;

public class EnemyShooterSniper : MonoBehaviour
{
    [Header("Configuración del disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;        // Disparos por segundo (bajo para simular carga)
    public float bulletSpeed = 15f;      // Velocidad alta tipo francotirador
    public float detectionRange = 10f;
    public float aimDuration = 1.5f;     // Tiempo que "apunta" antes de disparar

    private Transform player;
    private float nextFireTime = 0f;
    private bool isAiming = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange && Time.time >= nextFireTime && !isAiming)
        {
            StartCoroutine(AimAndShoot());
        }
    }

    private System.Collections.IEnumerator AimAndShoot()
    {
        isAiming = true;
        Vector2 direction = (player.position - firePoint.position).normalized;

        // 🔴 Línea roja de advertencia (solo visible en SceneView)
        Debug.DrawLine(firePoint.position, player.position, Color.red, aimDuration);

        // Espera mientras apunta
        yield return new WaitForSeconds(aimDuration);

        // Disparo
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        nextFireTime = Time.time + (1f / fireRate);
        isAiming = false;
    }
}
