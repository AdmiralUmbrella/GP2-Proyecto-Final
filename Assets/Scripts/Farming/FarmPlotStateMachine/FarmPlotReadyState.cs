using UnityEngine;

// Estado: Listo para Cosechar 
public class FarmPlotReadyState : FarmPlotBaseState
{
    public FarmPlotReadyState(FarmPlotStateManager manager, FarmPlotData plotData)
        : base(manager, plotData) { }

    public override void Enter()
    {
        Debug.Log("Parcela en estado: Lista para cosechar");
        // Cambiar a visual de cultivo maduro
        if (plotData.plotRenderer != null)
        {
            plotData.plotRenderer.material.color = Color.green;
        }
    }

    public override void Update() { }

    public override void Exit()
    {
        Debug.Log("Cosecha completada");
    }

    public override string GetInteractionPrompt(FarmingTool currentTool)
    {
        return "Presiona E para cosechar";
    }

    public override bool CanInteract(FarmingTool currentTool)
    {
        return true; // Se puede cosechar con cualquier herramienta o manos vacías
    }

    public override void OnInteract(FarmingTool currentTool)
    {
        // Aquí añadiremos la lógica para:
        // 1. Añadir el cultivo al inventario del jugador
        // 2. Reproducir efectos de partículas
        // 3. Reproducir sonido de cosecha

        Debug.Log("¡Cultivo cosechado!");

        // Volver al estado inicial
        manager.ChangeState(manager.UnplowedState);
    }
}
