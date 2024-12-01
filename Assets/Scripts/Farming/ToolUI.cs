using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolUI : MonoBehaviour
{
    [SerializeField] private Image toolIcon;
    [SerializeField] private TMP_Text toolName;
    [SerializeField] private PlayerToolManager toolManager;

    private void Start()
    {
        toolManager.OnToolChanged += UpdateToolUI;
        UpdateToolUI(null);
    }

    private void UpdateToolUI(ToolData tool)
    {
        if (tool != null)
        {
            toolIcon.sprite = tool.toolIcon;
            toolName.text = tool.toolName;
            toolIcon.enabled = true;
        }
        else
        {
            toolIcon.enabled = false;
            toolName.text = "Sin herramienta";
        }
    }
}
