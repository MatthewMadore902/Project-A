using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public bool canDoubleJump = false;
    public int numJumps = 2;
    public Transform groundCheck;
    public float gourndDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;


    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, gourndDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (isGrounded == true)
        {
            Jump();
            canDoubleJump = false;
        }
        if (isGrounded == false && numJumps > 0)
        {
            canDoubleJump = true;
        }
        if (isGrounded == true && numJumps == 0)
        {
            numJumps = 2;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (canDoubleJump == true && numJumps >= 0)
        {
            Jump();
        }
        if (numJumps <= 0)
        {
            canDoubleJump = false;
            numJumps = 0;
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            numJumps = numJumps - 1;
        }
    }
}
