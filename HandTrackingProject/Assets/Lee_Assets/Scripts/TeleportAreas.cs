using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAreas : MonoBehaviour
{
    //[Header("Teleport Points")]
    [SerializeField] Transform[] _teleportPoints;

    // Start is called before the first frame update
    void Start()
    {
        if (_teleportPoints != null)
        {
            TeleportToPoint(ref _teleportPoints, 0);
        }
    }



    public void TeleportToPoint(ref Transform[] teleportPoints, int index)
    {
        Debug.LogWarning("Teleport called");
        transform.position = teleportPoints[index].transform.position;
        transform.rotation = teleportPoints[index].transform.rotation;
    }
}
