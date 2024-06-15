using UnityEngine;

public class EquipFlashlight : MonoBehaviour
{
    public GameObject flashlightPrefab;
    private GameObject flashlightInstance;
    public Transform flashlightHolder;
    public Vector3 flashlightScale = new Vector3(1, 1, 1);
    public Vector3 flashlightRotation = new Vector3(90, 0, 0); // Ajustar la rotaci�n de la linterna
    public Animator animator; // A�adir una referencia al Animator
    public float attackCooldownFina = 2.0f; // Tiempo de enfriamiento entre ataques para pila fina
    public float attackCooldownAncha = 1.5f; // Tiempo de enfriamiento entre ataques para pila ancha
    public float detectionRadius = 0.5f; // Radio de detecci�n del SphereCast
    public float maxDetectionDistance = 10f; // Distancia m�xima de detecci�n

    private bool isEquipped = false;
    private GetObject getObjectScript;
    private float equipDuration = 0f;
    private float equipStartTime = 0f;
    private Light flashlightLight;
    private float lastAttackTime = -9999f; // Tiempo del �ltimo ataque inicializado en un valor muy bajo
    private int flashlightDamage = 20; // Da�o de la linterna (se ajustar� seg�n la pila)
    private float attackCooldown = 0.9f; // Tiempo de enfriamiento actual (se ajustar� seg�n la pila)

    // Tipos de pilas
    private enum BatteryType { None, PilaFina, PilaAncha }
    private BatteryType currentBatteryType = BatteryType.None;

    void Start()
    {
        // Obtener la referencia al script GetObject
        getObjectScript = FindObjectOfType<GetObject>();

        // Verificar si se obtuvo la referencia correctamente
        if (getObjectScript == null)
        {
            Debug.LogError("No se encontr� el script GetObject en la escena.");
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
                Equip(10f, BatteryType.PilaFina);
                getObjectScript.contadorPilaFina--;
            }
            else if (getObjectScript.contadorPilaAncha > 0)
            {
                Equip(5f, BatteryType.PilaAncha);
                getObjectScript.contadorPilaAncha--;
            }
            else
            {
                Debug.Log("No hay pila disponible para equipar la linterna.");
            }
        }

        // Comprobar si la linterna est� equipada y si la duraci�n ha pasado
        if (isEquipped && Time.time - equipStartTime >= equipDuration)
        {
            Unequip();
        }

        // Comprobar si la linterna est� equipada y usarla para atacar al monstruo
        if (isEquipped)
        {
            AttackWithFlashlight();
        }
    }

    void Equip(float duration, BatteryType batteryType)
    {
        equipDuration = duration;
        equipStartTime = Time.time;
        currentBatteryType = batteryType;

        // Ajustar el da�o y el tiempo de enfriamiento seg�n el tipo de pila
        if (batteryType == BatteryType.PilaFina)
        {
            flashlightDamage = 20;
            attackCooldown = attackCooldownFina;
        }
        else if (batteryType == BatteryType.PilaAncha)
        {
            flashlightDamage = 40;
            attackCooldown = attackCooldownAncha;
        }

        if (flashlightInstance == null)
        {
            flashlightInstance = Instantiate(flashlightPrefab, flashlightHolder);
            flashlightInstance.transform.localPosition = Vector3.zero;
            flashlightInstance.transform.localRotation = Quaternion.Euler(flashlightRotation); // Ajustar la rotaci�n de la linterna
            flashlightInstance.transform.localScale = flashlightScale;
            flashlightLight = flashlightInstance.GetComponentInChildren<Light>();
        }

        if (animator != null)
        {
            animator.SetBool("isEquipping", true); // Activar la animaci�n
        }

        isEquipped = true;
        Debug.Log("Linterna equipada con " + batteryType + " por " + duration + " segundos.");
    }

    void Unequip()
    {
        if (flashlightInstance != null)
        {
            Destroy(flashlightInstance);
            flashlightInstance = null;
            flashlightLight = null;
        }

        if (animator != null)
        {
            animator.SetBool("isEquipping", false); // Desactivar la animaci�n
        }

        isEquipped = false;
        currentBatteryType = BatteryType.None;
        Debug.Log("Linterna desequipada.");
    }

    void AttackWithFlashlight()
    {
        // Comprobar si ha pasado suficiente tiempo desde el �ltimo ataque
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Intentando atacar con la linterna.");
            if (flashlightLight != null)
            {
                Ray ray = new Ray(flashlightLight.transform.position, flashlightLight.transform.forward);
                RaycastHit hit;

                // Usar SphereCast para detectar enemigos en un �rea m�s amplia
                if (Physics.SphereCast(ray, detectionRadius, out hit, maxDetectionDistance))
                {
                    Debug.DrawRay(ray.origin, ray.direction * maxDetectionDistance, Color.red); // Depuraci�n del rayo

                    if (hit.collider.CompareTag("Monster"))
                    {
                        Debug.Log("Monstruo detectado."); // Depuraci�n de la detecci�n del monstruo

                        // A�adir l�gica para da�ar al monstruo
                        EnemyAiTutorial monster = hit.collider.GetComponent<EnemyAiTutorial>();
                        if (monster != null)
                        {
                            // Comprobar la distancia entre la linterna y el monstruo
                            float distanceToMonster = Vector3.Distance(flashlightLight.transform.position, monster.transform.position);
                            if (distanceToMonster <= maxDetectionDistance)
                            {
                                monster.TakeDamage(flashlightDamage);
                                lastAttackTime = Time.time; // Actualizar el tiempo del �ltimo ataque
                                Debug.Log("Atac� al monstruo con " + flashlightDamage + " de da�o. Pr�ximo ataque disponible en " + attackCooldown + " segundos.");
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("No se detect� ning�n objeto en el rango de la linterna."); // Depuraci�n de la falta de detecci�n
                }
            }
        }
    }
}
