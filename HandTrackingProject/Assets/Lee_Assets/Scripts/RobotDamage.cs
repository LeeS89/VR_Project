using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDamage : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer m_Renderer;
    [SerializeField] Material _originalMat;
    [SerializeField] Material _damageMat;
    private float _damageVisual = 0.0f;

    

    // Update is called once per frame
    void Update()
    {
        if(_damageVisual > 0.0f)
        {
            _damageVisual -= Time.deltaTime;
            m_Renderer.material = _damageMat;

            if(_damageVisual <= 0.0f)
            {
                m_Renderer.material = _originalMat;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            _damageVisual = 0.5f;
        }
    }
}
