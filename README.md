# VR_Project
VR_Project created by Lee Slammon
## Description

A Virtual Reality moon based game that utilizes the gesture and voice recognition capabilities of the Meta Quest 2. Players can move around freely by simply making a certain gesture and turning their head to rotate. Special commands can be executed by combining certain gestures with spoken words/phrases, i.e. holding out your hand and saying "Freeze" stops bullets shot at the player mid flight and fires them back at the enemy upon closing your hand again. That is just 1 example of the multiple gesture/voice combination commands available in game. Players can also use their trusty lightsaber to deflect incoming fire and slice enemies in half.
[You can view a short demo of the project by clicking here](https://tusmm-my.sharepoint.com/:v:/g/personal/a00287814_student_tus_ie/EdaekUXbmQlHuhQr8vo6KxYBLjlDqbM7qjOqAcLkPVHm5Q?nav=eyJyZWZlcnJhbEluZm8iOnsicmVmZXJyYWxBcHAiOiJTdHJlYW1XZWJBcHAiLCJyZWZlcnJhbFZpZXciOiJTaGFyZURpYWxvZyIsInJlZmVycmFsQXBwUGxhdGZvcm0iOiJXZWIiLCJyZWZlcnJhbE1vZGUiOiJ2aWV3In19&e=Ua9f7e)
## Enemy AI Finite State Machine

The enemies in the game utilize Unity's NavMesh system and are controlled by a Finite State MAchine which allows for clear and manageable transitions between their different states such as: Stationary, Chasing, Patrol, Backtracking and Death.

Each state inherits from a baseState class which contains shared functions required for each state

- Chasing State: When an enemy "spots" the player this send an alert to the rest of that enemies group, and each agent enters the chasing state, hunting down the player and shooting on sight
```
private void ChasePlayer()
    {
        bool _checkValidPath = CheckPathValidity(GetClosestPointOnNavMesh(transform.position), playerPos);

        if(!_checkValidPath )
        {
            BaseState newState = GetComponent<StationaryState>();
            _stateController.RequestStateChange(newState);
        }
        else
        {
            _agent.stoppingDistance = 5.0f;
            _agent.SetDestination(playerPos);
        }

        if (distToPlayer <= (_tempStopDistance - 1.0f))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
                BaseState newState = GetComponent<BacktrackState>();
                _stateController.RequestStateChange(newState);
            }
           
        }

        if (distToPlayer <= (_tempStopDistance + 0.1f))
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(PlayerProximityDelay());
            }       
        }
        else if (distToPlayer <= _runningDistance)
        {
            _animation.WalkingDistance();
        }
        else
        {
            _animation.RunningDistance();
        }
    
    }

    IEnumerator PlayerProximityDelay()
    {
        yield return new WaitForSeconds(0.2f);

        Debug.Log("Successful change");
        _coroutine = null;
        BaseState newState = GetComponent<StationaryState>();
        _stateController.RequestStateChange(newState);
    }
```

- Stationary State: When the enemy reaches the player, it stops chasing and enter its stationary state and continues shooting at the player. If the player moves, the agents enter the chasing state again

- Backtracking State: When an enemy has reached the player and entered its stationary state and if the player moves towards the enemy, the enemy then enters its backtracking state where it walks backwards to avoid the players weapon and continues to shoot.

- Patrol State: The default state for the agents. Agents follow a certain path around the navmesh and is given a new random point at each destination to keep the player guessing.

The transition between each state is handled by the state transition manager. Any time an agent requires a state change, the current state sends a request to the state transition manager to handle the transition smoothly. First the OnExitState() method is called from the current state to stop any state specific functions from running and then the OnEnterState() method of the new state is called to initialize or re-initialize its functions and variables.

```
private void ChangeState(BaseState state)
    {
        _currentState.ExitState();

        _currentState = state;
        _currentState.EnterState();
    }
```


#### Project Updates
I will be updating this ReadMe periodically with what I have been working on up to that point.

Project Creation
- Sourcing assets for my home scene and moonbase scenes environment
- Sourcing player objects and enemy objects for moonbase scene
- Designed Lightsaber in blender & imported to unity & combined parts of lightsaber with downloaded lightsaber asset
- Setting up shaders & partial scene lighting (for the area I will be testing the main functionality before expanding) and optimizing for Vr

NavMesh & NavMesh agent Creation
- Researching documentation and youtube videos on how to set up navMesh.
- Set up enemy objects with navmesh agent components
- Created animation state trees for enemies
- Set up enemy states i.e. Patrol/Alert/Stationary/Death/Backtrack
- Added Field of view script to enemies (Followed tutorial for this & modified to fit my own custom functionality)
- Set up way points for navmesh agents

Agent State transition & firing weapon set up 
