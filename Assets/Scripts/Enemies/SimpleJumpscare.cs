using UnityEngine;
using UnityEngine.Events;

public class SimpleJumpscare : MonoBehaviour
{
    [Header("Jumpscare Settings")]
    [SerializeField] private GameObject jumpscarePrefab;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float displayTime = 1.5f;
    [SerializeField] private float spawnDistance = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip jumpscareSound;
    [SerializeField] private float volumeScale = 1f;

    private void Start()
    {
        // Encontrar el SanityManager y registrar nuestro evento
        var sanityManager = FindFirstObjectByType<SanityManager>();
        if (sanityManager != null)
        {
            var threshold = new UnityEngine.Events.UnityEvent();
            threshold.AddListener(ShowJumpscare);
            sanityManager.AddThreshold(60f, "MovingJumpscare", threshold);
        }
    }

    private void ShowJumpscare()
    {
        // Obtener la cámara del jugador
        Camera playerCamera = Camera.main;
        if (playerCamera == null) return;

        // Calcular la posición de aparición frente al jugador
        Vector3 spawnPos = playerCamera.transform.position +
                          playerCamera.transform.forward * spawnDistance;

        // Instanciar el jumpscare
        GameObject jumpscareInstance = Instantiate(
            jumpscarePrefab,
            spawnPos,
            Quaternion.LookRotation(-playerCamera.transform.forward)
        );

        // Configurar el movimiento
        var movement = jumpscareInstance.AddComponent<JumpscareMovement>();
        movement.Initialize(playerCamera.transform, moveSpeed, displayTime);

        // Reproducir sonido
        if (jumpscareSound != null)
        {
            AudioSource.PlayClipAtPoint(
                jumpscareSound,
                spawnPos,
                volumeScale
            );
        }
    }
}