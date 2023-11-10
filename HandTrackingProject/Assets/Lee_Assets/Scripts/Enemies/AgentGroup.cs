using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AgentGroup : MonoBehaviour
{
    public List<PatrolState> agents;
    public List<DeathState> _deadAgents;
    public AudioSource _alert;

    private void Awake()
    {
        _alert = GetComponent<AudioSource>();
        foreach (var agent in agents)
        {
            if (agent.TryGetComponent<DeathState>(out var deathState))
            {
                _deadAgents.Add(deathState);
            }
        }
    }

    
    /// <summary>
    /// Subscribes to the enemies OnDisabled event
    /// </summary>
    private void OnEnable()
    {
        /*Debug.Log("OnEnable called!!!!!!!!!!!!!!!!!!!!!!!!1");*/
        foreach (var agent in _deadAgents)
        {
            agent.OnDisabled += CheckAgentGroupState;
        }
    }

    private void OnDisable()
    {
        foreach (var agent in _deadAgents)
        {
            agent.OnDisabled -= CheckAgentGroupState;
        }
    }

    /// <summary>
    /// When an agent is killed, this checks if any agents from that agents group
    /// are still active, if none are active then this starts a countdown to revive the group again
    /// </summary>
    private void CheckAgentGroupState()
    {
        /*Debug.Log("Check state called!!!!!!!!!!!!!!!!!!!!!!!!1");*/
        if (EntireGroupDead())
        {
            StartCoroutine(ReviveGroup());
        }
    }

    private bool EntireGroupDead()
    {
        foreach (var agent in _deadAgents)
        {
            if(agent.IsEnabled)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Revives Agents group.
    /// Need to come back to later to add variable revival times
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReviveGroup()
    {
        //Debug.Log("Revive called!!!!!!!!!!!!!!!!!!!!!!!!1");

        yield return new WaitForSeconds(5.0f);

        foreach(var agent in _deadAgents)
        {
            agent.transform.root.gameObject.SetActive(true);
            agent.Revive();
        }
    }


    /// <summary>
    /// When an agent spots the player, it alerts the rest of the agents in the group to the players location 
    /// </summary>
    public void AlertAgents()
    {
        _alert.Play();

        foreach (var agent in agents)
        {
            
            agent.ActivateAlertPhase();
        }
    }

    
}
