using UnityEngine;
using UnityEngine.SceneManagement;

public class GetObject : MonoBehaviour
{
    public GameObject handPoint;
    public int contadorLlave = 0;
    public GameObject pickedObject = null;
    public string mensaje;

    public float tiempoDeMensaje = 2f;
    private float tiempoMostrandoMensaje = 0f;
    private bool mostrandoMensaje = false;

    public string tagVent = "ventilacion";
    private bool ventActive = false;
    public string tagCambioEscena = "cambioEscena";

    void Update()
    {
        if (pickedObject != null)
        {
            mensaje = "Pulsa 'R' para soltar";
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
            mensaje = "Pulsa 'E' para agarrar";
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
            if (contadorLlave == 0 && !ventActive)
            {
                mensaje = "Pulsa 'F' para abrir";
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
        }

        else if (other.gameObject.CompareTag(tagCambioEscena) && ventActive)
        {
            SceneManager.LoadScene("Hall");
        }
    }

    void OnGUI()
    {
        if (mostrandoMensaje)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 45;
            float width = 800;
            float height = style.CalcHeight(new GUIContent(mensaje), width);
            Rect rect = new Rect(Screen.width / 2 - width / 2, 50, width, height);
            GUI.Label(rect, mensaje, style);
        }
    }
}


