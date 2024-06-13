using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public Animator ani;

    private int maxHealth = 100;
    private int currentHealth;

    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;

    void Start()
    {
        ani = GetComponent<Animator>();
        currentHealth = maxHealth;

        targetPoint = 0;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);
        // Calcular la dirección hacia el siguiente punto de patrulla
        Vector3 directionToTarget = patrolPoints[targetPoint].position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        // Rotar el personaje para que siempre mire hacia adelante
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, speed * Time.deltaTime);

        // Controlar la animación de caminar basada en la velocidad del personaje
        if (speed > 0.1f)
        {
            ani.SetBool("walk", true); // Asume que "IsWalking" es el parámetro booleano para la animación de caminar
        }
        else
        {
            ani.SetBool("walk", false);
        }
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Monster hit by flashlight Remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Monster has died!");
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint")) // Asegúrate de que los waypoints tengan el tag "Waypoint"
        {
            IncreaseTargetInt();
        }
    }

    void IncreaseTargetInt()
    {
        targetPoint++;
        Debug.Log("IncreaseTargetInt ");
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }
}
