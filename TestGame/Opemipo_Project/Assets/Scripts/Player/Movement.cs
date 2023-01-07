using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("External Assets:")]
    public CharacterController controller;
    public Animator anim;
    public Transform cam;
    //public GameObject glideTrail;

    [Header("Math:")]
    public float moveSpeed;
    public float xPos;
    public float zPos;
    public float jumpSpeed;

    [Header("Physics:")]
    public float smthTime = 0.1f;
    float smthVelocity;
    public float gravity = 10.0f;
    public bool canGlide;
    public bool isGlide;

    [Header("Transform:")]
    Vector3 jumpDir = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        

        xPos = Input.GetAxis("Horizontal");
        zPos = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(xPos, 0f, zPos).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // uses trig and stuff to find the angle theta between x/z axis and vector (x, z)
                                                                                       // vector (x, z) is the direction the player is facing; it will give an angle in radians.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smthVelocity, smthTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        if (controller.isGrounded)
        {
            canGlide = false;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                jumpDir.y = jumpSpeed;
                controller.Move(jumpDir * Time.deltaTime);
            }
            // anim.SetTrigger("IsJumping");
        }

        if (controller.velocity.y < 0 && controller.isGrounded == false)
        {
            Debug.Log(controller.velocity);
            canGlide = true;
            //isGlide = true;
            //anim.setbool("isgliding", false);
        }
        //if (controller.isGrounded)
        //{
        //    canGlide = false;
        //    //isGlide = false;
        //}

        if (canGlide && Input.GetKey(KeyCode.LeftShift))
        {
            isGlide = true;
            gravity = 10f;
        }
        else
        {
            isGlide = false;
            gravity = 60f;
        }

        jumpDir.y -= gravity * Time.deltaTime;
        controller.Move(jumpDir * Time.deltaTime);

        //transform.Rotate(0, Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0);
        //Vector3 vel = transform.up * gravity;
        //controller.SimpleMove(vel);

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
