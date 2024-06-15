using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset; // Distancia entre la cámara y el personaje
    public float cameraSpeed = 10f;

    public float rangoXMin = -0.55f; // Límite izquierdo del rango en el eje X
    public float rangoXMax = 0.5f; // Límite derecho del rango en el eje X

    // Update is called once per frame
    void LateUpdate()
    {
        // Calcula la posición deseada de la cámara (solo en el eje X)
        float xPosition = Mathf.Clamp(player.position.x + offset.x, rangoXMin, rangoXMax);
        Vector3 desiredPosition = new Vector3(xPosition, transform.position.y, transform.position.z);

        // Interpola suavemente la posición actual de la cámara hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed * Time.deltaTime);
    }
}