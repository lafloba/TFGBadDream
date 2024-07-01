using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CinematicController : MonoBehaviour
{
    public Image[] images; // Array de im�genes
    public TextMeshProUGUI textComponent; // Componente de texto (usar Text si no usas TextMeshPro)
    public string[] messages; // Array de mensajes
    public float fadeDuration = 1.5f; // Duraci�n del fade
    private int currentMessageIndex = 0;

    public Animator aniFade;
    public GameObject fadePanel; // Referencia al panel de fade
    private bool isTransitioning = false; // Controla si se est� realizando una transici�n de escena

    void Start()
    {
        fadePanel.SetActive(true); // Aseg�rate de que el panel de fade est� activo
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
            // Cargar la escena "Habitacion" cuando no haya m�s mensajes
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(ActivateFadeOut());
            }
        }
    }

    void ShowCurrentImage()
    {
        // Desactiva todas las im�genes
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
        }

        // Activa la imagen correspondiente seg�n el �ndice del mensaje
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
        yield return new WaitForSeconds(fadeDuration); // Espera la duraci�n del fade-in
        fadePanel.SetActive(false); // Desactiva el panel de fade despu�s del fade-in
    }

    IEnumerator ActivateFadeOut()
    {
        fadePanel.SetActive(true); // Aseg�rate de que el panel de fade est� activo
        aniFade.SetBool("fade", true);
        yield return new WaitForSeconds(fadeDuration); // Espera la duraci�n del fade-out
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Habitacion");
    }
}
