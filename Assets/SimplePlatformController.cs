using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlatformController : MonoBehaviour
{

    [Header("-Common-")]
    public bool canMove = true;
    public float speed = 6;
    Vector3 moveDirection = Vector3.zero;
    public float jumpSpeed = 8;
    public float gravity = 20;
    [Header("-JumpStuff-")]
    int canJump = 0;
    public int maxJumps = 2;

    CharacterController controller;
    [Header("-Push Power!-")]
    public float pushPower = 2.0f;
    //float antiBump = .75f;
    // Use this for initialization

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputForController();
        JumpSystem();
        ApplyGravity();
        ApplyMovement();
    }

    void GetInputForController()
    {
        //Grab Raw input plus Speed;
        if (controller.isGrounded)
        {
            moveDirection.x = Input.GetAxis("Horizontal") * speed;
        }
        else
        {
            //if In air change to make harder or easier to control
            moveDirection.x = Input.GetAxis("Horizontal") * speed;
        }
        //Not needed for Platforming!
        //moveDirection.y = Input.GetAxis("Vertical") * speed;
    }

    void JumpSystem()
    {
        if (canJump < maxJumps)
        {
            if (Input.GetButtonDown("Jump"))
            {
                canJump++;
                moveDirection.y = jumpSpeed;
            }
        }
        if (controller.isGrounded)
        {
            canJump = 0;
        }
    }

    void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            //Apply Gravity
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {

        }

    }

    void ApplyMovement()
    {
        //Apply the Movement use Time.delta time to sync to the clock
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Pass the hit along for Pushing things
        PlayerPushThings(hit);
    }

    void PlayerPushThings(ControllerColliderHit hit)
    {
        //Grab RigidBody
        Rigidbody body = hit.collider.attachedRigidbody;
        //Return of can't move Body!
        if (body == null || body.isKinematic)
            return;
        //Return if wrong y Angle;
        if (hit.moveDirection.y < -0.3)
            return;
        //Grab Push Direction
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        //ApplyPushing to the Object
        body.velocity = pushDir * pushPower;
    }
}
