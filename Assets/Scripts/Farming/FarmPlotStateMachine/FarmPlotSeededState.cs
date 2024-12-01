using UnityEngine;

// Estado: Con semillas
public class FarmPlotSeededState : FarmPlotBaseState
{
    public FarmPlotSeededState(FarmPlotStateManager manager, FarmPlotData plotData)
        : base(manager, plotData) { }

    public override void Enter()
    {
        plotData.plotRenderer.material = plotData.seededMaterial;
        Debug.Log("Parcela en estado: Sembrada");
    }

    public override void Exit() { }

    public override void Update() { }

    public override string GetInteractionPrompt(FarmingTool currentTool)
    {
        return currentTool == FarmingTool.Watering ?
            "Presiona E para regar" :
            "Necesitas la regadera para regar";
    }

    public override bool CanInteract(FarmingTool currentTool)
    {
        return currentTool == FarmingTool.Watering;
    }

    public override void OnInteract(FarmingTool currentTool)
    {
        if (currentTool == FarmingTool.Watering)
        {
            manager.ChangeState(manager.WateredState);
        }
    }
}
