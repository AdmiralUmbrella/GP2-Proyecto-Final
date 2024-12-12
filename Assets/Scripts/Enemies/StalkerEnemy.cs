using UnityEngine;

public class StalkerEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float minDistanceToPlayer = 2f;

    [Header("Vision Settings")]
    [SerializeField] private float playerDetectionAngle = 90f;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Debug")]
    [SerializeField] private bool showDebugRays = true;

    private Transform playerTransform;
    private Camera playerCamera;
    private bool isInPlayerView;
    private Vector3 lastKnownPlayerPosition;

    private void Start()
    {
        // Obtener referencias necesarias
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = Camera.main;
        lastKnownPlayerPosition = transform.position;

        // Verificar referencias
        if (playerTransform == null || playerCamera == null)
        {
            Debug.LogError("StalkerEnemy: No se encontró el jugador o la cámara!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Actualizar estado de visibilidad
        isInPlayerView = IsVisibleToPlayer();

        // Actualizar comportamiento basado en visibilidad
        if (!isInPlayerView)
        {
            MoveTowardsPlayer();
        }

        // Debugging visual
        if (showDebugRays)
        {
            DrawDebugRays();
        }
    }

    private bool IsVisibleToPlayer()
    {
        // 1. Verificar si el enemigo está en el frustum de la cámara
        Vector3 directionToCamera = playerCamera.transform.position - transform.position;
        float dot = Vector3.Dot(playerCamera.transform.forward, -directionToCamera.normalized);

        // Si está fuera del ángulo de visión, no es visible
        if (dot < Mathf.Cos(playerDetectionAngle * 0.5f * Mathf.Deg2Rad))
        {
            return false;
        }

        // 2. Verificar si hay obstáculos entre el jugador y el enemigo
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToCamera, out hit, detectionRange, obstacleLayer))
        {
            // Si golpeamos algo que no es el jugador, no somos visibles
            if (!hit.transform.CompareTag("Player"))
            {
                return false;
            }
        }

        // 3. Verificar si el enemigo está en pantalla
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);
        bool isInViewport = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                           viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                           viewportPoint.z > 0;

        return isInViewport;
    }

    private void MoveTowardsPlayer()
    {
        // Actualizar última posición conocida del jugador
        lastKnownPlayerPosition = playerTransform.position;

        // Calcular distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, lastKnownPlayerPosition);

        // Solo mover si estamos más lejos que la distancia mínima
        if (distanceToPlayer > minDistanceToPlayer)
        {
            // Calcular dirección hacia el jugador
            Vector3 directionToPlayer = (lastKnownPlayerPosition - transform.position).normalized;

            // Mover hacia el jugador sin rotar
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;

            // Mantener una altura fija
            Vector3 fixedPosition = transform.position;
            fixedPosition.y = 4.5f;
            transform.position = fixedPosition;
        }
    }

    private void DrawDebugRays()
    {
        // Dibujar rayo hacia el jugador
        Color rayColor = isInPlayerView ? Color.red : Color.green;
        Debug.DrawLine(transform.position, playerTransform.position, rayColor);

        // Dibujar dirección de vista
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.blue);

    }

    // Método público para consultar el estado de visibilidad
    public bool IsVisible() => isInPlayerView;
}