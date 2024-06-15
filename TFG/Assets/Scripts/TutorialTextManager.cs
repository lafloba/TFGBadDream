using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTextManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // Arrastra tu objeto TextMeshPro aqu� en el Inspector
    public List<string> tutorialMessages;
    public AudioSource backgroundMusic; // Arrastra tu fuente de audio aqu� en el Inspector
    public GameObject tutorialPanel; // Arrastra tu panel del tutorial aqu� en el Inspector
    public GameObject pilafina; // Arrastra tu GameObject "pilafina" aqu� en el Inspector

    private int currentMessageIndex = 0;
    private bool spaceKeyBlocked = false;
    private bool endOfMessages = false;

    void Start()
    {
        // Ocultar el panel y el GameObject pilafina al inicio
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        if (pilafina != null)
        {
            pilafina.SetActive(false);
        }

        // Iniciar la corrutina para mostrar el mensaje despu�s de 1 segundo
        StartCoroutine(ShowFirstMessageAfterDelay(1f));
    }

    IEnumerator ShowFirstMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Mostrar el panel y el primer mensaje
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }

        ShowMessage();
    }

    void ShowMessage()
    {
        if (tutorialMessages.Count > 0)
        {
            tutorialText.text = tutorialMessages[currentMessageIndex];
        }
    }

    void Update()
    {
        if (!endOfMessages && pilafina == null && !tutorialPanel.activeSelf)
        {
            // Mostrar el panel si pilafina es null y a�n no hemos alcanzado el final de los mensajes
            tutorialPanel.SetActive(true);
            spaceKeyBlocked = false;
        }

        // Procesar entrada de teclado si la tecla espacio no est� bloqueada
        if (!spaceKeyBlocked && Input.GetKeyDown(KeyCode.Space))
        {
            // Avanzar al siguiente mensaje si hay m�s
            if (currentMessageIndex < tutorialMessages.Count - 1)
            {
                currentMessageIndex++;
                ShowMessage();

                // Desactivar panel y bloquear tecla espacio despu�s del mensaje 7
                if (currentMessageIndex == 7)
                {
                    tutorialPanel.SetActive(false);
                    spaceKeyBlocked = true;

                    // Activar pilafina despu�s del mensaje 7
                    if (pilafina != null)
                    {
                        pilafina.SetActive(true);
                    }
                }
            }
            else
            {
                // Si no hay m�s mensajes, ocultar el panel
                tutorialPanel.SetActive(false);
                endOfMessages = true;

                // Reanudar el tiempo
                Time.timeScale = 1f;
                if (backgroundMusic != null)
                {
                    backgroundMusic.ignoreListenerPause = false;
                }

                Debug.Log("End of tutorial messages.");
            }
        }
    }

    public void OnPilafinaCollected()
    {
        // Destruir el objeto pilafina
        if (pilafina != null)
        {
            Destroy(pilafina);
        }
    }
}







