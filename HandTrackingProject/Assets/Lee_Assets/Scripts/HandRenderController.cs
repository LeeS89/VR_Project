using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRenderController : MonoBehaviour
{
    //private OVRPlayerController playerController;
    public SkinnedMeshRenderer[] hands;


    // Start is called before the first frame update
    void Start()
    {
        //playerController= GetComponent<OVRPlayerController>();
        //hands = GetComponentsInChildren<SkinnedMeshRenderer>();
        ActivateHands();
        //bool usingHands = playerController.GetUse
    }
    private void Update()
    {
        ActivateHands();
    }

    private void ActivateHands()
    {
        foreach(var hand in hands)
        {
            hand.enabled = true;
        }
    }
    
}
