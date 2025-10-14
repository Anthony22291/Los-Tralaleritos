using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MullyMovement : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody2D rb;

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 5f;

    public enum CameraMode { TopDown, SideScroller }
    [Header("Cámaras")]
    public CameraMode currentCamera = CameraMode.TopDown; // Inicia con cámara principal

    [Header("Ground Check")]
    public Transform groundCheck;       // Punto para chequear si Mully está en el suelo
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;       // Capas que se consideran suelo
    private bool isGrounded;
    private bool canDoubleJump = false;

    [Header("Climb Check")]
    public Transform edgeCheck;         // Punto para detectar bordes
    public float edgeCheckDistance = 0.5f;
    public LayerMask climbableLayer;
    private bool canClimb = false;
    public float climbSpeed = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Siempre revisar input
        CheckGrounded();
        CheckEdge();

        Move();
        Jump();
        Climb();
    }

    #region Movimiento
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = 0f;

        if (currentCamera == CameraMode.TopDown)
        {
            rb.gravityScale = 0f; // Sin gravedad en top-down
            moveY = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
        }
        else if (currentCamera == CameraMode.SideScroller)
        {
            rb.gravityScale = 1f; // Gravedad normal en plataforma
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
        }
    }
    #endregion

    #region Salto
    void Jump()
    {
        if (currentCamera != CameraMode.SideScroller) return;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = true;
        }
        else if (!isGrounded && canDoubleJump && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            canDoubleJump = false;
        }
    }
    #endregion

    #region Chequeos
    void CheckGrounded()
    {
        // Raycast hacia abajo para detectar el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void CheckEdge()
    {
        if (currentCamera != CameraMode.SideScroller) return;

        // Raycast hacia adelante para detectar borde
        RaycastHit2D hit = Physics2D.Raycast(edgeCheck.position, Vector2.right, edgeCheckDistance, climbableLayer);
        canClimb = hit.collider != null;

        // Debug visual
        Debug.DrawRay(edgeCheck.position, Vector2.right * edgeCheckDistance, Color.red);
    }
    #endregion

    #region Trepar bordes
    void Climb()
    {
        if (currentCamera != CameraMode.SideScroller) return;

        if (canClimb && Input.GetAxisRaw("Vertical") > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
        }
    }
    #endregion
}
