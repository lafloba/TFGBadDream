using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public Animator animator;
    private Vector3 playerInput;
    private float horizontalMove;
    private float verticalMove;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        //controlar la velocidad en diagonal del personaje
        playerInput = new Vector3(horizontalMove, 0, verticalMove);

        //Animator
        if (playerInput == Vector3.zero)
        {
            //Idle
            animator.SetFloat("Speed", 0);
        }
        else
        {
            //Walk
            animator.SetFloat("Speed", 0.5f);

        }
    }
}
