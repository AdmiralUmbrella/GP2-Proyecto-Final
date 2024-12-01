using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SanityThreshold
{
    public float threshold;         // Nivel de cordura en el que se activa
    public bool isActive;          // Si el threshold está actualmente activo
    public string effectName;      // Nombre descriptivo del efecto
    public UnityEvent onEnter;     // Evento cuando entramos en este threshold
    public UnityEvent onExit;      // Evento cuando salimos de este threshold
    public UnityEvent whileActive; // Evento que se ejecuta mientras estamos en este threshold
}
