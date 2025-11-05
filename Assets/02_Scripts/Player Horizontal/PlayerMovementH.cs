using UnityEngine;

public class PlayerMovementH : MonoBehaviour
{
    // --- Configuración de Movimiento ---
    public float moveSpeed = 5f;     // Velocidad de movimiento horizontal
    public float jumpForce = 10f;    // Fuerza del salto
    public string groundTag = "Ground"; // Tag que identifica al suelo
    public Transform groundCheck;    // Objeto vacío para detectar el suelo
    public float groundCheckRadius = 0.2f; // Radio para la detección de colisión

    // --- Componentes ---
    private Rigidbody2D rb;
    private Animator animator;

    // --- Variables de Estado (Accesibles por FixedUpdate) ---
    private bool isGrounded; // Sigue siendo necesaria para la lógica de salto en el script

    // ⭐️ CORRECCIÓN: Declaración a nivel de clase para que sea accesible en FixedUpdate.
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Detección del Suelo (CON TAG)
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius);

        isGrounded = false;
        if (collider != null && collider.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
        }

        // 2. Entrada de Movimiento
        moveInput = Input.GetAxisRaw("Horizontal"); // Asignación del valor

        // 3. SALTO (Usa la barra espaciadora por defecto)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 4. Flip del Personaje
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // 5. CONTROL DE ANIMACIONES (USANDO SOLO IsWalking)
        if (isGrounded)
        {
            // En el suelo: IsWalking controla entre Caminar (True) e Idle (False)
            if (Mathf.Abs(moveInput) > 0.01f)
            {
                animator.SetBool("IsWalking", true); // Irá a Caminar
            }
            else
            {
                animator.SetBool("IsWalking", false); // Irá a Idle
            }
        }
        else // No está en el suelo (Saltando/Cayendo)
        {
            // Fuera del suelo: Usamos IsWalking = true para forzar la transición a Jump.
            animator.SetBool("IsWalking", true); // Irá a Jump (desde Idle o Caminar)
        }
    }

    void FixedUpdate()
    {
        // Aplicar Movimiento Horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
}