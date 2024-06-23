using UnityEngine;
using TMPro; // Necesario para acceder a TextMeshProUGUI

public class EquipFlashlight : MonoBehaviour
{
    public GameObject flashlightPrefab;
    private GameObject flashlightInstance;
    public Transform flashlightHolder;
    public Vector3 flashlightScale = new Vector3(1, 1, 1);
    public Vector3 flashlightRotation = new Vector3(90, 0, 0); // Ajustar la rotación de la linterna
    public Animator animator; // Añadir una referencia al Animator
    public float attackCooldownFina = 2.0f; // Tiempo de enfriamiento entre ataques para pila fina
    public float attackCooldownAncha = 0.5f; // Tiempo de enfriamiento entre ataques para pila ancha
    public float detectionRadius = 0.5f; // Radio de detección del SphereCast
    public float maxDetectionDistance = 10f; // Distancia máxima de detección
    public float pilaAnchaIntensity = 5.0f; // Intensidad de la linterna para pila ancha

    public TMP_Text remainingTimeText; // Referencia al componente TextMeshProUGUI para mostrar los segundos restantes

    private bool isEquipped = false;
    private GetObject getObjectScript;
    private float equipDuration = 0f;
    private float remainingEquipTime = 0f;
    private float equipStartTime = 0f;
    private Light flashlightLight;
    private float lastAttackTime = -9999f; // Tiempo del último ataque inicializado en un valor muy bajo
    private int flashlightDamage = 20; // Daño de la linterna (se ajustará según la pila)
    private float attackCooldown = 0.9f; // Tiempo de enfriamiento actual (se ajustará según la pila)
    private float defaultIntensity = 1.0f; // Intensidad predeterminada de la linterna

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
            if (isEquipped)
            {
                remainingEquipTime = equipDuration - (Time.time - equipStartTime);
                Unequip();
            }
            else
            {
                if (remainingEquipTime > 0f)
                {
                    Equip(remainingEquipTime, currentBatteryType);
                }
                else if (getObjectScript.contadorPilaFina > 0)
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
        }

        // Actualizar el tiempo restante solo si la linterna está equipada y la duración es mayor a cero
        if (isEquipped && equipDuration > 0f)
        {
            remainingEquipTime = equipDuration - (Time.time - equipStartTime);

            // Asegurarse de que remainingEquipTime no sea negativo
            if (remainingEquipTime < 0f)
            {
                remainingEquipTime = 0f;
            }

            // Actualizar el texto con los segundos restantes
            if (remainingTimeText != null)
            {
                remainingTimeText.text = "Tiempo restante: " + Mathf.CeilToInt(remainingEquipTime).ToString() + "s";
            }
        }

        // Comprobar si la linterna está equipada y si la duración ha pasado
        if (isEquipped && Time.time - equipStartTime >= equipDuration)
        {
            Unequip();
            remainingEquipTime = 0f;
        }

        // Comprobar si la linterna está equipada y usarla para atacar al monstruo
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

        // Ajustar el daño y el tiempo de enfriamiento según el tipo de pila
        if (batteryType == BatteryType.PilaFina)
        {
            flashlightDamage = 20;
            attackCooldown = attackCooldownFina;
            if (flashlightLight != null)
            {
                flashlightLight.intensity = defaultIntensity;
            }
        }
        else if (batteryType == BatteryType.PilaAncha)
        {
            flashlightDamage = 40;
            attackCooldown = attackCooldownAncha;
            if (flashlightLight != null)
            {
                flashlightLight.intensity = pilaAnchaIntensity;
            }
        }

        if (flashlightInstance == null)
        {
            flashlightInstance = Instantiate(flashlightPrefab, flashlightHolder);
            flashlightInstance.transform.localPosition = Vector3.zero;
            flashlightInstance.transform.localRotation = Quaternion.Euler(flashlightRotation); // Ajustar la rotación de la linterna
            flashlightInstance.transform.localScale = flashlightScale;
            flashlightLight = flashlightInstance.GetComponentInChildren<Light>();

            if (currentBatteryType == BatteryType.PilaAncha)
            {
                flashlightLight.intensity = pilaAnchaIntensity;
            }
            else
            {
                flashlightLight.intensity = defaultIntensity;
            }
        }

        if (animator != null)
        {
            animator.SetBool("isEquipping", true); // Activar la animación
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
            animator.SetBool("isEquipping", false); // Desactivar la animación
        }

        isEquipped = false;
        currentBatteryType = BatteryType.None;
        Debug.Log("Linterna desequipada.");
    }

    void AttackWithFlashlight()
    {
        // Comprobar si ha pasado suficiente tiempo desde el último ataque
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Intentando atacar con la linterna.");
            if (flashlightLight != null)
            {
                Ray ray = new Ray(flashlightLight.transform.position, flashlightLight.transform.forward);
                RaycastHit hit;

                // Usar Raycast para detectar enemigos directamente delante de la linterna
                if (Physics.Raycast(ray, out hit, maxDetectionDistance))
                {
                    Debug.DrawRay(ray.origin, ray.direction * maxDetectionDistance, Color.green); // Depuración del rayo

                    if (hit.collider.CompareTag("Monster"))
                    {
                        HandleMonsterHit(hit.collider);
                        return; // Salir si el monstruo es detectado
                    }
                }

                // Usar SphereCast para detectar enemigos en un área más amplia
                if (Physics.SphereCast(ray, detectionRadius, out hit, maxDetectionDistance))
                {
                    Debug.DrawRay(ray.origin, ray.direction * maxDetectionDistance, Color.red); // Depuración del rayo

                    if (hit.collider.CompareTag("Monster"))
                    {
                        HandleMonsterHit(hit.collider);
                        return; // Salir si el monstruo es detectado
                    }
                }

                Debug.Log("No se detectó ningún objeto en el rango de la linterna."); // Depuración de la falta de detección
            }
        }
    }

    void HandleMonsterHit(Collider monsterCollider)
    {
        Debug.Log("Monstruo detectado."); // Depuración de la detección del monstruo

        // Añadir lógica para dañar al monstruo
        EnemyAiTutorial monster = monsterCollider.GetComponent<EnemyAiTutorial>();
        if (monster != null)
        {
            // Comprobar la distancia entre la linterna y el monstruo
            float distanceToMonster = Vector3.Distance(flashlightLight.transform.position, monster.transform.position);
            if (distanceToMonster <= maxDetectionDistance)
            {
                monster.TakeDamage(flashlightDamage);
                lastAttackTime = Time.time; // Actualizar el tiempo del último ataque
                Debug.Log("Atacó al monstruo con " + flashlightDamage + " de daño. Próximo ataque disponible en " + attackCooldown + " segundos.");
            }
        }
    }
}
