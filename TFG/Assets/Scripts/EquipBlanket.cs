using System;
using UnityEngine;

public class EquipBlanket : MonoBehaviour
{
    public GameObject blanketObject; // Referencia al objeto manta
    public Transform blanketAttachPoint; // Punto de anclaje donde se fijará la manta cuando esté equipada
    public Vector3 blanketOffset; // Offset para ajustar la posición de la manta
    public Vector3 blanketRotation; // Rotación adicional para la manta

    private bool isBlanketEquipped = false; // Estado de la manta
    private float equipTime = 0f; // Tiempo desde que la manta fue equipada
    private float cooldownTime = 0f; // Tiempo desde que la manta fue desequipada
    private const float maxEquipDuration = 10f; // Duración máxima de la manta equipada
    private const float cooldownDuration = 15f; // Duración del tiempo de enfriamiento
    private string message = ""; // Mensaje a mostrar en pantalla
    private const float messageDisplayTime = 1f; // Tiempo que se muestra el mensaje
    private float messageTimer = 0f; // Temporizador para el mensaje

    public Font customFont; // Referencia a la fuente personalizada
    private bool mostrandoMensaje = false;

    void Update()
    {
        // Verificar si se presiona la tecla "G"
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (cooldownTime > 0f)
            {
                message = "La manta no esta disponible. Tiempo restante de enfriamiento: " + Mathf.Ceil(cooldownTime) + " segundos.";
                messageTimer = messageDisplayTime;
                mostrandoMensaje = true;
            }
            else
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

        // Si la manta está equipada, actualizar el tiempo y verificar si debe desequiparse
        if (isBlanketEquipped)
        {
            equipTime += Time.deltaTime;
            if (equipTime >= maxEquipDuration)
            {
                Unequip();
            }
        }
        else if (cooldownTime > 0f)
        {
            // Si la manta no está equipada, actualizar el tiempo de enfriamiento
            cooldownTime -= Time.deltaTime;
        }

        // Actualizar el temporizador del mensaje
        if (mostrandoMensaje)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
            {
                mostrandoMensaje = false;
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

        // Reiniciar el tiempo de equipamiento
        equipTime = 0f;
    }

    void Unequip()
    {
        // Desactivar el objeto manta
        blanketObject.SetActive(false);
        isBlanketEquipped = false;
        Debug.Log("Manta desequipada.");

        // Iniciar el tiempo de enfriamiento
        cooldownTime = cooldownDuration;
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
            float height = style.CalcHeight(new GUIContent(message), width);
            Rect rect = new Rect(Screen.width / 2 - width / 2, 50, width, height);
            GUI.Label(rect, message, style);
        }
    }

    // Método público para verificar si la manta está equipada
    public bool IsBlanketEquipped()
    {
        return isBlanketEquipped;
    }
}
