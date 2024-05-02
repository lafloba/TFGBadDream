using UnityEngine;
using UnityEngine.SceneManagement;

public class GetObject : MonoBehaviour
{
    public GameObject handPoint;
    //public int contadorManta = 0;
    public int contadorLlave = 0;
    public GameObject pickedObject = null;
    //private GameObject destroyObject = null;
    public string mensaje;

    public float tiempoDeMensaje = 2f; // Duración del mensaje en segundos
    private float tiempoMostrandoMensaje = 0f;
    private bool mostrandoMensaje = false;

    // Tamaño de la fuente del texto
    public int fontSize = 62;


    public string tagVent = "ventilacion";
    private bool ventActive = false;
    public string tagCambioEscena = "cambioEscena";

    // Update is called once per frame
    void Update()
    {
        if (pickedObject != null)
        {
            mensaje = "Pulsa 'R' para soltar";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f; // Reiniciar el tiempo de mostrar mensaje


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
                mensaje = ""; // Limpiar el mensaje después del tiempo especificado
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PickableObject"))
        {
            mensaje = "Pulsa 'E' para agarrar";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f; // Reiniciar el tiempo de mostrar mensaje

            if (Input.GetKey("e") && pickedObject == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = handPoint.transform.position;
                other.gameObject.transform.SetParent(handPoint.gameObject.transform);
                pickedObject = other.gameObject;
            }
        }

        /*else if (other.gameObject.CompareTag("manta"))
        {
            destroyObject = other.gameObject;

            if (Input.GetKey("f"))
            {
                Destroy(destroyObject);
                contadorManta += 1;
            }
        }*/

        else if (other.gameObject.CompareTag("puerta"))
        {
            mensaje = "Pulsa 'F' para abrir";
            mostrandoMensaje = true;
            tiempoMostrandoMensaje = 0f; // Reiniciar el tiempo de mostrar mensaje

            if (Input.GetKey("f") && contadorLlave == 0 && !ventActive)
            {
                mensaje = "Necesitas una llave para abrir la puerta.";
                mostrandoMensaje = true;
                tiempoMostrandoMensaje = 0f; // Reiniciar el tiempo de mostrar mensaje

                ventActive = true;

                GameObject objetoConGravedad = GameObject.FindGameObjectWithTag(tagVent);
                if (objetoConGravedad != null)
                {
                    Rigidbody rb = objetoConGravedad.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false; // Activa la gravedad en el objeto afectado
                    }
                }
            }
        }

        else if (other.gameObject.CompareTag(tagCambioEscena) && ventActive)
        {
            Debug.Log("Colisiona");
            SceneManager.LoadScene("Hall");
        }
    }

    // Método para mostrar el mensaje en la interfaz de usuario
    void OnGUI()
    {
        if (mostrandoMensaje)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 45; // Ajusta el tamaño de la fuente según tus necesidades
            float width = 800; // Ancho del rectángulo
            float height = style.CalcHeight(new GUIContent(mensaje), width); // Calcula la altura del rectángulo según el texto
            Rect rect = new Rect(Screen.width / 2 - width / 2, 50, width, height); // Posición y dimensiones del rectángulo
            GUI.Label(rect, mensaje, style);
        }
    }
}


