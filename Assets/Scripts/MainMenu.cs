using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // M�todo para iniciar el juego
    public void StartGame()
    {
        // Reemplaza "NombreDeTuEscena" con el nombre de la escena que quieres cargar
        Debug.Log("startgame presionado");
    }

    // M�todo para abrir las opciones
    public void OpenOptions()
    {
        // Aqu� puedes implementar la l�gica para abrir el men� de opciones
        Debug.Log("Opciones no implementadas a�n.");
    }

    // M�todo para salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Para probar en el editor de Unity, puedes usar:
        // UnityEditor.EditorApplication.isPlaying = false;
    }
}
