using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    public void IgnoreLayers()
    {
        Physics.IgnoreLayerCollision(7, 8, true);
    }

    public void ResetLayers()
    {
        Physics.IgnoreLayerCollision(7, 8, false);
    }
}
