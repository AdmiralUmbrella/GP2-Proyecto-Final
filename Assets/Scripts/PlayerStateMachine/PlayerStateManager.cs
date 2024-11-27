using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    // Referencias de componentes
    private CharacterController characterController;
    private Camera playerCamera;
    private PlayerBaseState currentState;
    private PlayerInput playerInput;

    // Variables de cámara
    private float verticalRotation = 0f;

    // Variables de movimiento
    private Vector3 playerVelocity;

    //Estados
    public PlayerIdleState IdleState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }

    private void Awake()
    {
        // Obtener referencias
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        if (playerData.cameraTransform != null)
        {
            playerCamera = playerData.cameraTransform.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("Camera Transform no está asignado en PlayerData!");
        }

        // Configuración inicial de sensibilidad
        UpdateInputType(Gamepad.current != null);

        // Configurar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inicializar estados
        IdleState = new PlayerIdleState(this, playerData);
        SprintState = new PlayerSprintState(this, playerData);

        // Estado inicial
        ChangeState(IdleState);
    }

    #region UPDATES
    private void Update()
    {
        // Verificar cambios en el tipo de input
        CheckInputTypeChange();

        UpdateGroundCheck();
        UpdateMovement();
        UpdateRotation();

        // Actualizar el estado actual
        currentState?.Update();
    }

    private void UpdateGroundCheck()
    {
        playerData.isGrounded = Physics.CheckSphere(
            playerData.groundCheck.position,
            playerData.groundCheckRadius,
            playerData.groundLayer
        );

        if (playerData.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
    }

    private void UpdateMovement()
    {
        if (!playerData.canMove) return;

        Vector3 move = transform.right * playerData.moveDirection.x +
                      transform.forward * playerData.moveDirection.z;

        // Usar currentSpeed en lugar de walkSpeed
        characterController.Move(move * playerData.currentSpeed * Time.deltaTime);
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void CheckInputTypeChange()
    {
        bool isGamepadNowConnected = Gamepad.current != null;
        if (playerData.isUsingGamepad != isGamepadNowConnected)
        {
            UpdateInputType(isGamepadNowConnected);
        }
    }

    private void UpdateInputType(bool isGamepad)
    {
        playerData.isUsingGamepad = isGamepad;
        playerData.currentSensitivity = isGamepad ?
            playerData.gamepadBaseSensitivity :
            playerData.mouseBaseSensitivity;

        Debug.Log($"Input type changed. Using {(isGamepad ? "Gamepad" : "Mouse")} " +
                  $"with sensitivity: {playerData.currentSensitivity}");
    }

    private void UpdateRotation()
    {
        if (!playerData.canMove) return;

        // Rotación horizontal del cuerpo
        transform.Rotate(Vector3.up * playerData.rotationInput.x *
                        playerData.currentSensitivity);

        // Rotación vertical de la cámara
        verticalRotation -= playerData.rotationInput.y * playerData.currentSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation,
                                     -playerData.maxLookAngle,
                                     playerData.maxLookAngle);

        if (playerCamera != null)
        {
            playerData.cameraTransform.localRotation =
                Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }
    #endregion


    #region Input System Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!playerData.canMove) return;
        Vector2 input = context.ReadValue<Vector2>();
        playerData.moveDirection = new Vector3(input.x, 0, input.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!playerData.canMove) return;
        playerData.rotationInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!playerData.canMove) return;

        if (context.started)
        {
            playerData.isSprinting = true;
            if (IsMoving())
            {
                ChangeState(SprintState);
            }
        }
        else if (context.canceled)
        {
            playerData.isSprinting = false;
            ChangeState(IdleState);
        }
    }
    #endregion

    #region Métodos de Estado
    public void ChangeState(PlayerBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    #endregion

    #region Getters públicos
    public bool IsMoving() => playerData.moveDirection.magnitude > 0.1f;
    public CharacterController GetCharacterController() => characterController;
    public float GetStaminaPercentage()
    {
        return playerData.currentStamina / playerData.maxStamina;
    }
    #endregion

    #region Debugging
    private void OnGUI()
    {
        // Mostrar barra de stamina temporal para testing
        GUI.Box(new Rect(20, 20, 200, 20), "");
        GUI.Box(new Rect(20, 20, 200 * GetStaminaPercentage(), 20), "Stamina");
    }
    #endregion
}