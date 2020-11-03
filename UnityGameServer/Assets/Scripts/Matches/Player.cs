using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public float gravity = -20f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 9f;
    public float jumpHeight = 4f;
    public PlayerDataHolder MyHolder;

    public Transform groundDirection;
    public Transform fallDirection;
    private bool[] inputs;
    private bool jumping;
    private bool jump;
    public float yVelocity = 0;
    public float terminalVelocity = -25;
    private Vector3 velocity;
    private Vector3 jumpDirection;
    private Vector3 forwardDirection;
    private Vector3 collisionPoint;
    private Vector2 inputNormalized;
    [SerializeField]
    private float forwardAngle;
    private float slopeAngle;
    [SerializeField]
    private float slopeMult;
    private float fallMult;
    private Ray groundRay;
    private RaycastHit groundHit;
    public Vector2 InputsVector;
    private void Start()
    {
        moveSpeed *= Time.deltaTime;
        jumpSpeed *= Time.deltaTime;
        inputs = new bool[2];
    }
    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        GetInputs();
        Locomotion();
    }

    private void GetInputs()
    {

        if (inputs[0]) //Forward
        {
            InputsVector.y = 1;
        }
        else
        {
            InputsVector.y = 0;
        }
            

        //Jump
        jump = inputs[1];
    }

    private void Locomotion()
    {
        
        GroundDirection();

        if (controller.isGrounded && slopeAngle <= controller.slopeLimit)
        {
            inputNormalized = InputsVector.normalized;
            
        } else if (!controller.isGrounded || slopeAngle > controller.slopeLimit)
        {
            inputNormalized = Vector2.Lerp(inputNormalized, Vector2.zero, 0.025f);
        }
        if(jump && controller.isGrounded && slopeAngle <= controller.slopeLimit)
            Jump();
        
        if(!controller.isGrounded && yVelocity > terminalVelocity)
            yVelocity += gravity * Time.deltaTime;
        else if (controller.isGrounded && slopeAngle > controller.slopeLimit)
            yVelocity = Mathf.Lerp(yVelocity, terminalVelocity, 0.25f);
        
        if (!jumping)
        {
            velocity = groundDirection.forward * (inputNormalized.magnitude * (moveSpeed * slopeMult)) + fallDirection.up * yVelocity;
        }
        else
            velocity = jumpDirection * jumpSpeed + Vector3.up * yVelocity;

        controller.Move(velocity);
        
            if (controller.isGrounded)
            {
                if (jumping)
                    jumping = false;
                yVelocity = 0;
            }
            // ServerSend.PlayerPosition(MyHolder, Parser.ParseHolderToInt(Dictionaries.CurrentMatches[MyHolder.GetMatchId()].GetAllPlayers()));
            // ServerSend.PlayerRotation(MyHolder, Parser.ParseHolderToInt(Dictionaries.CurrentMatches[MyHolder.GetMatchId()].GetAllPlayers()));
    }

    public void GroundDirection()
    {
        forwardDirection = transform.position;

        if (inputNormalized.magnitude > 0)
            forwardDirection += transform.forward * inputNormalized.y;
        else
            forwardDirection += transform.forward;
        
        groundDirection.LookAt(forwardDirection);
        fallDirection.rotation = transform.rotation;
        groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;
        slopeMult = 1;
        //fallMult = 1;
        if (!Physics.Raycast(groundRay, out groundHit, 0.3f)) return;
        slopeAngle = Vector3.Angle(transform.up, groundHit.normal);
        forwardAngle = Vector3.Angle(groundDirection.forward, groundHit.normal) - 90;
        if (forwardAngle < 0 && slopeAngle <= controller.slopeLimit)
        {
            slopeMult = 1 / Mathf.Cos(forwardAngle * Mathf.Deg2Rad);
            groundDirection.eulerAngles += new Vector3(-forwardAngle, 0, 0);
        } else if(slopeAngle > controller.slopeLimit)
        {
            float groundDistance = Vector3.Distance(groundRay.origin, groundHit.point);
            if (!(groundDistance <= 0.1f)) return;
            fallMult = 1 / Mathf.Cos((90 - slopeAngle) * Mathf.Deg2Rad);
            var groundCross = Vector3.Cross(groundHit.normal, Vector3.up);
            fallDirection.rotation =
                Quaternion.FromToRotation(transform.up, Vector3.Cross(groundCross, groundHit.normal));
        }
    }
    void Jump()
    {
        
        if(!jumping)
        jumping = true;

        jumpDirection = (transform.forward * InputsVector.y).normalized;
        yVelocity = Mathf.Sqrt(-gravity * jumpHeight);
        InputsVector.y = 0;
    }
    
    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

  
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        collisionPoint = hit.point;
        collisionPoint = collisionPoint - transform.position;
    }
}