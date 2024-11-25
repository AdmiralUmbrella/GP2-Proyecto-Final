public abstract class PlayerBaseState
{
    protected PlayerStateManager manager; // Referencia al manager
    protected PlayerData playerData;      // Referencia a los datos

    // Constructor que recibe las referencias
    public PlayerBaseState(PlayerStateManager manager, PlayerData playerData)
    {
        this.manager = manager;
        this.playerData = playerData;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}