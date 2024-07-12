using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    public GameObject objetoADestruir;
    public Collider colisionadorAAparecer;


    // Start is called before the first frame update
    void Start()
    {
        // Asegúrate de que el colisionador esté inicialmente desactivado
        if (colisionadorAAparecer != null)
        {
            colisionadorAAparecer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ControladorLlave.Instance.IsKeyCollected() && ControladorAmuleto.Instance.IsTrozoCollected())
        {
            if (objetoADestruir != null)
            {
                Destroy(objetoADestruir);
                objetoADestruir = null; // Asegúrate de no intentar destruirlo nuevamente
            }

            if (colisionadorAAparecer != null)
            {
                colisionadorAAparecer.enabled = true;
            }
        }
    }
}
