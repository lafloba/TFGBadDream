using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CodigoSalud : MonoBehaviour
{
    public float Salud = 100;
    public float SaludMaxima = 100;
    public CanvasGroup da�ado;

    public Image BarraSalud;

    // Update is called once per frame
    void Update()
    {
        ActualizarInterfaz();
    }

    public void recibirDa�o(float da�o)
    {
        Salud -= da�o;
        da�ado.alpha = 1 - Salud/SaludMaxima;

        if (Salud <= 0)
        {
            ReiniciarEscena();
        }
    }

    void ActualizarInterfaz()
    {
        BarraSalud.fillAmount = Salud / SaludMaxima;
    }

    void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
