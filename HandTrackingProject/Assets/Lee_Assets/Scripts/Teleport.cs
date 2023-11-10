using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    //[Header("Teleport Points")]
    [SerializeField] Transform[] _teleportPoints;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if(_teleportPoints != null)
        {
            TeleportToPoint(ref _teleportPoints, 0);
        }
    }

   

    public void TeleportToPoint(ref Transform[] teleportPoints, int index)
    {
        Debug.LogWarning("Teleport called");
        /*transform.position = teleportPoints[index].transform.position;
        transform.rotation = teleportPoints[index].transform.rotation;*/

        rb.MovePosition(teleportPoints[index].transform.position);
        rb.MoveRotation(teleportPoints[index].transform.rotation);
    }
}
