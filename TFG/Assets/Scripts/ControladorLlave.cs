using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance;

    [SerializeField] private float contadorLlaves;

    private void Awake()
    {
        if(Door.Instance == null)
        {
            Door.Instance = this;
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
