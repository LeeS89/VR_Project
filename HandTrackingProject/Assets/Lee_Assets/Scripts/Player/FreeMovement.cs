using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FreeMovement : BaseMovement
{
    //public Animator animator;
    public float _movementSpeed = 5f;
    //public float _rotationSpeed = 5f;
    //public float _maxSlopeAngle = 45f;
    //public float _groundVheckDistance = 0.1f;

    //private Rigidbody _rb;
    //public Transform playerCamera;
    //private Quaternion _targetRotation;
    //private const float GRAVITY = -9.81f;
    //private Vector3 _downwardForce;
    //private bool _isGrounded = false;
    //private bool _isTeleporting = false;
    private bool _move = false;
    //private bool _rotate = false;

   /* private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _targetRotation = transform.rotation;
        downwardForce = new Vector3(0, GRAVITY, 0);
    }*/

  /*  public enum MovementMode
    {
        FreeMovement,
        Teleporting
    }*/

    //public MovementMode currentMode = MovementMode.FreeMovement;



    /// <summary>
    /// Move is set to true by a tbd hand gesture
    /// When move is true, the move direction is set to where the player is looking along the z axis
    /// And when the player looks left or right (moveDirection != Vector3.zero), this sets the rotation direction, and gradually (Slerp) rotates
    /// to that direction while keeping moveDirection.y to 0 to prevent vertical rotations
    /// </summary>
    protected override void Update()
    {
        base.Update();
       
        //SetMoveDirectionAndRotation();
        /*if (_move)
        {

            Vector3 moveDirection = playerCamera.forward;
            moveDirection.y = 0f;
            moveDirection.Normalize();

            if (moveDirection != Vector3.zero)
            {
                Quaternion _targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                targetRotation = Quaternion.Slerp(targetRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }*/
    }

    protected override void FixedUpdate()
    {
        
        //if (_move)
        //{
        //HandleMovement();
        /*Vector3 _horizontalMovement = transform.forward * _movementSpeed * Time.fixedDeltaTime;
        _rb.velocity = new Vector3(_horizontalMovement.x, _rb.velocity.y, _horizontalMovement.z);*/

        //_rb.velocity = transform.forward * _movementSpeed * Time.fixedDeltaTime;

        //_rb.MoveRotation(Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f));
        //}

        /* if (!_isGrounded)
         {
             ApplyGravity();
             //_rb.AddForce(downwardForce, ForceMode.Acceleration);
         }*/
        base.FixedUpdate();

        if (!_move && groundCheck.IsGrounded())
        {
            StopMoving();
            //_rb.velocity = Vector3.zero;
        }

        
        //GroundCheck();
        /* RaycastHit hit;
         if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundVheckDistance))
         {
             _isGrounded = true;
             float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
             if (slopeAngle <= _maxSlopeAngle)
             {
                 _rb.constraints = RigidbodyConstraints.FreezeRotation;
             }
             else
             {
                 _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
             }
         }
         else
         {
             _isGrounded = false;
             _rb.constraints &= ~RigidbodyConstraints.FreezePosition;// | RigidbodyConstraints.FreezeRotation;
             //_rb.AddForce(Vector3.up *  GRAVITY);
         }*/


        /*_rb.MoveRotation(Quaternion.Euler(0f, _targetRotation.eulerAngles.y, 0f));*/

    }

    /*private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundVheckDistance))
        {
            _isGrounded = true;
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle <= _maxSlopeAngle)
            {
                _rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
        }
        else
        {
            _isGrounded = false;
            _rb.constraints &= ~RigidbodyConstraints.FreezePosition;// | RigidbodyConstraints.FreezeRotation;
            //_rb.AddForce(Vector3.up *  GRAVITY);
        }
    }*/

    private void StopMoving()
    {
        _rb.velocity = Vector3.zero;
    }

    /*public void SetMoveDirectionAndRotation()
    {
        if (_move)
        {

            _moveDirection = playerCamera.forward;
            _moveDirection.y = 0f;
            _moveDirection.Normalize();

            if (_moveDirection != Vector3.zero)
            {
                Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
                this._targetRotation = Quaternion.Slerp(this._targetRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            return;
        }
    }*/

    public override void HandleMovement()
    {
        if (_move)
        {
            /*Vector3 _horizontalMovement = transform.forward * _movementSpeed * Time.fixedDeltaTime;
            _rb.velocity = new Vector3(_horizontalMovement.x, _rb.velocity.y, _horizontalMovement.z);*/
            Vector3 desiredMovement = transform.forward * _movementSpeed;
            Vector3 force = (desiredMovement - _rb.velocity);
            force.y = 0;  // Ignore vertical force, we're handling that with gravity
            _rb.AddForce(force, ForceMode.VelocityChange);
        }
        else
        {
            return;
        }

        //_rb.velocity = transform.forward * _movementSpeed * Time.fixedDeltaTime;

        //_rb.MoveRotation(Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f));
    }

    /*/// <summary>
    /// Because there will be no physical controllers, rotation will be applied when the user
    /// turns their head left or right passed a certain threshold
    /// The threshold is there to avoid unwanted rotating when just taking a quick glance left or right and also to 
    /// minimize the potential for motion sickness
    /// 
    /// The stop rotating threshold is less than the start rotating threshold to avoid rapid changes between
    /// rotating and not rotating states, if the users gaze is hovering around the threshold.
    /// 
    /// The angle difference is the difference in angle (Discounting the y axis to prevent rotating up or down) between the users gaze and the current transform.rotation
    /// 
    /// --REMINDER--
    /// User will eventually be able to change the rotation speed to suit their own comfort level
    /// </summary>
    public override void HandleRotation()
    {
        

        _moveDirection = playerCamera.forward;
        _moveDirection.y = 0f;
        _moveDirection.Normalize();

        Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
        float angleDifference = Quaternion.Angle(this._targetRotation, _targetRotation);

        float rotateThreshold = 20.0f;
        float stopRotatingThreshold = 18.0f;

        if(!_isRotating && angleDifference > rotateThreshold)
        {
            _isRotating = true;
        }else if(_isRotating && angleDifference < stopRotatingThreshold)
        {
            _isRotating = false;
        }

        if (_isRotating)
        {
            //Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            this._targetRotation = Quaternion.Slerp(this._targetRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }*/

    private void ApplyGravity()
    {
        _rb.AddForce(_downwardForce, ForceMode.Acceleration);
    }


    public void SetMove(bool move)
    {
        //_rotate= move;
        _move = move;
    }

    /* private void OnTriggerEnter(Collider other)
     {
         if (other.CompareTag("Door"))
         {
             animator.SetBool("character_nearby", true);
         }
     }*/
}
