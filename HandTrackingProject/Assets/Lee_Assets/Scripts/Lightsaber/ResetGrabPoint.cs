using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGrabPoint : MonoBehaviour
{

    private Vector3 originalPos;
    private Quaternion originalRot;
    [SerializeField] Rigidbody rb;
    [SerializeField] Rigidbody rb2;

    // Start is called before the first frame update
    /*void Start()
    {
       originalPos= transform.localPosition;
       originalRot= transform.localRotation;
    }*/

    public void ResetPos()
    {
        Physics.IgnoreLayerCollision(0, 6,false);
        Physics.IgnoreLayerCollision(0, 4, false);
        transform.localPosition = originalPos;
        transform.localRotation = originalRot;

        rb.isKinematic = true;
        rb.useGravity = false;

        /*rb2.isKinematic = true;
        rb2.useGravity = false;*/
    }

    public void SetPos()
    {
        //Physics.IgnoreLayerCollision(0, 6, true);
        Physics.IgnoreLayerCollision(0, 4, true);

        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
