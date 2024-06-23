using UnityEngine;

public class ControladorEscenasPasillo : MonoBehaviour
{
    public static ControladorEscenasPasillo Instance;

    public Vector3 posicionJugador;
    public bool posicionGuardada = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarPosicionJugador(Vector3 posicion)
    {
        posicionJugador = posicion;
    }

    public Vector3 ObtenerPosicionJugador()
    {
        return posicionJugador;
    }

  
}
