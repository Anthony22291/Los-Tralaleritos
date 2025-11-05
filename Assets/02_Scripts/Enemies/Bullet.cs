using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuración de la Bala")]
    public float speed = 10f; // 👈 ¡NUEVO! Velocidad de la bala
    public float lifeTime = 5f;
    public int damage = 1;

    void Start()
    {
        // Programa la destrucción de la bala después del tiempo de vida.
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 👈 ¡NUEVO! Mueve la bala en línea recta (hacia transform.right)
        // Esto asume que la torreta ha rotado el FirePoint para apuntar correctamente.
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // --- 1. DESTRUCCIÓN POR SUELO (Ground) ---
        if (other.CompareTag("Suelo")) // 👈 ¡NUEVO!
        {
            Destroy(gameObject);
            return; // Detiene la ejecución para no chequear otros tags.
        }

        // --- 2. DESTRUCCIÓN POR ENEMIGO (Si se dispara entre enemigos) ---
        else if (other.CompareTag("Enemy"))
        {
            // Opcional: Aquí podrías añadir lógica de daño a enemigos si esta bala fuera del jugador.
            // Para la torreta, simplemente la destruimos.
            Destroy(gameObject);
            return;
        }

        // --- 3. IMPACTO EN EL JUGADOR ---
        else if (other.CompareTag("Player"))
        {
            // Busca el componente de salud en el jugador
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // Destruye la bala después de hacer daño
            return;
        }

        // Puedes añadir aquí otros elementos que la bala deba destruir (ej: paredes, escudos)
    }
}