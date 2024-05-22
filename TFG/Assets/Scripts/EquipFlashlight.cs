using UnityEngine;

public class EquipFlashlight : MonoBehaviour
{
    public GameObject flashlightPrefab;
    private GameObject flashlightInstance;
    public Transform flashlightHolder;
    public Vector3 flashlightScale = new Vector3(1, 1, 1);
    public Vector3 flashlightRotation = new Vector3(90, 0, 0);  // Ajustar la rotación de la linterna
    public Animator animator;  // Añadir una referencia al Animator

    private bool isEquipped = false;
    private GetObject getObjectScript;
    private float equipDuration = 0f;
    private float equipStartTime = 0f;

    void Start()
    {
        // Obtener la referencia al script GetObject
        getObjectScript = FindObjectOfType<GetObject>();

        // Verificar si se obtuvo la referencia correctamente
        if (getObjectScript == null)
        {
            Debug.LogError("No se encontró el script GetObject en la escena.");
        }
        else
        {
            Debug.Log("Script GetObject encontrado.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (getObjectScript.contadorPilaFina > 0)
            {
                Equip(5f);
                getObjectScript.contadorPilaFina--;
            }
            else if (getObjectScript.contadorPilaAncha > 0)
            {
                Equip(10f);
                getObjectScript.contadorPilaAncha--;
            }
            else
            {
                Debug.Log("No hay pila disponible para equipar la linterna.");
            }
        }

        // Check if the flashlight is equipped and the duration has passed
        if (isEquipped && Time.time - equipStartTime >= equipDuration)
        {
            Unequip();
        }
    }

    void Equip(float duration)
    {
        equipDuration = duration;
        equipStartTime = Time.time;

        if (flashlightInstance == null)
        {
            flashlightInstance = Instantiate(flashlightPrefab, flashlightHolder);
            flashlightInstance.transform.localPosition = Vector3.zero;
            flashlightInstance.transform.localRotation = Quaternion.Euler(flashlightRotation);  // Ajustar la rotación de la linterna
            flashlightInstance.transform.localScale = flashlightScale;
        }

        if (animator != null)
        {
            animator.SetBool("isEquipping", true);  // Activar la animación
        }

        isEquipped = true;
        Debug.Log("Linterna equipada por " + duration + " segundos.");
    }

    void Unequip()
    {
        if (flashlightInstance != null)
        {
            Destroy(flashlightInstance);
            flashlightInstance = null;
        }

        if (animator != null)
        {
            animator.SetBool("isEquipping", false);  // Desactivar la animación
        }

        isEquipped = false;
        Debug.Log("Linterna desequipada.");
    }
}
