using UnityEngine;

// Datos compartidos de la parcela
[System.Serializable]
public class FarmPlotData
{
    [Header("Visual Settings")]
    public MeshRenderer plotRenderer;
    public Material unplowedMaterial;
    public Material plowedMaterial;
    public Material seededMaterial;
    public Material wateredMaterial;
    public GameObject[] growthStagePrefabs;  // Modelos para diferentes etapas

    [Header("Growth Settings")]
    public float growthTime = 60f;           // Tiempo total para crecer
    public float currentGrowthTime;          // Tiempo actual de crecimiento
    public int growthStages = 4;             // Número de etapas de crecimiento

    [Header("Effects")]
    public ParticleSystem harvestParticles;
    public AudioClip harvestSound;
}
