using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

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
        }
        else
        {
            // Cargar la escena "Habitacion" cuando no haya más mensajes
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(ActivateFadeOut());
            }
            return; // Salir de la función para evitar mostrar una imagen adicional
        }

        // Mostrar la imagen correspondiente al mensaje actual
        ShowCurrentImage();
    }

    void ShowCurrentImage()
    {
        // Restablece el zoom de la imagen actual
        ResetZoom();

        // Activa la imagen correspondiente según el índice del mensaje
        int imageIndex = GetImageIndexForMessage(currentMessageIndex);
        if (imageIndex >= 0 && imageIndex < images.Length)
        {
            images[imageIndex].gameObject.SetActive(true);
            StartCoroutine(ZoomAnimation(images[imageIndex]));
        }
        else
        {
            Debug.LogWarning("Índice de imagen no válido para el mensaje actual.");
        }
    }

    int GetImageIndexForMessage(int messageIndex)
    {
        if (messageIndex < 3) return 0;
        if (messageIndex >= 3 && messageIndex < 5) return 1;
        if (messageIndex >= 5 && messageIndex < 7) return 2;
        if (messageIndex >= 7 && messageIndex < 9) return 3;
        return 4;
    }

    IEnumerator ZoomAnimation(Image image)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();

        float zoomFactor = 1.5f; // Puedes ajustar este valor según tus necesidades
        float duration = 10.0f; // Aumenta este valor para hacer la animación más lenta
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * zoomFactor, progress);
            yield return null;
        }

        // Asegura que la imagen se mantenga en su estado de zoom final
        rectTransform.localScale = Vector3.one * zoomFactor;
    }

    void ResetZoom()
    {
        // Desactiva todas las imágenes
        foreach (var img in images)
        {
            img.gameObject.SetActive(false);
        }

        // Encuentra la imagen actualmente activa y la vuelve a activar
        int activeImageIndex = -1;
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].gameObject.activeSelf)
            {
                activeImageIndex = i;
                break;
            }
        }

        // Si hay un índice de imagen activa válido, restablece su escala
        if (activeImageIndex != -1)
        {
            StartCoroutine(ResetZoomCoroutine(images[activeImageIndex]));
        }
    }

    IEnumerator ResetZoomCoroutine(Image image)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();

        float duration = 0.5f; // Duración del reset de zoom
        float startTime = Time.time;
        Vector3 initialScale = rectTransform.localScale;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            rectTransform.localScale = Vector3.Lerp(initialScale, Vector3.one, progress);
            yield return null;
        }

        // Asegura que la imagen se mantenga en su estado de zoom final
        rectTransform.localScale = Vector3.one;
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
