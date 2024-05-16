using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    // Declaraci�n de variables p�blicas para ser ajustadas en el Inspector de Unity
    public int rutina; // Variable que almacena la rutina actual del enemigo
    public float cronometro; // Variable que lleva el tiempo transcurrido
    public Animator ani; // Referencia al componente Animator del objeto
    public Quaternion angulo; // Almacena el �ngulo de rotaci�n del enemigo
    public float grado; // Almacena el �ngulo en grados



    // M�todo Start se llama antes de que se actualice el primer frame
    void Start()
    {
        // Se asigna el componente Animator al objeto
        ani = GetComponent<Animator>();
    }

    // M�todo Update se llama una vez por frame
    void Update()
    {
        // Llama al m�todo que controla el comportamiento del enemigo
        Comportamiento_Enemigo();
    }

    // M�todo que define el comportamiento del enemigo
    public void Comportamiento_Enemigo()
    {
        // Incrementa el cron�metro en funci�n del tiempo transcurrido
        cronometro += 1 * Time.deltaTime;

        // Si el cron�metro alcanza un cierto valor, cambia la rutina
        if (cronometro >= 4)
        {
            rutina = Random.Range(0, 2); // Selecciona una rutina aleatoria entre 0 y 1
            cronometro = 0; // Reinicia el cron�metro
        }

        // Dependiendo de la rutina actual, se ejecuta un comportamiento
        switch (rutina)
        {
            case 0: // Rutina 0: El enemigo se detiene
                ani.SetBool("walk", false);
                break;

            case 1: // Rutina 1: El enemigo rota en un �ngulo aleatorio
                grado = Random.Range(0, 360); // Selecciona un �ngulo aleatorio entre 0 y 360 grados
                angulo = Quaternion.Euler(0, grado, 0); // Convierte el �ngulo a un formato Quaternion
                rutina++; // Incrementa la rutina para que en la siguiente actualizaci�n ejecute el siguiente comportamiento
                break;

            case 2: // Rutina 2: El enemigo se mueve hacia adelante en la direcci�n del �ngulo aleatorio
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f); // Rota gradualmente hacia el �ngulo aleatorio
                transform.Translate(Vector3.forward * 1 * Time.deltaTime); // Se mueve hacia adelante
                ani.SetBool("walk", true); // Activa la animaci�n de caminar
                break;
        }
    }
}
