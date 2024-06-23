using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorLlave : MonoBehaviour
{
    public static ControladorLlave Instance;

    [SerializeField] private float contadorLlaves;

    private void Awake()
    {
        if(ControladorLlave.Instance == null)
        {
            ControladorLlave.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarLlave(float llaves)
    {
        contadorLlaves += llaves;
    }

    public bool IsKeyCollected()
    {
        return contadorLlaves > 0;
    }
}