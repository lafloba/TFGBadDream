using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    // Declaración de variables públicas para ser ajustadas en el Inspector de Unity
    public int rutina; // Variable que almacena la rutina actual del enemigo
    public float cronometro; // Variable que lleva el tiempo transcurrido
    public Animator ani; // Referencia al componente Animator del objeto
    public Quaternion angulo; // Almacena el ángulo de rotación del enemigo
    public float grado; // Almacena el ángulo en grados



    // Método Start se llama antes de que se actualice el primer frame
    void Start()
    {
        // Se asigna el componente Animator al objeto
        ani = GetComponent<Animator>();
    }

    // Método Update se llama una vez por frame
    void Update()
    {
        // Llama al método que controla el comportamiento del enemigo
        Comportamiento_Enemigo();
    }

    // Método que define el comportamiento del enemigo
    public void Comportamiento_Enemigo()
    {
        // Incrementa el cronómetro en función del tiempo transcurrido
        cronometro += 1 * Time.deltaTime;

        // Si el cronómetro alcanza un cierto valor, cambia la rutina
        if (cronometro >= 4)
        {
            rutina = Random.Range(0, 2); // Selecciona una rutina aleatoria entre 0 y 1
            cronometro = 0; // Reinicia el cronómetro
        }

        // Dependiendo de la rutina actual, se ejecuta un comportamiento
        switch (rutina)
        {
            case 0: // Rutina 0: El enemigo se detiene
                ani.SetBool("walk", false);
                break;

            case 1: // Rutina 1: El enemigo rota en un ángulo aleatorio
                grado = Random.Range(0, 360); // Selecciona un ángulo aleatorio entre 0 y 360 grados
                angulo = Quaternion.Euler(0, grado, 0); // Convierte el ángulo a un formato Quaternion
                rutina++; // Incrementa la rutina para que en la siguiente actualización ejecute el siguiente comportamiento
                break;

            case 2: // Rutina 2: El enemigo se mueve hacia adelante en la dirección del ángulo aleatorio
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f); // Rota gradualmente hacia el ángulo aleatorio
                transform.Translate(Vector3.forward * 1 * Time.deltaTime); // Se mueve hacia adelante
                ani.SetBool("walk", true); // Activa la animación de caminar
                break;
        }
    }
}
