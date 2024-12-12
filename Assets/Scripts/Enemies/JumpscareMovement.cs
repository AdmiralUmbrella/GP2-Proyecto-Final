using UnityEngine;

// Este componente se encargará del movimiento del jumpscare
public class JumpscareMovement : MonoBehaviour
{
    private Transform targetTransform;
    private float moveSpeed = 5f;
    private float destroyTime = 1.5f;
    private float initialDistance;
    private Vector3 startPosition;
    private float elapsedTime = 0f;

    public void Initialize(Transform target, float speed, float lifetime)
    {
        targetTransform = target;
        moveSpeed = speed;
        destroyTime = lifetime;
        startPosition = transform.position;
        initialDistance = Vector3.Distance(startPosition, target.position);

        // Asegurarnos de que el objeto mire al jugador desde el inicio
        transform.LookAt(target);
    }

    private void Update()
    {
        if (targetTransform == null) return;

        // Incrementar el tiempo transcurrido
        elapsedTime += Time.deltaTime * moveSpeed;

        // Calcular la posición usando Lerp
        float percentComplete = elapsedTime / moveSpeed;
        transform.position = Vector3.Lerp(
            startPosition,
            targetTransform.position + (targetTransform.forward * 0.5f), // Detenerse medio metro frente al jugador
            percentComplete
        );

        // Mantener el objeto mirando al jugador
        transform.LookAt(targetTransform);

        // Destruir el objeto después del tiempo especificado
        if (percentComplete >= 1.0f)
        {
            Destroy(gameObject, destroyTime);
            // Desactivar este script una vez que llegamos al destino
            enabled = false;
        }
    }
}
