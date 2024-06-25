using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePosition : MonoBehaviour
{
    public Transform player;
    public float pX;
    public float pY;
    public float pZ;

    public string previous;

    IEnumerator Start()
    {
        // Esperar hasta que prevScene se inicialice correctamente
        yield return new WaitUntil(() => !string.IsNullOrEmpty(GetObject.prevScene));

        // Debug log para ver los valores iniciales y la escena previa
        Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name);
        Debug.Log("Previous Scene: " + GetObject.prevScene);
        Debug.Log("Expected Previous Scene: " + previous);

        // Verificar si la escena previa coincide con la escena esperada
        if (GetObject.prevScene == previous)
        {
            Debug.Log("Expected Position: (" + pX + ", " + pY + ", " + pZ + ")");
            player.position = new Vector3(pX, pY, pZ);
            Debug.Log("Player position set to: " + player.position);
        }
        else
        {
            Debug.Log("Previous scene does not match. Player position not changed.");
        }
    }
}
