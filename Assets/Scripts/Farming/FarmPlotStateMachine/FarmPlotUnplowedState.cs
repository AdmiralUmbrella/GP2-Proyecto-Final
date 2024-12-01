using UnityEngine;

// Estado: Sin arar
public class FarmPlotUnplowedState : FarmPlotBaseState
{
    public FarmPlotUnplowedState(FarmPlotStateManager manager, FarmPlotData plotData)
        : base(manager, plotData) { }

    public override void Enter()
    {
        plotData.plotRenderer.material = plotData.unplowedMaterial;
        Debug.Log("Parcela en estado: Sin arar");
    }

    public override void Exit() { }

    public override void Update() { }

    public override string GetInteractionPrompt(FarmingTool currentTool)
    {
        return currentTool == FarmingTool.Shovel ?
            "Presiona E para arar la tierra" :
            "Necesitas una pala para arar";
    }

    public override bool CanInteract(FarmingTool currentTool)
    {
        Debug.Log($"UnplowedState CanInteract check for tool: {currentTool}");
        return currentTool == FarmingTool.Shovel;
    }

    public override void OnInteract(FarmingTool currentTool)
    {
        if (currentTool == FarmingTool.Shovel)
        {
            manager.ChangeState(manager.PlowedState);
        }
    }
}
