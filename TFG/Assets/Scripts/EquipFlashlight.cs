using UnityEngine;

public class EquipFlashlight : MonoBehaviour
{
    public GameObject flashlightPrefab;
    private GameObject flashlightInstance;
    public Transform flashlightHolder;
    public Vector3 flashlightScale = new Vector3(1, 1, 1);
    public Vector3 flashlightRotation = new Vector3(90, 0, 0);  // Ajustar la rotaci�n de la linterna
    public Animator animator;  // A�adir una referencia al Animator

    private bool isEquipped = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isEquipped = !isEquipped;
            if (isEquipped)
            {
                Equip();
            }
            else
            {
                Unequip();
            }
        }
    }

    void Equip()
    {
        flashlightInstance = Instantiate(flashlightPrefab, flashlightHolder);
        flashlightInstance.transform.localPosition = Vector3.zero;
        flashlightInstance.transform.localRotation = Quaternion.Euler(flashlightRotation);  // Ajustar la rotaci�n de la linterna
        flashlightInstance.transform.localScale = flashlightScale;

        if (animator != null)
        {
            animator.SetBool("isEquipping", true);  // Activar la animaci�n
        }
    }

    void Unequip()
    {
        if (flashlightInstance != null)
        {
            Destroy(flashlightInstance);
        }

        if (animator != null)
        {
            animator.SetBool("isEquipping", false);  // Desactivar la animaci�n
        }
    }
}




