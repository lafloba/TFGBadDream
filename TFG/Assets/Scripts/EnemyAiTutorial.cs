using UnityEngine;
using UnityEngine.UI;

public class EnemyAiTutorial : MonoBehaviour
{
    public Animator ani;
    public int maxHealth = 100;
    private int currentHealth;
    public Slider barraVidaEnemigo;

    public float moveSpeed = 2f;
    public float chaseSpeed = 4f; // Velocidad de persecución
    public float changeDirectionTime = 3f;
    public float obstacleDetectionRange = 2f;
    public float detectionRange = 0.5f; // Rango de detección del jugador
    public float stopChasingDistance = 0.25f; // Distancia para dejar de perseguir
    public LayerMask obstacleLayer;

    private Vector3 moveDirection;
    private float timeSinceLastDirectionChange;
    private float timeUntilNextDirectionChange = 0f;
    private bool isChasingPlayer = false; // Estado de persecución
    private bool isCollidingWithPlayer = false;

    // Variables para la fase de preparación
    public float preparationDuration = 0.5f; // Duración de la fase de preparación
    private float preparationStartTime;
    private bool isInPreparationPhase = false;

    // Referencia al jugador
    private Transform player;
    private EquipBlanket equipBlanket; // Referencia al script EquipBlanket

    private Rigidbody rb; // Referencia al Rigidbody del enemigo

