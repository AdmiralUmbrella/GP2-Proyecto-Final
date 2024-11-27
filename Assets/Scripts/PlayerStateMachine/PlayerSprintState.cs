using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateManager manager, PlayerData playerData)
        : base(manager, playerData)
    {
    }

    public override void Enter()
    {
        if (!CanEnterSprint())
        {
            manager.ChangeState(manager.IdleState);
            return;
        }

        playerData.isSprinting = true;
        playerData.currentSpeed = playerData.sprintSpeed;
        Debug.Log("Entering Sprint State");
    }

    public override void Update()
    {
        // Verificar si debemos salir del sprint
        if (ShouldExitSprint())
        {
            manager.ChangeState(manager.IdleState);
            return;
        }

        // Consumir stamina
        ConsumeStamina();
    }

    public override void Exit()
    {
        playerData.isSprinting = false;
        playerData.currentSpeed = playerData.walkSpeed;
        playerData.timeUntilStaminaRegen = playerData.staminaRegenDelay;
        Debug.Log("Exiting Sprint State");
    }

    private bool CanEnterSprint()
    {
        return playerData.currentStamina > 0 && playerData.canSprint;
    }

    private bool ShouldExitSprint()
    {
        return playerData.moveDirection.magnitude < 0.1f ||
               playerData.currentStamina <= 0 ||
               !playerData.canSprint;
    }

    private void ConsumeStamina()
    {
        playerData.currentStamina -= playerData.staminaDrainRate * Time.deltaTime;
        playerData.currentStamina = Mathf.Max(0f, playerData.currentStamina);
    }
}
