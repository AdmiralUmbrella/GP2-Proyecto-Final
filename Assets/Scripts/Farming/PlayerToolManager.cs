using UnityEngine;

public class PlayerToolManager : MonoBehaviour
{
    [Header("Tool Settings")]
    [SerializeField] private Transform toolHolder;          // Punto donde se sostiene la herramienta
    [SerializeField] private float dropForce = 5f;         // Fuerza al soltar herramienta

    private FarmingTool currentToolType = FarmingTool.None;
    private GameObject currentToolObject;
    private ToolData currentToolData;

    public FarmingTool CurrentToolType => currentToolType;
    public ToolData CurrentToolData => currentToolData;

    // Evento para notificar cambios de herramienta
    public event System.Action<ToolData> OnToolChanged;

    public void EquipTool(ToolData newTool)
    {
        // Si ya tenemos una herramienta, la soltamos
        if (currentToolObject != null)
        {
            DropCurrentTool();
        }

        // Equipar nueva herramienta
        currentToolType = newTool.toolType;
        currentToolData = newTool;

        // Instanciar el modelo 3D de la herramienta
        currentToolObject = Instantiate(newTool.toolPrefab, toolHolder);
        currentToolObject.transform.localPosition = Vector3.zero;
        currentToolObject.transform.localRotation = Quaternion.identity;

        // Notificar el cambio
        OnToolChanged?.Invoke(newTool);
    }

    public void DropCurrentTool()
    {
        if (currentToolObject != null)
        {
            // Desconectar de toolHolder
            currentToolObject.transform.parent = null;

            // Agregar RigidBody para física
            Rigidbody rb = currentToolObject.AddComponent<Rigidbody>();

            // Agregar fuerza hacia adelante
            rb.AddForce(transform.forward * dropForce, ForceMode.Impulse);

            // Agregar componente para poder recogerlo de nuevo
            var pickup = currentToolObject.AddComponent<ToolPickup>();
            pickup.SetToolData(currentToolData);

            // Limpiar referencias
            currentToolObject = null;
            currentToolType = FarmingTool.None;
            currentToolData = null;

            // Notificar el cambio
            OnToolChanged?.Invoke(null);
        }
    }
}