using UnityEngine;

public class FarmPlotStateManager : MonoBehaviour, IInteractable
{
    [SerializeField] private FarmPlotData plotData;
    private PlayerToolManager toolManager; // Nueva referencia cacheada

    // Estados
    public FarmPlotUnplowedState UnplowedState { get; private set; }
    public FarmPlotPlowedState PlowedState { get; private set; }
    public FarmPlotSeededState SeededState { get; private set; }
    public FarmPlotWateredState WateredState { get; private set; }
    public FarmPlotGrowingState GrowingState { get; private set; }
    public FarmPlotReadyState ReadyState { get; private set; }

    private FarmPlotBaseState currentState;

    private void Awake()
    {
        Debug.Log("FarmPlotStateManager Awake");
        // Inicializar estados
        UnplowedState = new FarmPlotUnplowedState(this, plotData);
        PlowedState = new FarmPlotPlowedState(this, plotData);
        SeededState = new FarmPlotSeededState(this, plotData);
        WateredState = new FarmPlotWateredState(this, plotData);
        GrowingState = new FarmPlotGrowingState(this, plotData);
        ReadyState = new FarmPlotReadyState(this, plotData);

        // Comenzar en estado sin arar
        ChangeState(UnplowedState);
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(FarmPlotBaseState newState)
    {
        Debug.Log($"Changing state from {currentState?.GetType().Name} to {newState.GetType().Name}");
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    // Implementación de IInteractable
    public string GetInteractionPrompt()
    {
        if (toolManager == null)
            return "No hay herramientas disponibles";

        return currentState.GetInteractionPrompt(toolManager.CurrentToolType);
    }

    public bool CanInteract(PlayerStateManager player)
    {
        if (toolManager == null)
        {
            toolManager = player.GetComponent<PlayerToolManager>();
            Debug.Log($"Got toolManager: {toolManager != null}, CurrentTool: {toolManager?.CurrentToolType}");
        }

        bool canInteract = currentState.CanInteract(toolManager?.CurrentToolType ?? FarmingTool.None);
        Debug.Log($"CanInteract result: {canInteract} for tool: {toolManager?.CurrentToolType}");
        return canInteract;
    }

    public void OnInteract(PlayerStateManager player)
    {
        if (toolManager == null)
            toolManager = player.GetComponent<PlayerToolManager>();

        currentState.OnInteract(toolManager.CurrentToolType);
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.z > 0)
        {
            var position = new Vector2(screenPos.x, Screen.height - screenPos.y);
            GUI.Label(new Rect(position.x, position.y, 200, 60),
                $"Estado: {currentState?.GetType().Name}\n" +
                $"Tiempo: {plotData.currentGrowthTime:F1}s");
        }
    }
#endif
}
