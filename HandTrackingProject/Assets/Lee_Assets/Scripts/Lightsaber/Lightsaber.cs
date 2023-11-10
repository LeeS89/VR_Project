//using AvatarAcademy;
using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    //[SerializeField] Rigidbody _handle;
    [SerializeField] private Animator _animator;
    [SerializeField] Animator _rotation;
    [SerializeField] AudioSource _audioSource1;
    [SerializeField] AudioSource _audioSource2;
    [SerializeField] AudioSource _audioSource3;
    [SerializeField] AudioClip _audioClip1;
    [SerializeField] AudioClip _audioClip2;
    [SerializeField] AudioClip _audioClip3;
    //[SerializeField] AVA_GrabHighlight _grabHighlight;
    private string[] _triggers = { "Extend", "Retract" };
    private int _index = 0;

    [SerializeField] float _speed = 0.0f;
    [SerializeField] private GameObject _hand;
    private bool _hum = false;
    Vector3 goalPosition;
    private bool _grabSword = false;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _isExtended = false;
    //private Transform _transform;
    [SerializeField] GameObject _parent;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponentInParent<Rigidbody>();
       // _transform = GetComponent<Transform>();
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        //_animator = GetComponent<Animator>();
        //_audioSource = GetComponent<AudioSource>();
        _hand = GameObject.Find("l_handMeshNode");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_grabSword)
        {
            goalPosition = _hand.transform.position;
            //goalPosition = goalPosition.normalized * 1 * Time.fixedDeltaTime;
            rb.isKinematic = false;
            //rb.useGravity = true;
            rb.AddForce((goalPosition - rb.transform.position) * 1);
            //rb.velocity = (goalPosition - (rb.transform.position)) * 1.5f;
            //rb.MovePosition((rb.transform.position + goalPosition) * Time.fixedDeltaTime * 1);
            //_parent.transform.localPosition = Vector3.MoveTowards(transform.localPosition, goalPosition, _speed);
        }
        
       /* if(_hum)
        {
            
            //_audioSource3.clip = _audioClip3;
            _audioSource2.Play();
            _audioSource2.loop = true;
        }
        else
        {
            _audioSource2.Stop();
            _audioSource2.loop = false;
        }*/

    }

    public void ToggleLightsaber()
    {
        _animator.SetTrigger(_triggers[_index]);
        if(_index == 0)
        {
            _audioSource1.PlayOneShot(_audioClip1);
            _audioSource2.Play();
            _audioSource2.loop = true;
            _isExtended = true;
        }
        else
        {
            _audioSource2.Stop();
            _audioSource2.loop = false;
            _audioSource1.PlayOneShot(_audioClip2);
            _isExtended = false;
            
        }
        _index++;
       
        if (_index == _triggers.Length)
        {
            _index = 0;
        }
    }

    public void ForceGrabSword()
    {
        _grabSword = true;
        _rotation.SetTrigger("StopRotate");
        _rotation.enabled= false;
        //ActivateHighLight(false);
    }

    public void DropSword()
    {
        _grabSword = false;
        rb.isKinematic = true;
        //ActivateHighLight(true);

    }

    /*public void ActivateHighLight(bool state)
    {
        _grabHighlight.ToggleHighlight(state);
    }*/

    public void ResetSword()
    {
        
        _parent.transform.position = _startPosition;
        _parent.transform.rotation = _startRotation;

        if (_isExtended)
        {
            ToggleLightsaber();
        }
        //ActivateHighLight(true);
        RotateSword();
        
    }

    private void RotateSword()
    {
        if (!_rotation.enabled)
        {
            _rotation.enabled = true;
            _rotation.ResetTrigger("StopRotate");
            _rotation.SetTrigger("Rotate");
        }
        else
        {
            return;
        }
        //_rotation.SetBool("Restart",true);
        
    }

    public void ExtendLightsaber()
    {
        _animator.SetTrigger("Extend");
        //_audioSource.PlayOneShot(_audioClip1);
        _audioSource1.PlayOneShot(_audioClip2);
        _isExtended = true;
    }
    

    public void RetractLightsaber()
    {
        _animator.SetTrigger("Retract");
        _isExtended = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "StopGrab")
        {
            DropSword();
        }
    }
}
