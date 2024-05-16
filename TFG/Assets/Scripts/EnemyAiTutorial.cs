using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutoriaal : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public float grado;
    public float distanciaMovimiento = 5f;
    public float velocidadMovimiento = 1.5f;
    public LayerMask groundLayer; // Capa del terreno navegable

    private NavMeshAgent agente;
    private Vector3 destino;
    private bool moviendose;

    void Start()
    {
        ani = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
        agente.speed = velocidadMovimiento;
        agente.angularSpeed = 120f;
        agente.acceleration = 8f;
    }

    void Update()
    {
        Comportamiento_Enemigo();
        ani.SetBool("walk", agente.velocity.magnitude > 0.1f);
    }

    public void Comportamiento_Enemigo()
    {
        cronometro += 1 * Time.deltaTime;

        if (cronometro >= 4)
        {
            rutina = Random.Range(0, 3);
            cronometro = 0;
            moviendose = false;
        }

        switch (rutina)
        {
            case 0:
                agente.isStopped = true;
                break;

            case 1:
                if (!moviendose)
                {
                    grado = Random.Range(0, 360);
                    transform.rotation = Quaternion.Euler(0, grado, 0);
                    rutina++;
                }
                break;

            case 2:
                if (!moviendose)
                {
                    Vector3 randomDirection = Random.insideUnitSphere * distanciaMovimiento;
                    randomDirection += transform.position;

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomDirection, out hit, distanciaMovimiento, NavMesh.AllAreas))
                    {
                        destino = hit.position;
                        agente.SetDestination(destino);
                        agente.isStopped = false;
                        moviendose = true;
                    }
                }
                break;
        }
    }
}

