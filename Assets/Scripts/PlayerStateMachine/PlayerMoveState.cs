using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateManager manager, PlayerData playerData)
        : base(manager, playerData)
    {
    }

    public override void Enter()
    {
        playerData.currentSpeed = playerData.walkSpeed;
        playerData.isSprinting = false;
        Debug.Log("Entering Move State");
    }

    public override void Update()
    {
        manager.RegenerateStamina();

        // Verificar si debemos volver a Idle
        if (!manager.IsMoving())
        {
            manager.ChangeState(manager.IdleState);
            return;
        }

        // Verificar si debemos cambiar a Sprint
        if (playerData.isSprinting && playerData.canSprint && playerData.currentStamina > 0)
        {
            manager.ChangeState(manager.SprintState);
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Move State");
    }
}
