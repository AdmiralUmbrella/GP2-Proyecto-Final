using UnityEngine;

// Estado: Arada
public class FarmPlotPlowedState : FarmPlotBaseState
{
    public FarmPlotPlowedState(FarmPlotStateManager manager, FarmPlotData plotData)
        : base(manager, plotData) { }

    public override void Enter()
    {
        plotData.plotRenderer.material = plotData.plowedMaterial;
        Debug.Log("Parcela en estado: Arada");
    }

    public override void Exit() { }

    public override void Update() { }

    public override string GetInteractionPrompt(FarmingTool currentTool)
    {
        return currentTool == FarmingTool.Seeds ?
            "Presiona E para plantar semillas" :
            "Necesitas semillas para plantar";
    }

    public override bool CanInteract(FarmingTool currentTool)
    {
        return currentTool == FarmingTool.Seeds;
    }

    public override void OnInteract(FarmingTool currentTool)
    {
        if (currentTool == FarmingTool.Seeds)
        {
            manager.ChangeState(manager.SeededState);
        }
    }
}
