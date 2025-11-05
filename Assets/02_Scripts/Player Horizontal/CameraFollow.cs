using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // Controla el suavizado, un valor más pequeño es más suave.
    [Range(0.01f, 1f)]
    public float smoothFactor = 0.05f;

    // El desplazamiento (offset) en los ejes X e Y
    public Vector2 cameraOffset = new Vector2(0f, 2f);

    // La profundidad (Z) de la cámara. Debe ser un valor constante y negativo (ej. -10).
    private const float Z_CAMERA_DEPTH = -10f;

    // Usamos LateUpdate para asegurar que el jugador ha terminado de moverse en el frame.
    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("SmoothCameraFollow: ¡El objetivo (Target) no ha sido asignado!");
            return;
        }

        // 1. Definir la posición de destino SOLO en X y Y.
        // La cámara solo seguirá al Target en X e Y, manteniendo su propia Z.
        Vector3 targetPosition = new Vector3(
            target.position.x + cameraOffset.x,
            target.position.y + cameraOffset.y,
            Z_CAMERA_DEPTH // Mantiene la profundidad de la cámara constante
        );

        // 2. Aplicar suavizado (Interpolación Lineal - Lerp)
        // La cámara se moverá desde su posición actual hacia la posición de destino.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor);

        // 3. Asignar la nueva posición
        transform.position = smoothedPosition;
    }
}