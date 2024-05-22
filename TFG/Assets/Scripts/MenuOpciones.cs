using UnityEngine;
using UnityEngine.UI;

public class MenuOpciones : MonoBehaviour
{
    public Toggle muteToggle; // Referencia al Toggle

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
}


