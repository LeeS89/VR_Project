using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTest : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioSource _deflect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActivateAudio()
    {
        if(!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        if(CompareTag("Head"))
        {
            _deflect.Play();
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Head")
        {
            _deflect.Play();
        }
    }

}
