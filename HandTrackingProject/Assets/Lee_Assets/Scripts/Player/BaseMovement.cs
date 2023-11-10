using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseMovement : MonoBehaviour
{
    [SerializeField]
    protected GroundCheck groundCheck;


    // Physics variables and references
    protected Rigidbody _rb;
    protected const float GRAVITY = -9.81f;
    [SerializeField]
    protected float _groundCheckDistance = 0.1f; 
    protected Vector3 _downwardForce;

    //Rotation and movement variables
    protected Quaternion _targetRotation;
    protected Vector3 _moveDirection;

    [SerializeField]
    protected float _rotationSpeed = 2f;
    protected float _maxSlopeAngle = 45f;
    
    //protected bool _isGrounded = false;
    protected bool _isRotating = false;

    [SerializeField]
    protected Transform playerCamera;


    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _downwardForce = new Vector3(0, GRAVITY, 0);
        _moveDirection = playerCamera.forward;
        _targetRotation = transform.rotation;
    }
   

    protected virtual void Update()
    {
        /*_rb.MoveRotation(Quaternion.Euler(0f, _targetRotation.eulerAngles.y, 0f));*/
        /* _moveDirection = playerCamera.forward;
         _moveDirection.y = 0f;
         _moveDirection.Normalize();

         Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
         float angleDifference = Quaternion.Angle(this._targetRotation, _targetRotation);

         float someThreshold = 20.0f; // 15 degrees threshold

         if (angleDifference > someThreshold)
         {
             //Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
             this._targetRotation = Quaternion.Slerp(this._targetRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
         }*/
    }

    public abstract void HandleMovement();
    //public abstract void HandleRotation();

    /// <summary>
    /// Raycast is cast from the players position downwards for a distance of _groundCheckDistance
    /// to detect wether the player is grounded or not
    /// This is used to determine if gravity needs to be applied to the player
    /// </summary>
    protected virtual void FixedUpdate()
    {
       /* RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance))
        {
            _isGrounded = true;
            //float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            *//*if (slopeAngle <= _maxSlopeAngle)
            {
                _rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }*//*
        }
        else
        {
            _isGrounded = false;
            //_rb.constraints &= ~RigidbodyConstraints.FreezePosition;// | RigidbodyConstraints.FreezeRotation;
            //_rb.AddForce(Vector3.up *  GRAVITY);
        }*/

        if (!groundCheck.IsGrounded())
        {
            _rb.AddForce(_downwardForce, ForceMode.Acceleration);
        }

        /*_rb.MoveRotation(Quaternion.Euler(0f, _targetRotation.eulerAngles.y, 0f));*/
    }
}
