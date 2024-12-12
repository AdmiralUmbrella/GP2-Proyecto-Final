using UnityEngine;

[RequireComponent(typeof(SanityManager))]
public class NightSanityManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float sanityStartHour = 18f;    // Hora a la que comienza a disminuir la cordura
    [SerializeField] private float sanityEndHour = 6f;       // Hora a la que deja de disminuir la cordura

    [Header("Sanity Decay")]
    [SerializeField] private float nightSanityDecayMultiplier = 2f;  // Multiplicador de pérdida de cordura durante la noche
    [SerializeField] private AnimationCurve decayIntensityCurve = AnimationCurve.Linear(0f, 1f, 1f, 2f); // Curva de intensidad

    private SanityManager sanityManager;
    private DayNightCycle dayNightCycle;
    private bool isInDangerousHours = false;

    private void Awake()
    {
        sanityManager = GetComponent<SanityManager>();
        dayNightCycle = FindFirstObjectByType<DayNightCycle>();

        if (dayNightCycle == null)
        {
            Debug.LogError("NightSanityManager: No se encontró el DayNightCycle en la escena!");
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        if (dayNightCycle != null)
        {
            dayNightCycle.OnTimeChanged += CheckDangerousHours;
        }
    }

    private void OnDisable()
    {
        if (dayNightCycle != null)
        {
            dayNightCycle.OnTimeChanged -= CheckDangerousHours;
        }
    }

    private void Update()
    {
        if (isInDangerousHours)
        {
            ApplyNightSanityDecay();
        }
    }

    private void CheckDangerousHours(float currentHour)
    {
        // Consideramos "horas peligrosas" desde sanityStartHour hasta sanityEndHour del día siguiente
        bool isDangerous = false;

        if (sanityStartHour < sanityEndHour)
        {
            // Caso simple: el período está dentro del mismo día
            isDangerous = currentHour >= sanityStartHour && currentHour < sanityEndHour;
        }
        else
        {
            // Caso donde el período cruza la medianoche
            isDangerous = currentHour >= sanityStartHour || currentHour < sanityEndHour;
        }

        if (isDangerous != isInDangerousHours)
        {
            isInDangerousHours = isDangerous;
            Debug.Log($"Estado de horas peligrosas cambiado a: {isDangerous} a las {currentHour:F1}");
        }
    }

    private void ApplyNightSanityDecay()
    {
        float currentHour = dayNightCycle.CurrentHour;
        float normalizedTime;

        // Calcular el tiempo normalizado para la curva de intensidad
        if (currentHour >= sanityStartHour)
        {
            normalizedTime = (currentHour - sanityStartHour) / (24f - sanityStartHour + sanityEndHour);
        }
        else
        {
            normalizedTime = (currentHour + 24f - sanityStartHour) / (24f - sanityStartHour + sanityEndHour);
        }

        // Obtener el multiplicador de la curva de intensidad
        float intensityMultiplier = decayIntensityCurve.Evaluate(normalizedTime);

        // Aplicar la pérdida de cordura
        float decayAmount = -nightSanityDecayMultiplier * intensityMultiplier * Time.deltaTime;
        sanityManager.ModifySanity(decayAmount);
    }
}