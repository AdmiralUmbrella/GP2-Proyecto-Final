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
        RegenerateStamina();

        // Verificar transición a sprint
        if (manager.IsMoving() && playerData.isSprinting && playerData.canSprint && playerData.currentStamina > 0)
        {
            manager.ChangeState(manager.SprintState);
            return;
        }

        // Si el jugador está quieto, incrementar el tiempo en idle
        if (!manager.IsMoving())
        {
            idleTime += Time.deltaTime;
            // Aquí podrías añadir lógica especial después de cierto tiempo en idle
            // Por ejemplo, para tu juego de terror, podrías aumentar la probabilidad
            // de eventos paranormales después de estar quieto mucho tiempo
        }
        else
        {
            idleTime = 0f;
        }
    }

    public override void Exit()
    {
        idleTime = 0f;
        Debug.Log("Exiting Idle State");
    }

    private void RegenerateStamina()
    {
        if (playerData.timeUntilStaminaRegen > 0)
        {
            playerData.timeUntilStaminaRegen -= Time.deltaTime;
            return;
        }

        if (playerData.currentStamina < playerData.maxStamina)
        {
            playerData.currentStamina += playerData.staminaRegenRate * Time.deltaTime;
            playerData.currentStamina = Mathf.Min(playerData.currentStamina, playerData.maxStamina);
        }
    }
}