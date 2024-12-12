using UnityEngine;
using System;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float dayLengthInMinutes = 24f;    // Duración de un día completo en minutos reales
    [SerializeField] private float startingHour = 6f;           // Hora del día en que inicia el juego

    [Header("Sun Settings")]
    [SerializeField] private Light directionalLight;            // La luz principal que representa el sol
    [SerializeField] private float maxSunIntensity = 1f;       // Intensidad máxima de la luz solar
    [SerializeField] private float minSunIntensity = 0f;       // Intensidad mínima de la luz solar

    [Header("Ambient Light Settings")]
    [SerializeField] private Color dayAmbientColor = Color.gray;
    [SerializeField] private Color nightAmbientColor = Color.black;

    // Variables privadas para el tracking del tiempo
    public float currentTimeInHours;                          // La hora actual del día (0-24)
    private float timeMultiplier;                              // Multiplicador calculado basado en dayLengthInMinutes

    // Propiedades públicas para otros sistemas
    public float CurrentHour => currentTimeInHours;
    public bool IsNight => currentTimeInHours > 18f || currentTimeInHours < 6f;

    // Eventos para notificar cambios importantes en el ciclo
    public event Action OnDayStart;
    public event Action OnNightStart;
    public event Action<float> OnTimeChanged;      // Envía la hora actual

    private void Awake()
    {
        // Calcular el multiplicador de tiempo basado en la duración deseada del día
        timeMultiplier = 24f / (dayLengthInMinutes * 60f);
        currentTimeInHours = startingHour;

        if (directionalLight == null)
        {
            Debug.LogError("DayNightCycle: No se ha asignado una luz direccional!");
        }
    }

    private void Update()
    {
        UpdateTime();
        UpdateLighting();
        CheckForTimeEvents();
    }

    private void UpdateTime()
    {
        // Actualizar el tiempo actual
        float previousHour = currentTimeInHours;
        currentTimeInHours += Time.deltaTime * timeMultiplier;

        // Mantener el tiempo entre 0-24
        if (currentTimeInHours >= 24f)
        {
            currentTimeInHours -= 24f;
        }

        // Notificar el cambio de tiempo
        if (Mathf.Floor(previousHour) != Mathf.Floor(currentTimeInHours))
        {
            OnTimeChanged?.Invoke(currentTimeInHours);
        }
    }

    private void UpdateLighting()
    {
        if (directionalLight == null) return;

        // Calcular la rotación del sol basada en la hora del día
        float sunRotation = (currentTimeInHours - 6f) * 15f; // 15 grados por hora
        directionalLight.transform.rotation = Quaternion.Euler(sunRotation, 170f, 0f);

        // Calcular la intensidad de la luz basada en la hora
        float intensityMultiplier = 1f;

        // Atardecer (18:00 - 19:00)
        if (currentTimeInHours >= 18f && currentTimeInHours <= 19f)
        {
            intensityMultiplier = 1f - (currentTimeInHours - 18f);
        }
        // Amanecer (5:00 - 6:00)
        else if (currentTimeInHours >= 5f && currentTimeInHours <= 6f)
        {
            intensityMultiplier = (currentTimeInHours - 5f);
        }
        // Noche (19:00 - 5:00)
        else if (currentTimeInHours >= 19f || currentTimeInHours <= 5f)
        {
            intensityMultiplier = 0f;
        }

        // Aplicar la intensidad
        directionalLight.intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, intensityMultiplier);

        // Actualizar la luz ambiental
        RenderSettings.ambientLight = Color.Lerp(nightAmbientColor, dayAmbientColor, intensityMultiplier);
    }

    private void CheckForTimeEvents()
    {
        // Verificar inicio del día (6:00)
        if (currentTimeInHours >= 6f && currentTimeInHours < 6f + timeMultiplier)
        {
            OnDayStart?.Invoke();
        }
        // Verificar inicio de la noche (18:00)
        else if (currentTimeInHours >= 18f && currentTimeInHours < 18f + timeMultiplier)
        {
            OnNightStart?.Invoke();
        }
    }

    // Métodos públicos para control del tiempo
    public void SetTime(float hour)
    {
        currentTimeInHours = Mathf.Clamp(hour, 0f, 24f);
        UpdateLighting();
    }

    public void ModifyTimeScale(float multiplier)
    {
        timeMultiplier = (24f / (dayLengthInMinutes * 60f)) * multiplier;
    }
}
