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
    void Update()
    {
        if (pickedObject != null)
        {
            mensaje = "Pulsa ' R ' para soltar";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("r"))
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.gameObject.transform.SetParent(null);
                pickedObject = null;

            }
        }

        if (mostrandoMensaje)
        {
            tiempoMostrandoMensaje += Time.deltaTime;
            if (tiempoMostrandoMensaje >= tiempoDeMensaje)
            {
                mostrandoMensaje = false;
                mensaje = "";
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PickableObject"))
        {
            mensaje = "Pulsa ' E ' para agarrar";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("e") && pickedObject == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = handPoint.transform.position;
                other.gameObject.transform.SetParent(handPoint.gameObject.transform);
                pickedObject = other.gameObject;
            }
        }

        else if (other.gameObject.CompareTag("puerta"))
        {
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
                }
            }

            else if (ControladorLlave.Instance.IsKeyCollected() && !ventActive)
            {
                mensaje = "Pulsa ' F ' para abrir con la llave";
                mostrandoMensaje = true;
                tiempoMostrandoMensaje = 0f;

                if (Input.GetKeyDown("f"))
                {
                    AbrirPuertaConLlave(other.gameObject);
                }
            }
        }

        else if (other.gameObject.CompareTag("key"))
        {
            mensaje = "Pulsa ' F ' para recoger la llave";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                contadorLlave++;
                ControladorLlave.Instance.SumarLlave(contadorLlave);
                Destroy(other.gameObject);
                mensaje = "Llave recogida. Volviendo a la habitación...";
                mostrandoMensaje = true;
                tiempoMostrandoMensaje = 0f;

                // Iniciar la transición de regreso a la habitación
                ActivateFadeOutToRoom();
            }
        }

        else if (other.gameObject.CompareTag("key2"))
        {
            mensaje = "Pulsa ' F ' para recoger la llave";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                contadorLlave++;
                ControladorLlave.Instance.SumarLlave(contadorLlave);
                Destroy(other.gameObject);
            }
        }

        else if (other.gameObject.CompareTag("pila"))
        {
            mensaje = "Pulsa ' F '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                // Incrementar el contador
                contadorPilaFina++;

                // Destruir el objeto
                Destroy(other.gameObject);
            }
        }

        else if (other.gameObject.CompareTag("trozo"))
        {
            mensaje = "Pulsa ' F '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                contadorTrozo++;
                ControladorAmuleto.Instance.SumarTrozo(contadorTrozo);
                Destroy(other.gameObject);
            }
        }

        else if (other.gameObject.CompareTag("FinDemo"))
        {
            mensaje = "Pulsa ' F '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                ActivateFadeOutToMenu();
            }
        }

        else if (other.gameObject.CompareTag("pilaAncha"))
        {
            mensaje = "Pulsa ' F '";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;

            if (Input.GetKey("f"))
            {
                // Incrementar el contador
                contadorPilaAncha++;

                // Destruir el objeto
                Destroy(other.gameObject);
            }
        }

        else if (other.gameObject.CompareTag(tagCambioEscena) && ventActive && !isTransitioning)
        {
            isTransitioning = true;
            ActivateFadeOut();
        }

        else if (other.gameObject.CompareTag("puertaAccess"))
        {
            mensaje = "Pulsa ' F ' para abrir";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                // Iniciar la transición de regreso a la habitación
                ActivateFadeOutToRoom();
            }

        }
        else if (other.gameObject.CompareTag("puertaAlm"))
        {
            mensaje = "Pulsa ' F ' - ALMACEN 1";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                ControladorEscenasPasillo.Instance.GuardarPosicionJugador(transform.position);
                ActivateFadeOutToAlm();
            }

        }
        else if (other.gameObject.CompareTag("puertaAlm1"))
        {
            mensaje = "Pulsa ' F ' - ALMACEN 2";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                // Iniciar la transición de regreso a la habitación
                ActivateFadeOutToAlm1();
            }

        }

        else if (other.gameObject.CompareTag("puertaCorridor"))
        {
            mensaje = "Pulsa ' F ' para abrir";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                // Iniciar la transición de regreso a la habitación

                ActivateFadeOutToCorridor();

            }

        }

        else if (other.gameObject.CompareTag("puertaRoom1"))
        {
            mensaje = "Pulsa ' F ' - HABITACION 1";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                // Iniciar la transición de regreso a la habitación
                ActivateFadeOutToRoom1();
            }

        }

        else if (other.gameObject.CompareTag("puertaRoom2"))
        {
            mensaje = "Pulsa ' F ' - HABITACION 2";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                // Iniciar la transición de regreso a la habitación
                ActivateFadeOutToRoom2();
            }

        }

        else if (other.gameObject.CompareTag("puertaBlock"))
        {
            mensaje = "Pulsa ' F ' para abrir";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f;
            if (Input.GetKey("f"))
            {
                mensaje = "No se puede pasar";
                mostrandoMensaje = true;
                tiempoMostrandoMensaje = 0f;
            }

        }

    }

    void OnGUI()
    {
        if (mostrandoMensaje)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 45;
            if (customFont != null)
            {
                style.font = customFont; // Asigna la fuente personalizada
            }
            float width = 800;
            float height = style.CalcHeight(new GUIContent(mensaje), width);
            Rect rect = new Rect(Screen.width / 2 - width / 2, 50, width, height);
            GUI.Label(rect, mensaje, style);
        }
    }

    public void ActivateFadeOut()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadNextScene", 1.5f);
    }

    public void ActivateFadeOutToMenu()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadMenu", 1.5f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Hall");
        isTransitioning = false;
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        isTransitioning = false;
    }

    public void ActivateFadeOutToRoom()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadRoomScene", 1.5f);
    }

    public void ActivateFadeOutToRoom1()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadRoom1Scene", 1.5f);
    }

    public void ActivateFadeOutToRoom2()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadRoom2Scene", 1.5f);
    }

    public void ActivateFadeOutToAlm()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadAlmScene", 1.5f);
    }

    public void ActivateFadeOutToAlm1()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadAlm1Scene", 1.5f);
    }

    void LoadRoomScene()
    {
        SceneManager.LoadScene("Habitacion");
        isTransitioning = false;
    }

    void LoadRoom1Scene()
    {
        SceneManager.LoadScene("Habitacion 1");
        isTransitioning = false;
    }

    void LoadRoom2Scene()
    {
        SceneManager.LoadScene("Habitacion 2");
        isTransitioning = false;
    }

    void LoadAlmScene()
    {
        SceneManager.LoadScene("Almacen");
        isTransitioning = false;
        
    }

    void LoadAlm1Scene()
    {
        SceneManager.LoadScene("Almacen 1");
        isTransitioning = false;
    }

    public void ActivateFadeOutToCorridor()
    {
        aniFade.SetBool("fade", true);
        Invoke("LoadCorridorScene", 1.5f);
    }

    void LoadCorridorScene()
    {
        SceneManager.LoadScene("Corridor");
        isTransitioning = false;
        ControladorEscenasPasillo.Instance.posicionGuardada = true;
    }

    public int GetContadorPilaFina()
    {
        return contadorPilaFina;
    }

    public int GetContadorPilaAncha()
    {
        return contadorPilaAncha;
    }

        void AbrirPuertaConLlave(GameObject puerta)
        {
            // Asegúrate de actualizar el mensaje antes de ocultarlo
            mensaje = ""; // Limpiar el mensaje
            mostrandoMensaje = false;

            ventActive = false;

            GameObject objetoConGravedad = GameObject.FindGameObjectWithTag(tagVent);
            if (objetoConGravedad != null)
            {
                Rigidbody rb = objetoConGravedad.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
            Destroy(puerta);

            ActivateFadeOutToCorridor();
        }
    }
