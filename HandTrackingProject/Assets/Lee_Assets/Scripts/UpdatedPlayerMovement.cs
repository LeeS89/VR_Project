using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UpdatedPlayerMovement : MonoBehaviour
{
    //public Animator animator;
    public float _movementSpeed = 5f;
    public float _rotationSpeed = 5f;
    public float _maxSlopeAngle = 45f;
    public float _groundVheckDistance = 0.1f;

    private Rigidbody _rb;
    public Transform playerCamera;
    private Quaternion targetRotation;
    private const float GRAVITY = -9.81f;
    private Vector3 downwardForce;
    private bool _isGrounded = false;
    //private bool _isTeleporting = false;
    private bool _move = false;
    //private bool _rotate = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        targetRotation= transform.rotation;
        downwardForce = new Vector3(0, GRAVITY, 0);
    }

    public enum MovementMode
    {
        FreeMovement,
        Teleporting
    }
    
    public MovementMode currentMode = MovementMode.FreeMovement;
    


    /// <summary>
    /// Move is set to true by a tbd hand gesture
    /// When move is true, the move direction is set to where the player is looking along the z axis
    /// And when the player looks left or right (moveDirection != Vector3.zero), this sets the rotation direction, and gradually (Slerp) rotates
    /// to that direction while keeping moveDirection.y to 0 to prevent vertical rotations
    /// </summary>
    private void Update()
    {
        /*switch (currentMode)
        {
            case MovementMode.FreeMovement:
                SetMoveDirectionAndRotation();
                //HandleFreeMovement();
                break;

            case MovementMode.Teleporting:
                //HandleTeleportationUpdate();
                break;
        }*/
        SetMoveDirectionAndRotation();
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

    private void FixedUpdate()
    {
        switch (currentMode)
        {
            case MovementMode.FreeMovement:
                HandleFreeMovement();
                break;

            case MovementMode.Teleporting:
                //HandleTeleportationUpdate();
                break;
        }
        //if (_move)
        //{
            HandleFreeMovement();
            /*Vector3 _horizontalMovement = transform.forward * _movementSpeed * Time.fixedDeltaTime;
            _rb.velocity = new Vector3(_horizontalMovement.x, _rb.velocity.y, _horizontalMovement.z);*/

            //_rb.velocity = transform.forward * _movementSpeed * Time.fixedDeltaTime;

            //_rb.MoveRotation(Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f));
        //}

        if (!_isGrounded)
        {
            ApplyGravity();
            //_rb.AddForce(downwardForce, ForceMode.Acceleration);
        }


        if (!_move && _isGrounded)
        {
            StopMoving();
            //_rb.velocity = Vector3.zero;
        }


        GroundCheck();
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


        _rb.MoveRotation(Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f));

    }

    private void GroundCheck()
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
    }

    private void StopMoving()
    {
        _rb.velocity = Vector3.zero;
    }

    private void SetMoveDirectionAndRotation()
    {
        if (_move)
        {

            Vector3 moveDirection = playerCamera.forward;
            moveDirection.y = 0f;
            moveDirection.Normalize();

            if (moveDirection != Vector3.zero)
            {
                Quaternion _targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                targetRotation = Quaternion.Slerp(targetRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            return;
        }
    }

    private void HandleFreeMovement()
    {
        if (_move)
        {
            Vector3 _horizontalMovement = transform.forward * _movementSpeed * Time.fixedDeltaTime;
            _rb.velocity = new Vector3(_horizontalMovement.x, _rb.velocity.y, _horizontalMovement.z);
        }
        else
        {
            return;
        }

        //_rb.velocity = transform.forward * _movementSpeed * Time.fixedDeltaTime;

        //_rb.MoveRotation(Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f));
    }

    private void ApplyGravity()
    {
        _rb.AddForce(downwardForce, ForceMode.Acceleration);
    }
/*
    public void SetTeleporting(bool teleporting)
    {
        _isTeleporting = teleporting;
    }*/

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
