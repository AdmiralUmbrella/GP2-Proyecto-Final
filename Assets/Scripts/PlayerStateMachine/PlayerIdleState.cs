using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private float idleTime = 0f;

    public PlayerIdleState(PlayerStateManager manager, PlayerData playerData)
        : base(manager, playerData)
    {
    }

    public override void Enter()
    {
        // Configuración inicial al entrar al estado idle
        playerData.currentSpeed = playerData.walkSpeed;
        idleTime = 0f;
        Debug.Log("Entering Idle State");
    }

    public override void Update()
    {
        // Regenerar stamina
        manager.RegenerateStamina();

        // Si hay movimiento, cambiar a MoveState
        if (manager.IsMoving())
        {
            manager.ChangeState(manager.MoveState);
            return;
        }

        // Actualizar tiempo en idle
        idleTime += Time.deltaTime;

    }

    public override void Exit()
    {
        idleTime = 0f;
        Debug.Log("Exiting Idle State");
    }
}