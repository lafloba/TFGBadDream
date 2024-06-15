using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;

    private bool juegoPausado = false;

    public Animator aniFade;
    public GameObject fadePanel; // Referencia al panel de fade-out
    private bool isTransitioning = false; // Controla si se está realizando una transición de escena

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Resume();
            }
            else
            {
                Pausa();
            }
        }
    }

    public void Pausa()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
    }

    public void Resume()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(QuitRoutine());
        }
    }

    private IEnumerator QuitRoutine()
    {
        // Desactivar la pausa antes de iniciar el fade out
        Time.timeScale = 1f;
        fadePanel.SetActive(true);
        aniFade.SetBool("fade", true);

        // Esperar 1.5 segundos en tiempo real para que la animación de fade se complete
        yield return new WaitForSecondsRealtime(1.5f);

        // Cargar la escena del menú principal
        SceneManager.LoadScene(0);
        isTransitioning = false;
    }
}
