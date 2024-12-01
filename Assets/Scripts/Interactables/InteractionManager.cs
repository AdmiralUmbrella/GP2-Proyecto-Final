using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform cameraTransform;

    [Header("Debug Settings")]
    [SerializeField] private bool showDebug = true;
    [SerializeField] private Color rayColor = Color.green;
    [SerializeField] private Color hitColor = Color.red;

    private IInteractable currentInteractable;
    private PlayerStateManager playerManager;
    private RaycastHit lastHit;
    private bool hasHit;

    private void Awake()
    {
        playerManager = GetComponent<PlayerStateManager>();

        if (cameraTransform == null)
        {
            Debug.LogError("InteractionManager: No camera transform assigned!");
        }
    }

    private void Update()
    {
        CheckForInteractable();
        UpdateUI();

        // Debug logging
        if (showDebug)
        {
            if (currentInteractable != null)
            {
                Debug.Log($"Current Interactable: {currentInteractable.GetType().Name} - Prompt: {currentInteractable.GetInteractionPrompt()}");
            }
        }
    }

    private void CheckForInteractable()
    {
        if (cameraTransform == null) return;

        // Raycast desde la cámara
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        hasHit = Physics.Raycast(ray, out lastHit, interactionRange, interactableLayer);

        if (hasHit)
        {
            // Intentar obtener un componente que implemente IInteractable
            IInteractable interactable = lastHit.collider.GetComponent<IInteractable>();

            if (interactable != null && interactable.CanInteract(playerManager))
            {
                currentInteractable = interactable;
                if (showDebug)
                {
                    Debug.Log($"Found interactable: {lastHit.collider.name} at distance {lastHit.distance}");
                }
                return;
            }
        }

        currentInteractable = null;
    }

    private void OnDrawGizmos()
    {
        if (!showDebug || cameraTransform == null) return;

        // Dibujar el rayo de interacción
        Gizmos.color = rayColor;
        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * interactionRange);

        // Si hay un hit, mostrar el punto de impacto
        if (hasHit)
        {
            Gizmos.color = hitColor;
            Gizmos.DrawWireSphere(lastHit.point, 0.1f);
            Gizmos.DrawLine(lastHit.point, lastHit.point + lastHit.normal * 0.5f);
        }
    }

    private void UpdateUI()
    {
        if (currentInteractable != null)
        {
            // Aquí actualizarías tu UI con currentInteractable.GetInteractionPrompt()
            Debug.Log($"Interaction available: {currentInteractable.GetInteractionPrompt()}");
        }
    }

    // Llamado desde el Input System cuando se presiona la tecla de interacción
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Solo ejecutar cuando el botón es presionado, no cuando se mantiene o se suelta
        if (!context.performed) return;

        if (currentInteractable != null && currentInteractable.CanInteract(playerManager))
        {
            Debug.Log("Interacting with object");
            currentInteractable.OnInteract(playerManager);
        }
        else
        {
            var toolManager = playerManager.GetComponent<PlayerToolManager>();
            if (toolManager != null && toolManager.CurrentToolType != FarmingTool.None)
            {
                Debug.Log("Dropping tool");
                toolManager.DropCurrentTool();
            }
        }
    }

#if UNITY_EDITOR
    // Debug GUI en la ventana de juego
    private void OnGUI()
    {
        if (!showDebug) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 100));
        GUILayout.Label($"Interaction Range: {interactionRange}");
        GUILayout.Label($"Has Hit: {hasHit}");
        if (hasHit)
        {
            GUILayout.Label($"Hit Object: {lastHit.collider.name}");
            GUILayout.Label($"Hit Distance: {lastHit.distance:F2}");
        }
        if (currentInteractable != null)
        {
            GUILayout.Label($"Current Interactable: {currentInteractable.GetType().Name}");
        }
        GUILayout.EndArea();
    }
#endif
}