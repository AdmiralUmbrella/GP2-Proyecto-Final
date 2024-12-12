using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;           // Referencia al Slider de Volumen
    public Dropdown resolutionDropdown;   // Referencia al Dropdown de Resoluci�n

    [Header("Audio Settings")]
    public AudioMixer audioMixer;         // Referencia al Audio Mixer

    private Resolution[] resolutions;     // Array para almacenar las resoluciones disponibles

    void Start()
    {
        // Configurar el Slider de Volumen
        if (volumeSlider != null)
        {
            Debug.Log("Configuring Volume Slider...");
            volumeSlider.onValueChanged.AddListener(SetVolume);
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.75f);
        }
        else
        {
            Debug.LogWarning("VolumeSlider no est� asignado en el Inspector.");
        }

        // Configurar el Dropdown de Resoluci�n
        if (resolutionDropdown != null)
        {
            Debug.Log("Configuring Resolution Dropdown...");
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

            Debug.Log("Resolutions added to Dropdown:");
            foreach (var option in options)
            {
                Debug.Log(option);
            }
        }
        else
        {
            Debug.LogWarning("ResolutionDropdown no est� asignado en el Inspector.");
        }

        // Aplicar configuraciones guardadas
        ApplySettings();
    }

    // M�todo para ajustar el volumen
    public void SetVolume(float volume)
    {
        Debug.Log("SetVolume called with value: " + volume);
        if (audioMixer != null)
        {
            float dB = (volume > 0) ? Mathf.Log10(volume) * 20 : -80f;
            audioMixer.SetFloat("MasterVolume", dB);
            Debug.Log("Volume set to " + dB + " dB");
        }
        else
        {
            Debug.LogWarning("AudioMixer no est� asignado en el Inspector.");
        }

        // Guardar la configuraci�n
        PlayerPrefs.SetFloat("Volume", volume);
    }

    // M�todo para ajustar la resoluci�n
    public void SetResolution(int resolutionIndex)
    {
        Debug.Log("SetResolution called with index: " + resolutionIndex);
        if (resolutions.Length > resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            Debug.Log("Resolution set to: " + resolution.width + "x" + resolution.height);
            // Guardar la configuraci�n
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        }
        else
        {
            Debug.LogWarning("�ndice de resoluci�n fuera de rango.");
        }
    }

    // M�todo para aplicar configuraciones guardadas al iniciar el juego
    public void ApplySettings()
    {
        Debug.Log("Applying saved settings...");
        // Aplicar volumen
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.75f);
        SetVolume(savedVolume);

        // Aplicar resoluci�n
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        SetResolution(savedResolutionIndex);
    }

    // Guardar las preferencias al salir de la aplicaci�n
    void OnApplicationQuit()
    {
        Debug.Log("Saving PlayerPrefs...");
        PlayerPrefs.Save();
    }
}
