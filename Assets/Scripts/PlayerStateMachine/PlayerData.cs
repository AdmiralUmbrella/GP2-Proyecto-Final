using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;

    [Header("Sprint Settings")]
    public float sprintSpeed = 8f;
    public float currentSpeed = 5f; // Velocidad actual (walk o sprint)
    public bool isSprinting = false;

    [Header("Stamina System")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float staminaDrainRate = 20f;       // Cuánta stamina se consume por segundo al correr
    public float staminaRegenRate = 10f;       // Cuánta stamina se regenera por segundo
    public float staminaRegenDelay = 1f;       // Tiempo antes de empezar a regenerar
    public bool canSprint = true;              // Flag para controlar si puede correr
    public float timeUntilStaminaRegen = 0f;  // Contador para la regeneración

    [Header("Head Bob Settings")]
    public float bobFrequency = 2.0f;       // Frecuencia del movimiento
    public float bobAmplitude = 0.02f;      // Altura del movimiento
    public float sprintBobMultiplier = 1.5f; // Multiplicador para sprint
    public Vector3 originalCameraPosition;   // Posición original de la cámara

    [Header("Input Settings")]
    public float mouseBaseSensitivity = 0.5f;    // Sensibilidad base para mouse
    public float gamepadBaseSensitivity = 2.5f;  // Sensibilidad base para gamepad
    public float currentSensitivity;             // Sensibilidad actual
    public float maxLookAngle = 80f;

    [Header("References")]
    public Transform cameraTransform;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Ground Check")]
    public float groundCheckRadius = 0.2f;

    [Header("State Info")]
    public bool isGrounded;
    public bool canMove = true;
    public Vector3 moveDirection;
    public Vector2 rotationInput;
    public bool isUsingGamepad;
}
