using UnityEngine;

// Estado: Creciendo
public class FarmPlotGrowingState : FarmPlotBaseState
{
    private float growthProgress = 0f;

    public FarmPlotGrowingState(FarmPlotStateManager manager, FarmPlotData plotData)
        : base(manager, plotData) { }

    public override void Enter()
    {
        growthProgress = plotData.currentGrowthTime / plotData.growthTime;
        Debug.Log($"Parcela en estado: Creciendo - Progreso inicial: {growthProgress:P2}");
        UpdateVisuals();
    }

    public override void Update()
    {
        // Actualizar tiempo de crecimiento
        plotData.currentGrowthTime += Time.deltaTime;

        // Calcular nuevo progreso
        float newProgress = plotData.currentGrowthTime / plotData.growthTime;

        // Si el progreso ha cambiado significativamente, actualizar visuales
        if (Mathf.Abs(newProgress - growthProgress) > 0.1f)
        {
            growthProgress = newProgress;
            UpdateVisuals();
            Debug.Log($"Progreso de crecimiento: {growthProgress:P2}");
        }

        // Verificar si está listo para cosechar
        if (plotData.currentGrowthTime >= plotData.growthTime)
        {
            manager.ChangeState(manager.ReadyState);
        }
    }

    private void UpdateVisuals()
    {
        // Aquí puedes actualizar el modelo 3D del cultivo según el progreso
        // Por ejemplo, cambiar su escala o swapear diferentes meshes

        // Por ahora solo cambiamos el color para demostración
        if (plotData.plotRenderer != null)
        {
            Color currentColor = plotData.wateredMaterial.color;
            Color growthColor = Color.Lerp(currentColor, Color.green, growthProgress);
            plotData.plotRenderer.material.color = growthColor;
        }
    }

    public override void Exit()
    {
        Debug.Log("Saliendo del estado de crecimiento");
    }

    public override string GetInteractionPrompt(FarmingTool currentTool)
    {
        return $"Creciendo... {growthProgress:P2}";
    }

    public override bool CanInteract(FarmingTool currentTool)
    {
        return false; // No se puede interactuar mientras crece
    }

    public override void OnInteract(FarmingTool currentTool) { }
}

