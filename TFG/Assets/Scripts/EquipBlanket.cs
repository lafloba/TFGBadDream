using UnityEngine;

public class EquipBlanket : MonoBehaviour
{
    public GameObject blanketObject; // Referencia al objeto manta
    public Transform blanketAttachPoint; // Punto de anclaje donde se fijará la manta cuando esté equipada
    public Vector3 blanketOffset; // Offset para ajustar la posición de la manta
    public Vector3 blanketRotation; // Rotación adicional para la manta

    private bool isBlanketEquipped = false; // Estado de la manta

    void Update()
    {
        // Verificar si se presiona la tecla "G"
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isBlanketEquipped)
            {
                Unequip();
            }
            else
            {
                Equip();
            }
        }
    }

    void Equip()
    {
        // Activar el objeto manta
        blanketObject.SetActive(true);
        isBlanketEquipped = true;

        // Fijar la posición de la manta al punto de anclaje con el offset
        blanketObject.transform.position = blanketAttachPoint.position + blanketOffset;
        // Aplicar rotación adicional a la manta
        blanketObject.transform.rotation = blanketAttachPoint.rotation * Quaternion.Euler(blanketRotation);

        Debug.Log("Manta equipada.");
    }

    void Unequip()
    {
        // Desactivar el objeto manta
        blanketObject.SetActive(false);
        isBlanketEquipped = false;
        Debug.Log("Manta desequipada.");
    }
}
