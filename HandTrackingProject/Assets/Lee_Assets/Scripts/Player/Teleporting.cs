using System;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class Teleporting : BaseMovement
{
    private bool _isTeleporting = false;
    private bool _canTeleport = false;
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private GameObject _indexFingerTipLeft;
    public float maxLineDistance;
    private Vector3[] _points;

    public Material _green;
    public Material _red;

    private Vector3 _newPosition;
    private Quaternion _newRotation;

    protected override void Awake()
    {
        base.Awake();
        //_lineRenderer = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void HandleMovement()
    {
        if(_isTeleporting)
        {
            DrawLineRenderer();
        }
    }

    /// <summary>
    /// For testing purposes only
    /// </summary>
    private void DrawLineRenderer()
    {
        Vector3 startPoint = _indexFingerTipLeft.transform.position;
        Vector3 endPoint = _indexFingerTipLeft.transform.position - _indexFingerTipLeft.transform.right * maxLineDistance;
        _points = new Vector3[] { startPoint, endPoint };
        Debug.LogError($"Start Point: {startPoint}, End Point: {endPoint}");
        Vector3 direction = (endPoint - startPoint).normalized;

        _lineRenderer.SetPositions(_points);

        if (Physics.Raycast(startPoint,direction,out RaycastHit hit, maxLineDistance))
        {
            //endPoint = hit.point;

            if (NavMesh.SamplePosition(hit.point, out NavMeshHit hit1, 0.2f, NavMesh.AllAreas))
            {
                //endPoint = hit1.position;
                _lineRenderer.material = _green;
                _newPosition = hit1.position;
                _newRotation = transform.rotation;
                _canTeleport = true;
            }
            else
            {
                _lineRenderer.material = _red;
                _canTeleport = false;
            }
        }
        else
        {
            _lineRenderer.material = _red;
            _canTeleport = false;
        }
        /*_points = new Vector3[] { startPoint, endPoint };
        _lineRenderer.SetPositions(_points);*/


        /*_lineRenderer.SetPosition(0, startPoint);
        _lineRenderer.SetPosition(1, endPoint);*/
    }

    public void Teleport()
    {
        if(_canTeleport)
        {
            _rb.MovePosition( _newPosition );
            //transform.position = _newPosition;
            _rb.MoveRotation( _newRotation );
            //transform.rotation = _newRotation;
            //_canTeleport = false;
        }
        
        SetIsTeleporting(false);
    }

   /* /// <summary>
    /// Free rotation is suspended while the user is attempting to select a teleport point
    /// to ensure the user is always facing the direction in which they wish to teleport and to avoid accidental rotation
    /// which may disorientate the user
    /// </summary>
    public override void HandleRotation()
    {
        if (!_isTeleporting)
        {
            _moveDirection = playerCamera.forward;
            _moveDirection.y = 0f;
            _moveDirection.Normalize();

            Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            float angleDifference = Quaternion.Angle(this._targetRotation, _targetRotation);

            float someThreshold = 20.0f; // 15 degrees threshold

            if (angleDifference > someThreshold)
            {
                //Quaternion _targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
                this._targetRotation = Quaternion.Slerp(this._targetRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }*/


    /// <summary>
    /// This method is called by the teleporting gesture
    /// to suspend free rotation and to enter the teleporting method
    /// </summary>
    /// <param name="teleporting"></param>
    public void SetIsTeleporting(bool teleporting)
    {
        if (teleporting)
        {
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }

        _isTeleporting = teleporting;
        
    }

   
}
