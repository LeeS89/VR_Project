using UnityEngine;

public abstract class BaseRotation : MonoBehaviour
{

    [SerializeField]
    protected GroundCheck groundCheck;

    // Physics variables and references
    protected Rigidbody _rb;
    
   

    //Rotation and movement variables
    protected Quaternion _targetRotation;
    protected Vector3 _moveDirection;

    [SerializeField]
    protected float _rotationSpeed = 2f;
   
    protected bool _isRotating = false;

    [SerializeField]
    protected Transform playerCamera;


    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
       
        _moveDirection = playerCamera.forward;
        _targetRotation = transform.rotation;
    }


    protected virtual void Update()
    {
        //_moveDirection = playerCamera.forward;
       /* _rb.MoveRotation(Quaternion.Euler(0f, _targetRotation.eulerAngles.y, 0f));*/
      
    }

    protected virtual void LateUpdate()
    {

    }

    public abstract void HandleRotation();

   /* /// <summary>
    /// Raycast is cast from the players position downwards for a distance of _groundCheckDistance
    /// to detect wether the player is grounded or not
    /// This is used to determine if gravity needs to be applied to the player
    /// </summary>
    protected virtual void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance))
        {
            _isGrounded = true;
         
        }
        else
        {
            _isGrounded = false;
           
        }

    
    }*/
}
