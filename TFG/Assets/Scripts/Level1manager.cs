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
        // Make sure the collider is initially disabled
        if (colisionadorAAparecer != null)
        {
            colisionadorAAparecer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Door.Instance.IsKeyCollected() && ControladorAmuleto.Instance.IsTrozoCollected())
        {
            if (objetoADestruir != null)
            {
                Destroy(objetoADestruir);
                objetoADestruir = null; // Ensure we don't try to destroy it again
            }

            if (colisionadorAAparecer != null)
            {
                colisionadorAAparecer.enabled = true;
            }
        }
    }
}