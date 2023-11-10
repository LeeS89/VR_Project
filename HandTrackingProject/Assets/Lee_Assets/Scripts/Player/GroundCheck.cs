using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public Transform leftHand;
    public Transform rightHand;

    [SerializeField]
    private float _groundCheckDistance = 0.1f;
   
    private bool _isGrounded = false;


    private void Update()
    {
        Debug.DrawRay(rightHand.position, -rightHand.right, Color.yellow);
        Debug.DrawRay(leftHand.position, leftHand.right, Color.blue);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _, _groundCheckDistance))
        {
            _isGrounded = true;
            
        }
        else
        {
            _isGrounded = false;
           
        }

    }

    public bool IsGrounded() => _isGrounded;
    /*{
        return _isGrounded;
    }*/
}
