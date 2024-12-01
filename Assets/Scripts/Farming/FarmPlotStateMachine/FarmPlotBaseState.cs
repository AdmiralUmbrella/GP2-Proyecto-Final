// Estado base para parcelas
public abstract class FarmPlotBaseState
{
    protected FarmPlotStateManager manager;
    protected FarmPlotData plotData;

    public FarmPlotBaseState(FarmPlotStateManager manager, FarmPlotData plotData)
    {
        this.manager = manager;
        this.plotData = plotData;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    public abstract string GetInteractionPrompt(FarmingTool currentTool);
    public abstract bool CanInteract(FarmingTool currentTool);
    public abstract void OnInteract(FarmingTool currentTool);
}
