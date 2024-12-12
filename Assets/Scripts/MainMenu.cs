using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Método para iniciar el juego
    public void StartGame()
    {
        // Reemplaza "NombreDeTuEscena" con el nombre de la escena que quieres cargar
        Debug.Log("startgame presionado");
    }

    // Método para abrir las opciones
    public void OpenOptions()
    {
        // Aquí puedes implementar la lógica para abrir el menú de opciones
        Debug.Log("Opciones no implementadas aún.");
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Para probar en el editor de Unity, puedes usar:
        // UnityEditor.EditorApplication.isPlaying = false;
    }
}
