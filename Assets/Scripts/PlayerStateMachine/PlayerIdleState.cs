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
        // Configuraci�n inicial al entrar al estado idle
        playerData.currentSpeed = playerData.walkSpeed;
        idleTime = 0f;
        Debug.Log("Entering Idle State");
    }

    public override void Update()
    {
        // Regenerar stamina
        RegenerateStamina();

        // Verificar transici�n a sprint
        if (manager.IsMoving() && playerData.isSprinting && playerData.canSprint && playerData.currentStamina > 0)
        {
            manager.ChangeState(manager.SprintState);
            return;
        }

        // Si el jugador est� quieto, incrementar el tiempo en idle
        if (!manager.IsMoving())
        {
            idleTime += Time.deltaTime;
            // Aqu� podr�as a�adir l�gica especial despu�s de cierto tiempo en idle
            // Por ejemplo, para tu juego de terror, podr�as aumentar la probabilidad
            // de eventos paranormales despu�s de estar quieto mucho tiempo
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