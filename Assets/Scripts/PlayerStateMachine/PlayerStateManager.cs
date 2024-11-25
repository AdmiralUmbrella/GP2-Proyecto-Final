using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] public PlayerData playerData;

    private CharacterController characterController;
    private PlayerBaseState currentState;


    // Hacer públicas las referencias a los estados
    public PlayerIdleState IdleState { get; private set; }
    public PlayerHitState HitState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }


    // Hacer público el CharacterController para que los estados puedan usarlo
    public CharacterController CharacterController => characterController;

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    private void Awake()
    {
        // Obtener componentes
        characterController = GetComponent<CharacterController>();

        // Inicializar estados
        IdleState = new PlayerIdleState(this, playerData);
        HitState = new PlayerHitState(this, playerData);
        DeadState = new PlayerDeadState(this, playerData);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void Update()
    {

    }

    public void ChangeState(PlayerBaseState newState)
    {
        // Salir del estado actual
        currentState?.Exit();

        // Entrar al nuevo estado
        currentState = newState;
        currentState?.Enter();
    }

    public void OnMove(InputAction.CallbackContext context)
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }




}