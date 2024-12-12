using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // M�todo para iniciar el juego
    public void PlayGame()
    {
        // Reemplaza "GameScene" con el nombre de la escena que quieres cargar
        SceneManager.LoadScene("Day&NightTest");
    }

    // M�todo para salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Para probar en el editor de Unity, puedes usar:
        // #if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying = false;
        // #endif
    }
}
