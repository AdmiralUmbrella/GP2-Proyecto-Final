using UnityEngine;

// Estado: Regada
public class FarmPlotWateredState : FarmPlotBaseState
{
    public FarmPlotWateredState(FarmPlotStateManager manager, FarmPlotData plotData)
        : base(manager, plotData) { }

    public override void Enter()
    {
        plotData.plotRenderer.material = plotData.wateredMaterial;
        plotData.currentGrowthTime = 0f;
        Debug.Log("Parcela en estado: Regada");
        // Iniciar crecimiento después de un frame
        manager.StartCoroutine(StartGrowthNextFrame());
    }

    private System.Collections.IEnumerator StartGrowthNextFrame()
    {
        yield return null;
        manager.ChangeState(manager.GrowingState);
    }

    public override void Exit() { }
    public override void Update() { }
    public override string GetInteractionPrompt(FarmingTool currentTool) => "Iniciando crecimiento...";
    public override bool CanInteract(FarmingTool currentTool) => false;
    public override void OnInteract(FarmingTool currentTool) { }
}
