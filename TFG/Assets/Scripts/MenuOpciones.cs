using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MenuOpciones : MonoBehaviour
{
    public Toggle muteToggle; // Referencia al Toggle
    public Toggle fullScreenToggle; // Referencia al Toggle pantalla completa
    public TMP_Dropdown resoluciones;
    Resolution[] tiposResoluciones;

    void Start()
    {
        // Asegúrate de que el toggle esté en el estado correcto al iniciar el juego
        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        muteToggle.isOn = isMuted;
        SetMute(isMuted);

        // Suscribirse al evento de cambio del Toggle
        muteToggle.onValueChanged.AddListener(delegate {
            MuteUnmute(muteToggle.isOn);
        });

        if (Screen.fullScreen)
        {
            fullScreenToggle.isOn = true;
        }
        else
        {
            fullScreenToggle.isOn = false;
        }

        RevisarResolucion();
    }

    public void MuteUnmute(bool isMuted)
    {
        SetMute(isMuted);
        // Guardar el estado del mute en PlayerPrefs
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    private void SetMute(bool isMuted)
    {
        AudioListener.volume = isMuted ? 0 : 1;
    }

    public void ActivarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }

    public void RevisarResolucion()
    {
        tiposResoluciones = Screen.resolutions;
        resoluciones.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for(int i=0; i<tiposResoluciones.Length; i++)
        {
            string opcion = tiposResoluciones[i].width + " x " + tiposResoluciones[i].height;
            opciones.Add(opcion);

            if(Screen.fullScreen && tiposResoluciones[i].width == Screen.currentResolution.width && tiposResoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }

        resoluciones.AddOptions(opciones);
        resoluciones.value = resolucionActual;
        resoluciones.RefreshShownValue();
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        Resolution resolucion = tiposResoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }
}


