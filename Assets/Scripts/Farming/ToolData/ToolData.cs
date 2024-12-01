using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "Farming/Tool Data")]
public class ToolData : ScriptableObject
{
    public string toolName;
    public FarmingTool toolType;
    public GameObject toolPrefab;     // Modelo 3D de la herramienta
    public Sprite toolIcon;           // Ícono para UI
    [TextArea]
    public string toolDescription;    // Descripción para UI
}
