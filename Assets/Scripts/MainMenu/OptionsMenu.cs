using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Dropdown resolutionDropdown;

    [Header("Audio Settings")]
    public AudioMixer audioMixer;

    private Resolution[] resolutions;

    void Start()
    {
        // Configurar el Slider de Volumen
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            // Cargar el volumen guardado o por defecto
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.75f);
        }

        // Configurar el Dropdown de Resolución
        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
            resolutionDropdown.RefreshShownValue();
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        // Aplicar configuraciones guardadas
        ApplySettings();
    }

    // Método para ajustar el volumen
    public void SetVolume(float volume)
    {
        // Asumiendo que tienes un AudioMixer con un parámetro llamado "MasterVolume"
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
        // Guardar la configuración
        PlayerPrefs.SetFloat("Volume", volume);
    }

    // Método para ajustar la resolución
    public void SetResolution(int resolutionIndex)
    {
        if (resolutions.Length > resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            // Guardar la configuración
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        }
    }

    // Método para aplicar configuraciones guardadas
    public void ApplySettings()
    {
        // Aplicar volumen
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.75f);
        SetVolume(savedVolume);

        // Aplicar resolución
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        SetResolution(savedResolutionIndex);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
