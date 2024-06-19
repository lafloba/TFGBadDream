using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialTextManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // Arrastra tu objeto TextMeshPro aquí en el Inspector
    public List<string> tutorialMessages;
    public AudioSource backgroundMusic; // Arrastra tu fuente de audio aquí en el Inspector
    public GameObject tutorialPanel; // Arrastra tu panel del tutorial aquí en el Inspector
    public GameObject pilafina; // Arrastra tu GameObject "pilafina" aquí en el Inspector
    public GameObject pilafina1; // Arrastra tu GameObject "pilafina1" aquí en el Inspector
    public GameObject pilafina2; // Arrastra tu GameObject "pilafina2" aquí en el Inspector
    public GameObject pilafina3; // Arrastra tu GameObject "pilafina3" aquí en el Inspector
    public GameObject monster; // Arrastra tu objeto "monstruo" aquí en el Inspector
    public GameObject monster1; // Arrastra tu objeto "monstruo1" aquí en el Inspector
    public GameObject monster2; // Arrastra tu objeto "monstruo2" aquí en el Inspector
    public GameObject monster3; // Arrastra tu objeto "monstruo3" aquí en el Inspector
    public GameObject monster4; // Arrastra tu objeto "monstruo4" aquí en el Inspector
    public GameObject monster5; // Arrastra tu objeto "monstruo5" aquí en el Inspector
    public GameObject pilaancha; // Arrastra tu GameObject "pilaancha" aquí en el Inspector
    public GameObject pilaancha1; // Arrastra tu GameObject "pilaancha1" aquí en el Inspector
    public GameObject key; // Arrastra tu GameObject "key" aquí en el Inspector

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
        if (pilafina1 != null)
        {
            pilafina1.SetActive(false);
        }
        if (pilafina2 != null)
        {
            pilafina2.SetActive(false);
        }
        if (pilafina3 != null)
        {
            pilafina3.SetActive(false);
        }
        if (monster != null)
        {
            monster.SetActive(false);
        }
        if (monster1 != null)
        {
            monster1.SetActive(false);
        }
        if (monster2 != null)
        {
            monster2.SetActive(false);
        }
        if (monster3 != null)
        {
            monster3.SetActive(false);
        }
        if (monster4 != null)
        {
            monster4.SetActive(false);
        }
        if (monster5 != null)
        {
            monster5.SetActive(false);
        }
        if (pilaancha != null)
        {
            pilaancha.SetActive(false);
        }
        if (pilaancha1 != null)
        {
            pilaancha1.SetActive(false);
        }
        if (key != null)
        {
            key.SetActive(false);
        }

        // Iniciar la corrutina para mostrar el mensaje después de 1 segundo
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
            // Mostrar el panel si pilafina es null y aún no hemos alcanzado el final de los mensajes
            tutorialPanel.SetActive(true);
            spaceKeyBlocked = false;
        }

        // Procesar entrada de teclado si la tecla espacio no está bloqueada
        if (!spaceKeyBlocked && Input.GetKeyDown(KeyCode.Space))
        {
            // Avanzar al siguiente mensaje si hay más
            if (currentMessageIndex < tutorialMessages.Count - 1)
            {
                currentMessageIndex++;
                ShowMessage();

                // Desactivar panel y bloquear tecla espacio después del mensaje 7
                if (currentMessageIndex == 7)
                {
                    tutorialPanel.SetActive(false);
                    spaceKeyBlocked = true;

                    // Activar pilafina después del mensaje 7
                    if (pilafina != null)
                    {
                        pilafina.SetActive(true);
                    }
                }
            }
            else
            {
                // Si no hay más mensajes, ocultar el panel
                tutorialPanel.SetActive(false);
                endOfMessages = true;

                // Reanudar el tiempo
                Time.timeScale = 1f;
                if (backgroundMusic != null)
                {
                    backgroundMusic.ignoreListenerPause = false;
                }

                Debug.Log("End of tutorial messages.");

                // Activar el objeto "key"
                if (key != null)
                {
                    key.SetActive(true);
                }
            }
        }

        // Desactivar el panel en el mensaje 12, independientemente de la entrada del teclado
        if (currentMessageIndex == 12) // Asumiendo que el índice comienza desde 0
        {
            tutorialPanel.SetActive(false);
            spaceKeyBlocked = true; // Opcionalmente, bloquear el avance con la tecla espacio

            // Activar el objeto "monstruo"
            if (monster != null)
            {
                monster.SetActive(true);
            }
            else
            {
                // Si el objeto "monstruo" es null, reactivar los mensajes
                tutorialPanel.SetActive(true);
                spaceKeyBlocked = false;

            }
        }

        // Desactivar el panel en el mensaje 15, independientemente de la entrada del teclado
        if (currentMessageIndex == 15) // Asumiendo que el índice comienza desde 0
        {
            tutorialPanel.SetActive(false);
            spaceKeyBlocked = true; // Opcionalmente, bloquear el avance con la tecla espacio

            // Activar el objeto "monstruo"
            if (pilaancha != null)
            {
                pilaancha.SetActive(true);
            }
            else
            {
                // Si el objeto "monstruo" es null, reactivar los mensajes
                tutorialPanel.SetActive(true);
                spaceKeyBlocked = false;

            }
        }

        // Desactivar el panel en el mensaje 18, independientemente de la entrada del teclado
        if (currentMessageIndex == 18) // Asumiendo que el índice comienza desde 0
        {
            tutorialPanel.SetActive(false);
            spaceKeyBlocked = true; // Opcionalmente, bloquear el avance con la tecla espacio

            // Activar el objeto "monstruo"
            if (monster1 != null)
            {
                monster1.SetActive(true);
            }
            else
            {
                // Si el objeto "monstruo" es null, reactivar los mensajes
                tutorialPanel.SetActive(true);
                spaceKeyBlocked = false;

            }
        }

        // Desactivar el panel en el mensaje 29, independientemente de la entrada del teclado
        if (currentMessageIndex == 29) // Asumiendo que el índice comienza desde 0
        {
            tutorialPanel.SetActive(false);
            spaceKeyBlocked = true; // Opcionalmente, bloquear el avance con la tecla espacio

            // Activar el objeto "monstruo" y pila
            if (monster2 != null || pilafina1 != null)
            {
                monster2.SetActive(true);
                pilafina1.SetActive(true);
            }
            else
            {
                // Si el objeto "monstruo" es null, reactivar los mensajes
                tutorialPanel.SetActive(true);
                spaceKeyBlocked = false;

            }
        }

        // Desactivar el panel en el mensaje 33, independientemente de la entrada del teclado
        if (currentMessageIndex == 33) // Asumiendo que el índice comienza desde 0
        {
            tutorialPanel.SetActive(false);
            spaceKeyBlocked = true; // Opcionalmente, bloquear el avance con la tecla espacio

            // Activar el objeto "monstruo" y pila
            if (monster3 != null || monster4 != null || monster5 != null)
            {
                monster3.SetActive(true);
                monster4.SetActive(true);
                monster5.SetActive(true);
                pilafina2.SetActive(true);
                pilafina3.SetActive(true);
                pilaancha1.SetActive(true);
            }
            else
            {
                // Si el objeto "monstruo" es null, reactivar los mensajes
                tutorialPanel.SetActive(true);
                spaceKeyBlocked = false;

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
