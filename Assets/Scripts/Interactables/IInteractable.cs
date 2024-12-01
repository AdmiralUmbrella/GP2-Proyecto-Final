using UnityEngine;

public interface IInteractable
{
    string GetInteractionPrompt();  // Texto que se mostrará al jugador
    void OnInteract(PlayerStateManager player);  // Método que se ejecuta al interactuar
    bool CanInteract(PlayerStateManager player); // Verificar si se puede interactuar
}
