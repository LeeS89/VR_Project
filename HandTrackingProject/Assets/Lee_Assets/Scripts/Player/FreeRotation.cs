using UnityEngine;

public class FreeRotationtion : BaseRotation
{
    public override void HandleRotation()
    {

        _moveDirection = playerCamera.forward;
        _moveDirection.y = 0f;
        _moveDirection.Normalize();

        Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
        float angleDifference = Quaternion.Angle(this._targetRotation, _targetRotation);

        float rotateThreshold = 20.0f;
        float stopRotatingThreshold = 18.0f;

        if (!_isRotating && angleDifference > rotateThreshold)
        {
            _isRotating = true;
        }
        else if (_isRotating && angleDifference < stopRotatingThreshold)
        {
            _isRotating = false;
        }

        if (_isRotating)
        {
            //Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            this._targetRotation = Quaternion.Slerp(this._targetRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            
        }

        _rb.MoveRotation(Quaternion.Euler(0f, this._targetRotation.eulerAngles.y, 0f));
    }

}
