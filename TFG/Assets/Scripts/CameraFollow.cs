using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset; // Distancia entre la cámara y el personaje
    public float smoothTime = 0.3f; // Tiempo de suavizado

    public float rangoXMin = -0.55f; // Límite izquierdo del rango en el eje X
    public float rangoXMax = 0.5f; // Límite derecho del rango en el eje X

    public float rangoZMin = -0.55f; // Límite izquierdo del rango en el eje Z
    public float rangoZMax = 0.5f; // Límite derecho del rango en el eje Z

    private Vector3 velocity = Vector3.zero; // Velocidad utilizada por SmoothDamp

    // LateUpdate is called once per frame, after all Update calls
    void LateUpdate()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Calcula la posición deseada de la cámara (en los ejes X y Z)
        float xPosition = Mathf.Clamp(player.position.x + offset.x, rangoXMin, rangoXMax);
        float zPosition = transform.position.z;

        if (sceneName == "Corridor")
        {
            zPosition = Mathf.Clamp(player.position.z + offset.z, rangoZMin, rangoZMax);
        }

        Vector3 desiredPosition = new Vector3(xPosition, transform.position.y, zPosition);

        // Interpola suavemente la posición actual de la cámara hacia la posición deseada usando SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
