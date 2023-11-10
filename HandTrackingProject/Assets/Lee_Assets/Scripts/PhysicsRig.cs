using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRig : MonoBehaviour
{

    public Transform playerHead;
   /* public Transform leftController;
    public Transform rightController;
*/
    //public GameObject _leftJoint;
    //public GameObject _rightJoint;

   /* public GameObject leftHand;
    public GameObject rightHand;
*/
    //public ConfigurableJoint headJoint;
   /* public ConfigurableJoint rightControllerJoint;
    public ConfigurableJoint leftControllerJoint;*/

    public CapsuleCollider playerCollider;

    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2f;

   

    // Update is called once per frame
    void FixedUpdate()
    {
        playerCollider.height = Mathf.Clamp(playerHead.localPosition.y,bodyHeightMin, bodyHeightMax);
        playerCollider.center = new Vector3(playerHead.localPosition.x, playerCollider.height / 2, playerHead.localPosition.z);

       /* leftControllerJoint.targetPosition = leftController.localPosition;
        leftControllerJoint.targetRotation = leftController.localRotation;*/
        /* leftControllerJoint.targetPosition = leftHand.transform.localPosition;
         leftControllerJoint.targetRotation = leftHand.transform.localRotation;*/

        //_leftJoint.transform.localPosition = leftHand.transform.localPosition;
        //_rightJoint.transform.localRotation = rightHand.transform.localRotation;

       /* rightControllerJoint.targetPosition = rightController.localPosition;
        rightControllerJoint.targetRotation = rightController.localRotation;*/
        /*rightControllerJoint.targetPosition = rightHand.transform.localPosition;
        rightControllerJoint.targetRotation = rightHand.transform.localRotation;*/

        //headJoint.targetPosition = playerHead.localPosition;
    }
}
