using Oculus.Interaction;
using UnityEngine;

public class SnapRotation : BaseRotation
{
    private Quaternion _targetRot;
    public Transform leftHand;
    //public Transform rightHand;

    [SerializeField] SelectorUnityEventWrapper _applyRotation;
    private float yRotation;

    private bool _canRotate = false;
    private float rotationTimeout = 0.0f;
    private float _timeToRotate = 0.0f;

    [SerializeField] private Transform _rHand;
    private Vector3 _handDirection;


    protected override void Awake()
    {
        base.Awake();
        /*_handDirection = -_rHand.right;*/
    }

    public void HandleRotationSnap()
    {
        if (_canRotate)
        {
            //Debug.LogError("Snap rotation is being called!!!!!");
            //if (!_isTeleporting)
            //{
            //_moveDirection = playerCamera.forward;
            _handDirection = -_rHand.right;
            _handDirection.y = 0f;
            _handDirection.Normalize();

            //Quaternion _targetEndRotation = Quaternion.LookRotation(_handDirection, Vector3.up);
            Quaternion _targetEndRotation = Quaternion.LookRotation(_handDirection, new Vector3(0,0,1));
            //float angleDifference = Quaternion.Angle(this._targetRotation, _targetEndRotation);

            //float someThreshold = 20.0f; // 15 degrees threshold

            //if (angleDifference > someThreshold)
            //{
            //Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            //this._targetRotation = Quaternion.Slerp(this._targetRotation, _targetEndRotation, _rotationSpeed * Time.deltaTime);
            this._targetRotation = Quaternion.Euler(0, _targetEndRotation.eulerAngles.y, 0);
            //}
            //_rb.MoveRotation(Quaternion.Euler(0f, this._targetRotation.eulerAngles.y, 0f));
            //}
        }

        if(_timeToRotate > 0.0f)
        {
            _timeToRotate -= Time.deltaTime;

            if(_timeToRotate <= 0.0f )
            {
                _canRotate = true;
            }
        }
        if(rotationTimeout > 0.0f)
        {
            rotationTimeout -= Time.deltaTime;

            if(rotationTimeout <= 0.0f)
            {
                _canRotate = false;
                _applyRotation.enabled = false;
            }
        }
    }


    public void SetRotationTimeout()
    {
        rotationTimeout = 0.75f;
    }

    public void RotationActivateTimer()
    {
        _timeToRotate = 0.5f;
    }

    public override void HandleRotation()
    {
        //Debug.LogError("Snap rotation is being called!!!!!");
        //if (!_isTeleporting)
        //{
            _moveDirection = playerCamera.forward;
            _moveDirection.y = 0f;
            _moveDirection.Normalize();

            Quaternion _targetEndRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            float angleDifference = Quaternion.Angle(this._targetRotation, _targetEndRotation);

            float someThreshold = 20.0f; // 15 degrees threshold

            if (angleDifference > someThreshold)
            {
                //Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
                this._targetRotation = Quaternion.Slerp(this._targetRotation, _targetEndRotation, _rotationSpeed * Time.deltaTime);
            }
        _rb.MoveRotation(Quaternion.Euler(0f, this._targetRotation.eulerAngles.y, 0f));
        //}
    }

   /* public void SnapRot()
    {
        _targetRot = Quaternion.LookRotation(-rightHand.right, Vector3.up);

        yRotation = _targetRot.eulerAngles.y;
        //transform.rotation = _targetRot;
    }*/

    public void SetCanRotate(bool rotate)
    {
        _canRotate = rotate;
    }

    public void ApplyRotation()
    {
        if (_canRotate)
        {
            //if (this._targetRotation != null)
            //{
            //transform.rotation = Quaternion.Euler(0, yRotation, 0);
            //transform.rotation = this._targetRotation;
            _rb.MoveRotation(this._targetRotation);
            _canRotate = false;
            //transform.rotation = Quaternion.Euler(0f, this._targetRotation.eulerAngles.y, 0f);
            //this._targetRotation = transform.rotation;

            //this._targetRotation = null;

            //}
            //_canRotate = false;
        }
    }

  

}
