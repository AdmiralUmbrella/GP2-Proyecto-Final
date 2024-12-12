using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class SanityManager : MonoBehaviour
{
    [Header("Sanity Settings")]
    [SerializeField] private float maxSanity = 100f;
    public float currentSanity;
    [SerializeField] private float baseDecayRate = 1f;  // Pérdida base de cordura por segundo
    [SerializeField] private DayNightCycle dayNightCycle = new DayNightCycle();

    [Header("Threshold Settings")]
    [SerializeField] private List<SanityThreshold> sanityThresholds = new List<SanityThreshold>();

    [Header("Effect Settings")]
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private AudioSource heartbeatSource;
    [SerializeField] private float maxVignetteIntensity = 0.5f;

    private Vignette vignetteEffect;
    private ChromaticAberration chromaticAberration;
    private PlayerStateManager playerStateManager;

    private void Awake()
    {
        currentSanity = maxSanity;
        playerStateManager = GetComponent<PlayerStateManager>();

        // Obtener efectos de post-procesado
        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out vignetteEffect))
        {
            vignetteEffect.intensity.value = 0f;
        }
        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out chromaticAberration))
        {
            chromaticAberration.intensity.value = 0f;
        }
    }

    private void Update()
    {
        UpdateSanity();
        CheckThresholds();
        UpdateEffects();
    }

    private void UpdateSanity()
    {
        if (dayNightCycle.currentTimeInHours >= 19f)
        {
            // Aplicar decay base
            ModifySanity(-baseDecayRate * Time.deltaTime);
        }
    }

    public void ModifySanity(float amount)
    {
        float previousSanity = currentSanity;
        currentSanity = Mathf.Clamp(currentSanity + amount, 0f, maxSanity);

        // Notificar cambios significativos
        if (Mathf.Abs(previousSanity - currentSanity) > 1f)
        {
            OnSanityChanged();
        }
    }

    private void CheckThresholds()
    {
        foreach (var threshold in sanityThresholds)
        {
            bool shouldBeActive = currentSanity <= threshold.threshold;

            // Si entramos en el threshold
            if (shouldBeActive && !threshold.isActive)
            {
                threshold.isActive = true;
                threshold.onEnter?.Invoke();
                Debug.Log($"Entered {threshold.effectName} threshold at {threshold.threshold} sanity");
            }
            // Si salimos del threshold
            else if (!shouldBeActive && threshold.isActive)
            {
                threshold.isActive = false;
                threshold.onExit?.Invoke();
                Debug.Log($"Exited {threshold.effectName} threshold");
            }
            // Si estamos dentro del threshold
            else if (threshold.isActive)
            {
                threshold.whileActive?.Invoke();
            }
        }
    }

    private void UpdateEffects()
    {
        // Calcular intensidad base de efectos
        float sanityPercentage = currentSanity / maxSanity;
        float effectIntensity = 1f - sanityPercentage;

        // Actualizar efectos visuales
        if (vignetteEffect != null)
        {
            vignetteEffect.intensity.value = effectIntensity * maxVignetteIntensity;
        }

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = effectIntensity * 0.5f;
        }

        // Actualizar efectos de audio
        if (heartbeatSource != null)
        {
            heartbeatSource.volume = Mathf.Lerp(0f, 1f, effectIntensity);
            heartbeatSource.pitch = Mathf.Lerp(1f, 1.5f, effectIntensity);
        }
    }

    private void OnSanityChanged()
    {
        // Aquí puedes añadir cualquier lógica adicional cuando la cordura cambia significativamente
    }

    // Métodos públicos para interactuar con el sistema
    public float GetSanityPercentage() => currentSanity / maxSanity;

    public bool IsInThreshold(string thresholdName)
    {
        var threshold = sanityThresholds.Find(t => t.effectName == thresholdName);
        return threshold != null && threshold.isActive;
    }

    // Método para añadir nuevo threshold en runtime
    public void AddThreshold(float threshold, string name, UnityEvent onEnter = null,
                           UnityEvent onExit = null, UnityEvent whileActive = null)
    {
        sanityThresholds.Add(new SanityThreshold
        {
            threshold = threshold,
            effectName = name,
            onEnter = onEnter ?? new UnityEvent(),
            onExit = onExit ?? new UnityEvent(),
            whileActive = whileActive ?? new UnityEvent()
        });

        // Mantener los thresholds ordenados de mayor a menor
        sanityThresholds.Sort((a, b) => b.threshold.CompareTo(a.threshold));
    }
}
