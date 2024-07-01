using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CinematicController : MonoBehaviour
{
    public Image[] images; // Array de imágenes
    public TextMeshProUGUI textComponent; // Componente de texto (usar Text si no usas TextMeshPro)
    public string[] messages; // Array de mensajes
    public float fadeDuration = 1.5f; // Duración del fade
    private int currentMessageIndex = 0;

    public Animator aniFade;
    public GameObject fadePanel; // Referencia al panel de fade
    private bool isTransitioning = false; // Controla si se está realizando una transición de escena

    void Start()
    {
        fadePanel.SetActive(true); // Asegúrate de que el panel de fade esté activo
        StartCoroutine(FadeIn());
        ShowCurrentMessage();
        ShowCurrentImage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning)
        {
            ShowNextMessage();
        }
    }

    void ShowCurrentMessage()
    {
        textComponent.text = messages[currentMessageIndex];
    }

    void ShowNextMessage()
    {
        currentMessageIndex++;
        if (currentMessageIndex < messages.Length)
        {
            ShowCurrentMessage();
            ShowCurrentImage();
        }
        else
        {
            // Cargar la escena "Habitacion" cuando no haya más mensajes
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(ActivateFadeOut());
            }
        }
    }

    void ShowCurrentImage()
    {
        // Desactiva todas las imágenes
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
        }

        // Activa la imagen correspondiente según el índice del mensaje
        if (currentMessageIndex < 3)
        {
            images[0].gameObject.SetActive(true); // Muestra la primera imagen para los primeros 4 mensajes
        }
        else if (currentMessageIndex >= 3 && currentMessageIndex < 5)
        {
            images[1].gameObject.SetActive(true); // Muestra la segunda imagen a partir del mensaje 5
        }
        else if (currentMessageIndex >= 5 && currentMessageIndex < 7)
        {
            images[2].gameObject.SetActive(true); // Muestra la segunda imagen a partir del mensaje 5
        }
        else if (currentMessageIndex >= 7 && currentMessageIndex < 9)
        {
            images[3].gameObject.SetActive(true); // Muestra la segunda imagen a partir del mensaje 5
        }
        else
        {
            images[4].gameObject.SetActive(true); // Muestra la segunda imagen a partir del mensaje 5
        }
    }

    IEnumerator FadeIn()
    {
        aniFade.SetBool("fade", false);
        yield return new WaitForSeconds(fadeDuration); // Espera la duración del fade-in
        fadePanel.SetActive(false); // Desactiva el panel de fade después del fade-in
    }

    IEnumerator ActivateFadeOut()
    {
        fadePanel.SetActive(true); // Asegúrate de que el panel de fade esté activo
        aniFade.SetBool("fade", true);
        yield return new WaitForSeconds(fadeDuration); // Espera la duración del fade-out
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Habitacion");
    }
}
