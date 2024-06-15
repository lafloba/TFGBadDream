using UnityEngine;

public class ActivateEnemy : MonoBehaviour
{
    public GameObject enemyToActivate; // Referencia al GameObject del enemigo

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el jugador ha colisionado con el objeto vacío
        {
            // Activa el GameObject del enemigo
            if (enemyToActivate != null)
            {
                enemyToActivate.SetActive(true);
            }

            // Desactiva el objeto vacío para que no pueda activar al enemigo nuevamente
            gameObject.SetActive(false);
        }
    }
}