    public float cantidadDaño; // Daño causado al jugador

    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); // Obtener el Rigidbody

        currentHealth = maxHealth;

        // Encuentra el jugador por su nombre o etiqueta específica (en este caso, "Artie")
        player = GameObject.Find("Artie").transform;

        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Make sure it exists and is named 'Artie'.");
        }
        else
        {
            equipBlanket = player.GetComponent<EquipBlanket>();
            if (equipBlanket == null)
            {
                Debug.LogError("EquipBlanket script not found on player GameObject.");
            }
        }

        ChangeMoveDirection(); // Cambia la dirección inicial del enemigo
        ani.SetBool("walk", false); // Inicia la animación de movimiento
    }

    void Update()
    {
        barraVidaEnemigo.value = currentHealth;

        if (!IsInPreparationPhase() && !isCollidingWithPlayer) // Asegurarse de que el enemigo no se mueva si está colisionando con el jugador
        {
            // Verificar si el jugador está en el rango de detección
            if (CanSeePlayer())
            {
                Debug.Log("Can see player!");
                isChasingPlayer = true;
                moveDirection = (player.position - transform.position).normalized; // Perseguir al jugador
                moveDirection.y = 0; // Ignorar la componente Y
            }
            else if (isChasingPlayer && Vector3.Distance(transform.position, player.position) > stopChasingDistance)
            {
                Debug.Log("Player out of range, stop chasing.");
                isChasingPlayer = false; // Dejar de perseguir al jugador si está demasiado lejos
            }

            // Movimiento y cambio de dirección
            if (isChasingPlayer)
            {
                // Perseguir al jugador con velocidad de persecución
                transform.Translate(moveDirection * chaseSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                // Movimiento aleatorio con velocidad normal
                if (!IsObstacleInFront())
                {
                    transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    ChangeMoveDirection(); // Cambiar la dirección de movimiento al detectar un obstáculo
                }

                timeSinceLastDirectionChange += Time.deltaTime;

                if (timeSinceLastDirectionChange >= changeDirectionTime && timeUntilNextDirectionChange <= 0f)
                {
                    ChangeMoveDirection(); // Cambiar la dirección de movimiento
                }
                else if (timeUntilNextDirectionChange > 0f)
                {
                    timeUntilNextDirectionChange -= Time.deltaTime;
                }
            }

            bool isMoving = moveDirection != Vector3.zero;
            ani.SetBool("walk", isMoving); // Actualizar la animación basada en si el enemigo está moviéndose

            // Rotar al enemigo en la dirección de movimiento
            if (isMoving)
            {
                RotateTowardsMovementDirection();
            }
        }
        else
        {
            ani.SetBool("walk", false); // Detener la animación de movimiento cuando se detiene
        }
    }

    void ChangeMoveDirection()
    {
        Vector3 newDirection = Vector3.zero;
        do
        {
            newDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        } while (Physics.SphereCast(transform.position, 0.5f, newDirection, out _, obstacleDetectionRange, obstacleLayer));

        moveDirection = newDirection;
        timeSinceLastDirectionChange = 0f;
        timeUntilNextDirectionChange = changeDirectionTime;

        // Configurar la fase de preparación
        preparationStartTime = Time.time;
        isInPreparationPhase = true;
    }

    bool IsInPreparationPhase()
    {
        return isInPreparationPhase && Time.time < preparationStartTime + preparationDuration;
    }

    void FixedUpdate()
    {
        if (IsInPreparationPhase())
        {
            float rotationSpeed = 500f; // Velocidad de rotación
            Vector3 correctedDirection = new Vector3(moveDirection.x, 0, moveDirection.z).normalized;
            if (correctedDirection.sqrMagnitude > 0.01f) // Verifica que el vector no sea casi nulo
            {
                Quaternion targetRotation = Quaternion.LookRotation(correctedDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }

            // Comprueba si la fase de preparación ha finalizado
            if (Time.time >= preparationStartTime + preparationDuration)
            {
                isInPreparationPhase = false;
                // Reanudar el movimiento
                moveDirection = correctedDirection.normalized;
            }
        }
    }

    void RotateTowardsMovementDirection()
    {
        // Rotar al enemigo en la dirección de movimiento
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    }

    bool IsObstacleInFront()
    {
        RaycastHit hit;
        // Realiza un SphereCast para detectar obstáculos en frente del enemigo
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, obstacleDetectionRange, obstacleLayer))
        {
            // Si encuentra un obstáculo, devuelve true
            Debug.Log("Obstacle detected: " + hit.collider.gameObject.name);
            return true;
        }
        return false;
    }

    bool CanSeePlayer()
    {
        // Verificar si la manta está equipada
        if (equipBlanket != null && equipBlanket.IsBlanketEquipped())
        {
            Debug.Log("Player is invisible because the blanket is equipped.");
            return false; // El jugador es invisible si la manta está equipada
        }

        // Verificar si el jugador está dentro del rango de detección
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0; // Ignorar la componente Y

            // Raycast para verificar si hay obstáculos entre el enemigo y el jugador
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange, obstacleLayer))
            {
                // Si el rayo golpea un objeto en la capa de obstáculos, no puede ver al jugador
                if (!hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Obstacle between enemy and player: " + hit.collider.gameObject.name);
                    return false; // Hay un obstáculo entre el enemigo y el jugador
                }
            }

            Debug.Log("Player detected!");
            return true; // El jugador está dentro del rango de detección y visible
        }

        return false; // El jugador no está dentro del rango de detección
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Monster hit by flashlight. Remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Monster has died!");
        ani.SetBool("walk", false);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * obstacleDetectionRange);
        Gizmos.DrawRay(transform.position, transform.right * obstacleDetectionRange);
        Gizmos.DrawRay(transform.position, -transform.right * obstacleDetectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Visualizar el rango de detección del jugador
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.collider.GetComponent<CodigoSalud>())
        {
            isCollidingWithPlayer = true;
            ani.SetBool("walk", false); // Detener la animación de caminar
            Debug.Log("Enemy collided with player and stopped.");
            collision.collider.GetComponent<CodigoSalud>().recibirDaño(cantidadDaño);

            if (equipBlanket != null)
            {
                equipBlanket.Unequip(); // Usar la instancia de equipBlanket para llamar a Unequip
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.collider.GetComponent<CodigoSalud>())
        {
            isCollidingWithPlayer = true;
            ani.SetBool("walk", false); // Asegurarse de que la animación de caminar esté detenida
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.collider.GetComponent<CodigoSalud>())
        {
            isCollidingWithPlayer = false;
            Debug.Log("Enemy stopped colliding with player and resumed movement.");
        }
    }
}
