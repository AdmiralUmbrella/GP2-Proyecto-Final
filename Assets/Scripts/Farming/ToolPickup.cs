using UnityEngine;

public class ToolPickup : MonoBehaviour, IInteractable
{
    public ToolData toolData;

    public void SetToolData(ToolData data)
    {
        toolData = data;
    }

    public string GetInteractionPrompt()
    {
        return $"Presiona E para recoger {toolData.toolName}";
    }

    public bool CanInteract(PlayerStateManager player)
    {
        return true; // Siempre se puede intentar recoger una herramienta
    }

    public void OnInteract(PlayerStateManager player)
    {
        var toolManager = player.GetComponent<PlayerToolManager>();
        if (toolManager != null)
        {
            toolManager.EquipTool(toolData);
            // Destruir este pickup
            Destroy(gameObject);
        }
    }
}
