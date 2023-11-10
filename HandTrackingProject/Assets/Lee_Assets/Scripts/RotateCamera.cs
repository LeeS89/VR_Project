using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] GameObject _baseCamera;
    [SerializeField] Transform _cameraTransform;
    public float rotationSpeed = 100f;
    //private bool isRotating = false;
    //private bool _newRotate = false;
    //public float smoothness = 0.5f;
    //private Quaternion targetRotation = Quaternion.identity;

    [SerializeField] private Rigidbody rb;
    private float rotationInput = 0.0f;

    [SerializeField] GameObject _cameraRig;
    [SerializeField] SelectorUnityEventWrapper _rotateLeftPose;
    [SerializeField] SelectorUnityEventWrapper _rotateRightPose;
    [SerializeField] SelectorUnityEventWrapper[] _poses;
    private Quaternion _cameraRotation;
    private Vector3 _rotation = new Vector3(0, 0, 0);
    public float _rotateTimeOut;
    private float _disableRotate;
    private bool _startDisaleTimerLeft = false;
    private bool _startDisaleTimerRight = false;

    //private float rotationDirection = 1f;
    public Transform turnSource;

    [SerializeField] float _rotationSpeed;

    public bool _rotateLeft = false;
    private bool _rotateRight = false;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        _disableRotate = _rotateTimeOut;
        _cameraRotation = _cameraRig.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(_rotateLeft)
        {
            //_rotationSpeed = transform.eulerAngles.y;
            //_rotationSpeed = transform.rotation.y;
            transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y + _rotationSpeed,transform.rotation.z);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, (transform.rotation.y) +_rotationSpeed, transform.rotation.z));
            //_cameraRotation = Quaternion.Euler(new Vector3(_cameraRotation.x, (_cameraRotation.y) + _rotationSpeed, _cameraRotation.z));
            
            _rotationSpeed -= 1;
            //_cameraRotation.y -= _rotationSpeed;
        }
        if(_rotateRight)
        {
            //_rotationSpeed = transform.eulerAngles.y;
            //_rotationSpeed = transform.rotation.y;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + _rotationSpeed, transform.rotation.z);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, (transform.rotation.y) + _rotationSpeed, transform.rotation.z));
            //_cameraRotation = Quaternion.Euler(new Vector3(_cameraRotation.x, (_cameraRotation.y) + _rotationSpeed, _cameraRotation.z));

            _rotationSpeed += 1;
            //_cameraRotation.y -= _rotationSpeed;
        }*/
        /*if(_rotateLeft)
        {
            if(!isRotating)
            {
                isRotating = true;
                StartCoroutine(RotatePlayer());
            }
        }
        else
        {
            isRotating= false;
        }*/
        /*if(_rotateLeft)
        {
            //rotationDirection = 1f;
            isRotating = true;

            //Quaternion rotationDelta = Quaternion.Euler(0f, rotationSpeed * rotationDirection * Time.deltaTime, 0f);
            //targetRotation *= rotationDelta;
            targetRotation = Quaternion.Euler(0f,rotationSpeed * rotationDirection * Time.deltaTime,0f);

            //transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,smoothness* Time.deltaTime);
        }
        else
        {
            isRotating = false;
            rotationDirection= 0.0f;

        }

        if(isRotating)
        {
            Quaternion newRotation = Quaternion.Lerp(rb.rotation,targetRotation,smoothness * Time.deltaTime);
            rb.MoveRotation(newRotation);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
        }*/

        /*if(_rotateLeft)
        {
            rotationInput = -1f;
            float rotationAmount = rotationInput * -rotationSpeed * Time.deltaTime;

            rb.AddTorque(Vector3.down * rotationAmount);
        }else if(_rotateRight)
        {
            rotationInput = 1f;
            float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

            rb.AddTorque(Vector3.up* rotationAmount);
        }
        else
        {
            rotationInput = 0f;
        }*/
        /*if (_rotateLeft)
        {
            rotationInput = -1f;
            float rotationAmount = rotationInput * -rotationSpeed * Time.deltaTime;

            rb.AddTorque(Vector3.down * rotationAmount);
        }
        else if (_rotateRight)
        {
            rotationInput = 1f;
            float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

            rb.AddTorque(Vector3.up * rotationAmount);
        }
        else
        {
            rotationInput = 0f;
        }*/

        if (_rotateLeftPose != null)
        {
            if (_startDisaleTimerLeft)
            {
                if (_disableRotate > 0.0f)
                {
                    _disableRotate -= Time.deltaTime;
                }
                else
                {
                    DisableRotate(ref _rotateLeftPose);
                }
            }
            if (_startDisaleTimerRight)
            {
                if (_disableRotate > 0.0f)
                {
                    _disableRotate -= Time.deltaTime;
                }
                else
                {
                    DisableRotate(ref _rotateRightPose);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        /*if (_rotateLeft)
        {
            rotationInput = -1f;
            float rotationAmount = rotationInput * -rotationSpeed * Time.deltaTime;

            rb.AddTorque(Vector3.down * rotationAmount);
        }
        else if (_rotateRight)
        {
            rotationInput = 1f;
            float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

            rb.AddTorque(Vector3.up * rotationAmount);
        }
        else
        {
            rotationInput = 0f;
        }*/
        if (_rotateLeft)
        {
            rotationInput = -1f;
            Vector3 axis = Vector3.up;
            float angle = rotationSpeed * Time.fixedDeltaTime * rotationInput;
            Quaternion q = Quaternion.AngleAxis(angle, axis);
            rb.MoveRotation(rb.rotation * q);

            Vector3 newPosition = q * (rb.position - turnSource.position) + turnSource.position;
            rb.MovePosition(newPosition);
            //float rotationAmount = rotationInput * -rotationSpeed * Time.deltaTime;

            //rb.AddTorque(Vector3.down * rotationAmount);
        }
        else if (_rotateRight)
        {
            rotationInput = 1f;
            Vector3 axis = Vector3.up;
            float angle = rotationSpeed * Time.fixedDeltaTime * rotationInput;
            Quaternion q = Quaternion.AngleAxis(angle, axis);
            rb.MoveRotation(rb.rotation * q);

            Vector3 newPosition = q * (rb.position - turnSource.position) + turnSource.position;
            rb.MovePosition(newPosition);
            /*rotationInput = 1f;
            float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

            rb.AddTorque(Vector3.up * rotationAmount);*/
        }
        else
        {
            rotationInput = 0f;
        }

        /*if (_rotateLeft)
        {
            rotationInput = -1f;
            float rotationAmount = rotationInput * -rotationSpeed * Time.fixedDeltaTime;

            rb.AddTorque(Vector3.down * rotationAmount);
        }
        else if (_rotateRight)
        {
            rotationInput = 1f;
            float rotationAmount = rotationInput * rotationSpeed * Time.fixedDeltaTime;

            rb.AddTorque(Vector3.up * rotationAmount);
        }
        else
        {
            rotationInput = 0f;
        }*/
    }

    /* IEnumerator RotatePlayer()
     {
         Quaternion initialRotation = transform.rotation;
         Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, -90f, 0f);

         float t = 0f;
         while(t < 1f)
         {
             t += Time.deltaTime * rotationSpeed;
             transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
             yield return null;
         }
     }*/


    private void LateUpdate()
    {
        //_baseCamera.transform.rotation = transform.rotation;
        //_camera.transform.rotation = transform.rotation;
    }
    public void DisableRotate(ref SelectorUnityEventWrapper _pose)
    {
        if(_pose.enabled)
        {
            _pose.enabled = false;
            Set_DisableRotate();
            //SetStartDisableTimer(false);
            //_disableRotate = _rotateTimeOut;
        }

        return;
    }

    public void SetStartDisableTimer(bool enable)
    {
        _startDisaleTimerLeft = enable;
        _startDisaleTimerRight = enable;
    }

    public void ResetTransform()
    {
        transform.forward = _cameraTransform.forward;
    }

    public void Set_DisableRotate()
    {
        SetStartDisableTimer(false);
        _disableRotate = _rotateTimeOut;
    }

    public void RotateLeft(bool rotateLeft)
    {
        _rotationSpeed = transform.eulerAngles.y;
        //_rotationSpeed = transform.rotation.y;
        _rotateLeft = rotateLeft;
       /* if (_rotateLeft)
        {
            rotationDirection = 1f;
        }
        else
        {
            rotationDirection = 0f;
        }*/


        _rotateRight = false;

        if (_poses != null)
        {
            TogglePoses(rotateLeft);
        }
        
    }

    

    private void TogglePoses(bool toggle)
    {
        /*if (toggle)
        {
            if(_poses != null)
            {
                PoseManager.instance.DisablePoses(_poses);
            }
        }else if(!toggle)
        {
            if(_poses != null)
            {
                PoseManager.instance.EnablePoses(_poses);
            }
        }
        else
        {
            return;
        }*/
        return;
    }

    public void RotateRight(bool rotateRight)
    {
        _rotationSpeed = transform.eulerAngles.y;
        _rotateRight = rotateRight;
        _rotateLeft = false;
        TogglePoses(rotateRight);
    }
}
