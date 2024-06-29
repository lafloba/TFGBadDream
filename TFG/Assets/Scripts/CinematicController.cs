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
    public float imageDuration = 2f; // Duraci�n de cada imagen
    private int currentMessageIndex = 0;
    private int currentImageIndex = 0;

    public Animator aniFade;
    public GameObject fadePanel; // Referencia al panel de fade
    private bool isTransitioning = false; // Controla si se est� realizando una transici�n de escena

    void Start()
    {
        fadePanel.SetActive(true); // Aseg�rate de que el panel de fade est� activo
        StartCoroutine(FadeIn());
        StartCoroutine(CycleImages());
        ShowCurrentMessage();
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

    IEnumerator CycleImages()
    {
        while (true)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(i == currentImageIndex);
            }

            currentImageIndex = (currentImageIndex + 1) % images.Length;
            yield return new WaitForSeconds(imageDuration);
        }
    }

    IEnumerator FadeIn()
    {
        aniFade.SetBool("fade", false);
        yield return new WaitForSeconds(1.5f); // Asume que el fade-in dura 1.5 segundos
        fadePanel.SetActive(false); // Desactiva el panel de fade despu�s del fade-in
    }

    IEnumerator ActivateFadeOut()
    {
        fadePanel.SetActive(true); // Aseg�rate de que el panel de fade est� activo
        aniFade.SetBool("fade", true);
        yield return new WaitForSeconds(1.5f); // Espera la duraci�n del fade-out
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Habitacion");
    }
}
