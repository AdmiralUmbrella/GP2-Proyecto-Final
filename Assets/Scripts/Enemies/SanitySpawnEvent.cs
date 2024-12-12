using UnityEngine;
using UnityEngine.Events;

public class SanitySpawnEvent : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float sanityThreshold = 30f;
    [SerializeField] private bool canSpawnMultipleTimes = false;

    [Header("Optional Settings")]
    [SerializeField] private float minTimeBetweenSpawns = 10f;
    [SerializeField] private bool randomizeSpawnPoint = true;

    private bool hasSpawned = false;
    private float lastSpawnTime = -999f;

    private void Start()
    {
        // Validaciones iniciales
        if (prefabToSpawn == null)
        {
            Debug.LogError("SanitySpawnEvent: No se ha asignado un prefab para spawner!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SanitySpawnEvent: No se han asignado puntos de spawn!");
            return;
        }

        // Encontrar el SanityManager y registrar nuestro evento
        var sanityManager = FindFirstObjectByType<SanityManager>();
        if (sanityManager != null)
        {
            var spawnEvent = new UnityEvent();
            spawnEvent.AddListener(HandleSpawnEvent);

            // Registrar el threshold en el SanityManager
            sanityManager.AddThreshold(
                sanityThreshold,
                "SpawnEvent",
                spawnEvent
            );
        }
        else
        {
            Debug.LogError("SanitySpawnEvent: No se encontró el SanityManager en la escena!");
        }
    }

    private void HandleSpawnEvent()
    {
        // Verificar si podemos spawnear
        if (!CanSpawn()) return;

        // Seleccionar punto de spawn
        Transform selectedSpawnPoint = SelectSpawnPoint();

        // Instanciar el prefab
        GameObject spawnedObject = Instantiate(
            prefabToSpawn,
            selectedSpawnPoint.position,
            selectedSpawnPoint.rotation
        );

        // Actualizar estado
        hasSpawned = true;
        lastSpawnTime = Time.time;

        Debug.Log($"SanitySpawnEvent: Objeto spawneado en {selectedSpawnPoint.name}");
    }

    private bool CanSpawn()
    {
        // Si ya spawneamos y no permitimos múltiples spawns, retornar false
        if (hasSpawned && !canSpawnMultipleTimes) return false;

        // Verificar el tiempo mínimo entre spawns
        if (Time.time - lastSpawnTime < minTimeBetweenSpawns) return false;

        return true;
    }

    private Transform SelectSpawnPoint()
    {
        if (!randomizeSpawnPoint)
            return spawnPoints[0];

        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    // Método público para resetear el estado de spawn si es necesario
    public void ResetSpawnState()
    {
        hasSpawned = false;
        lastSpawnTime = -999f;
    }
}