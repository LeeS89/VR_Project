using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerDamage))]
public class EnemyHealthController : MonoBehaviour, IEnemyController
{
    private StateController m_StateController;
    [SerializeField] int _health = 4;
    private int _currentHealth;
    public int id;

    [SerializeField] SkinnedMeshRenderer m_Renderer;
    [SerializeField] Material _originalMat;
    [SerializeField] Material _damageMat;
    private float _damageVisual = 0.0f;


    public bool _death = false; // Test, can be deleted

    private void Awake()
    {
        m_StateController = GetComponentInParent<StateController>();
        SetHealth(_health);
        m_Renderer = GetComponent<SkinnedMeshRenderer>();
        
    }

    private void OnEnable()
    {
        SetHealth(_health);

        if (EnemyDamageEvent.instance != null)
        {
            SubscribeToEvent();
        }
    }

    private void OnDisable()
    {
        EnemyDamageEvent.instance.onDamageTriggerEnter -= OnTakeDamage;
    }

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvent();
    }

    private void Update()
    {
        if(_death) // Test
        {
            EnemyDamageEvent.instance.DamageTriggerEnter(id, 4);

            _death= false;
        }

        if (_damageVisual > 0.0f)
        {
            _damageVisual -= Time.deltaTime;
            m_Renderer.material = _damageMat;

            if (_damageVisual <= 0.0f)
            {
                m_Renderer.material = _originalMat;
            }
        }
    }

    private void SubscribeToEvent()
    {
        EnemyDamageEvent.instance.onDamageTriggerEnter += OnTakeDamage;
    }

    private void OnTakeDamage(int id, int damage)
    {
        if(id == this.id)
        {
            _damageVisual = 0.5f;
            UpdateHealth(damage);
        }
    }

    public void SetHealth(int health)
    {
        _currentHealth = health;
    }

    public int GetHealth()
    {
        return _currentHealth;
    }

    public void UpdateHealth(int damageAmount)
    {
        int healthUpdated = _currentHealth - damageAmount;

        if (healthUpdated > 0)
        {
            SetHealth(healthUpdated);
        }
        else
        {
            BaseState _newState = GetComponentInParent<DeathState>();
            m_StateController.RequestStateChange(_newState);
            TriggerDeathAnimation();
        }
    }

    public void TriggerDeathAnimation()
    {
        //Debug.LogWarning("Death called");
        EnemyDamageEvent.instance.DeathEnter(id);
    }

   
}
