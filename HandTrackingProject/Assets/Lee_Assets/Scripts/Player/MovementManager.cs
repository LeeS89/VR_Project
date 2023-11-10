using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public enum RotationType
    {
        FreeRotation,
        SnapRotation
    }
    public RotationType rotationMode;

    public FreeRotationtion freeRotation;
    public SnapRotation snapRotation;

    /// <summary>
    /// Need to implement ability for user to toggle between the 2 types of mevement
    /// </summary>
    public enum MoveType
    {
        FreeMovement,
        Teleporting
    }
    public MoveType moveMode;

    public FreeMovement freeMovement;
    public Teleporting teleporting;


    
    // Update is called once per frame
    void Update()
    {
        switch (moveMode)
        {
            case MoveType.FreeMovement:
                //freeMovement.HandleRotation();
                break;
            case MoveType.Teleporting:
                teleporting.HandleMovement();
                //freeMovement.SetMoveDirectionAndRotation();
                break;
        }

        switch (rotationMode)
        {
            case RotationType.FreeRotation:
                freeRotation.HandleRotation();
                break;
                case RotationType.SnapRotation:
                snapRotation.HandleRotationSnap();
                break;
            default:
                Debug.LogError("No Rotation type selected");
                break;
        }
    }

    /// <summary>
    /// Free movement uses physics so needs to be used within fixedUpdate
    /// </summary>
    private void FixedUpdate()
    {
        switch (moveMode)
        {
            case MoveType.FreeMovement:
                freeMovement.HandleMovement();
                //freeMovement.HandleRotation();
                break;
            case MoveType.Teleporting:
                //teleporting.HandleRotation();
                break;
            default:
                Debug.LogError("No movement type selected");
                break;
        }
    }


}
