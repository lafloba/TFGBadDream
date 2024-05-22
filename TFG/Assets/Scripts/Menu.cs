using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator aniFade;
    public GameObject fadePanel; // Referencia al panel de fade-out
    private bool isTransitioning = false; // Controla si se está realizando una transición de escena


    public void Jugar()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            ActivateFadeOut();
        }
            
    }

    public void Salir()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }

    public void ActivateFadeOut()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadNextScene", 1.5f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        isTransitioning = false;
    }
}
