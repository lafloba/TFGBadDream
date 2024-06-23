using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorAmuleto : MonoBehaviour
{
    public static ControladorAmuleto Instance;

    [SerializeField] private float contadorTrozos;

    private void Awake()
    {
        if (ControladorAmuleto.Instance == null)
        {
            ControladorAmuleto.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarTrozo(float trozo)
    {
        contadorTrozos += trozo;
    }

    public bool IsTrozoCollected()
    {
        return contadorTrozos > 0;
    }
}