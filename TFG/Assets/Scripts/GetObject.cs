using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetObject : MonoBehaviour
{
    public GameObject handPoint;
    public GameObject pickedObject = null;
    public string mensaje;
    public int contadorLlave = 0;
    public int contadorTrozo = 0;

    public float tiempoDeMensaje = 1f;
    private float tiempoMostrandoMensaje = 0f;
    private bool mostrandoMensaje = false;

    public string tagVent = "ventilacion";
    private bool ventActive = false;
    public string tagCambioEscena = "cambioEscena";

    public Font customFont; // Referencia a la fuente personalizada

    public Animator aniFade;
    public GameObject fadePanel; // Referencia al panel de fade-out
    private bool isTransitioning = false; // Controla si se está realizando una transición de escena

    // Variables públicas para contar las veces que se ha destruido el objeto
    public int contadorPilaFina = 0;
    public int contadorPilaAncha = 0;

    public static string currentScene;
    public static string prevScene;

    public GameObject llave;
    public GameObject amuleto;

    public Animator animator; // Añadir una referencia al Animator

    public void Start()
    {
        Debug.Log("Start method called.");

        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Current scene: " + currentScene);

        if (llave != null)
        {
            if (!ControladorLlave.Instance.ThereIsAKey())
            {
                Destroy(llave);
                Debug.Log("Llave destruida porque no hay llave en Door.");
            }
            else
            {
                Debug.Log("Llave no destruida porque hay llave en Door.");
            }
        }
        else
        {
            Debug.Log("Llave es null.");
        }

        if (amuleto != null)
        {
            if (!ControladorAmuleto.Instance.ThereIsAnAmulet())
            {
                Destroy(amuleto);
                Debug.Log("Amuleto destruido porque no hay amuleto en ControladorAmuleto.");
            }
            else
            {
                Debug.Log("Amuleto no destruido porque hay amuleto en ControladorAmuleto.");
            }
        }
        else
        {
            Debug.Log("Amuleto es null.");
        }
    }

    void Update()
    {
        if (pickedObject != null)
        {
            mensaje = "Pulsa ' R ' para soltar";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("r"))
            {
                Debug.Log("Tecla 'R' presionada.");
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.gameObject.transform.SetParent(null);
                pickedObject = null;
                animator.SetBool("Agarrado", false);
                Debug.Log("Objeto soltado.");
            }
        }

        if (mostrandoMensaje)
        {
            tiempoMostrandoMensaje += Time.deltaTime;
            if (tiempoMostrandoMensaje >= tiempoDeMensaje)
            {
                mostrandoMensaje = false;
                mensaje = "";
                Debug.Log("Mensaje ocultado.");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay called with " + other.gameObject.name);

        if (other.gameObject.CompareTag("PickableObject"))
        {
            Debug.Log("PickableObject detected.");
            mensaje = "Pulsa ' E '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("e") && pickedObject == null)
            {
                Debug.Log("Tecla 'E' presionada y no hay objeto recogido.");
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }
                other.transform.position = handPoint.transform.position;
                other.gameObject.transform.SetParent(handPoint.gameObject.transform);
                pickedObject = other.gameObject;
                animator.SetBool("Agarrado", true);
                Debug.Log("Objeto recogido: " + pickedObject.name);
            }
        }
        else if (other.gameObject.CompareTag("puerta"))
        {
            Debug.Log("Trigger con puerta detectado.");
            if (!ControladorLlave.Instance.IsKeyCollected() && !ventActive)
            {
                mensaje = "Pulsa ' F ' para abrir";
                mostrandoMensaje = true;
                tiempoMostrandoMensaje = 0f;

                if (Input.GetKey("f"))
                {
                    mensaje = "Necesitas una llave para abrir la puerta.";
                    mostrandoMensaje = true;
                    tiempoMostrandoMensaje = 0f;

                    ventActive = true;

                    GameObject objetoConGravedad = GameObject.FindGameObjectWithTag(tagVent);
                    if (objetoConGravedad != null)
                    {
                        Rigidbody rb = objetoConGravedad.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                        }
                    }
                    Debug.Log("Intento de abrir puerta sin llave.");
                }
            }
            else if (ControladorLlave.Instance.IsKeyCollected() && !ventActive)
            {
                mensaje = "Pulsa ' F ' para abrir con la llave";
                mostrandoMensaje = true;
                tiempoMostrandoMensaje = 0f;

                if (Input.GetKeyDown("f"))
                {
                    Debug.Log("Puerta abierta con llave.");
                    AbrirPuertaConLlave(other.gameObject);
                }
            }
        }
        else if (other.gameObject.CompareTag("key"))
        {
            Debug.Log("Llave detectada.");
            mensaje = "Pulsa ' F ' para recoger la llave";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                Debug.Log("Llave recogida.");
                contadorLlave++;
                ControladorLlave.Instance.SumarLlave(contadorLlave);
                Destroy(other.gameObject);
                mensaje = "Llave recogida. Volviendo a la habitación...";
                mostrandoMensaje = true;
                tiempoMostrandoMensaje = 0f;

                ActivateFadeOutToRoom();
            }
        }
        else if (other.gameObject.CompareTag("pila"))
        {
            Debug.Log("Pila fina detectada.");
            mensaje = "Pulsa ' F '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                Debug.Log("Pila fina recogida.");
                contadorPilaFina++;
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("pilaAncha"))
        {
            Debug.Log("Pila ancha detectada.");
            mensaje = "Pulsa ' F '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                Debug.Log("Pila ancha recogida.");
                contadorPilaAncha++;
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("trozo"))
        {
            Debug.Log("Trozo de amuleto detectado.");
            mensaje = "Pulsa ' F '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                Debug.Log("Trozo de amuleto recogido.");
                contadorTrozo++;
                ControladorAmuleto.Instance.SumarTrozo(contadorTrozo);
                Destroy(other.gameObject);
                ControladorAmuleto.Instance.amuletInScene = false;
            }
        }
        else if (other.gameObject.CompareTag("FinDemo"))
        {
            Debug.Log("Fin de demo detectado.");
            mensaje = "Pulsa ' F '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                Debug.Log("Fin de la demo activado.");
                ActivateFadeOutToMenu();
            }
        }
        else if (other.gameObject.CompareTag(tagCambioEscena) && ventActive && !isTransitioning)
        {
            Debug.Log("Cambio de escena detectado.");
            isTransitioning = true;
            ActivateFadeOut();
        }
        else if (other.gameObject.CompareTag("puertaAccess"))
        {
            Debug.Log("Puerta de acceso detectada.");
            mensaje = "Pulsa ' F ' para abrir";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                Debug.Log("Puerta de acceso abierta.");
                ActivateFadeOutToRoom();
            }
        }
        else if (other.gameObject.CompareTag("puertaAlm"))
        {
            Debug.Log("Puerta de Almacén 1 detectada.");
            mensaje = "Pulsa ' F ' - ALMACEN 1";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                prevScene = currentScene;
                Debug.Log("Transición al Almacén 1.");
                ActivateFadeOutToAlm();
            }
        }
        else if (other.gameObject.CompareTag("puertaAlm1"))
        {
            Debug.Log("Puerta de Almacén 2 detectada.");
            mensaje = "Pulsa ' F ' - ALMACEN 2";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                prevScene = currentScene;
                Debug.Log("Transición al Almacén 2.");
                ActivateFadeOutToAlm2();
            }
        }
    }

    private void ActivateFadeOut()
    {
        Debug.Log("Activando FadeOut para cambio de escena.");
        fadePanel.SetActive(true);
        aniFade.SetTrigger("FadeOut");
        Invoke("LoadNextScene", 1f); // Asumiendo que se llama LoadNextScene para cargar la siguiente escena
    }

    private void LoadNextScene()
    {
        Debug.Log("Cargando la siguiente escena.");
        // Código para cargar la siguiente escena
    }

    private void AbrirPuertaConLlave(GameObject puerta)
    {
        // Código para abrir la puerta con llave
        Debug.Log("AbrirPuertaConLlave llamado para la puerta: " + puerta.name);
    }

    private void ActivateFadeOutToRoom()
    {
        // Código para activar FadeOut y cambiar a la habitación
        Debug.Log("Activando FadeOut para volver a la habitación.");
    }

    private void ActivateFadeOutToMenu()
    {
        // Código para activar FadeOut y cambiar al menú
        Debug.Log("Activando FadeOut para cambiar al menú.");
    }

    private void ActivateFadeOutToAlm()
    {
        // Código para activar FadeOut y cambiar a Almacén 1
        Debug.Log("Activando FadeOut para cambiar a Almacén 1.");
    }

    private void ActivateFadeOutToAlm2()
    {
        // Código para activar FadeOut y cambiar a Almacén 2
        Debug.Log("Activando FadeOut para cambiar a Almacén 2.");
    }
}
