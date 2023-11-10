using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    [SerializeField] Animator _animator;

    public void OnOpen(string[] values)
    {
        Debug.LogWarning("Door open heard");

        if (values[0] == "one")
        {

            _animator.SetBool("character_nearby", true);
        }
    }
}
