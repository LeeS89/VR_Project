using UnityEngine;

/// <summary>
/// The purpose of this script is to handle the enemy state transitions and nothing else
/// </summary>
public class StateController : MonoBehaviour
{
    private Vector3 _startPos;
    private Quaternion _startRot;


    private BaseState _currentState;
    private PatrolState _patrolState;
    private StationaryState _stationaryState;
    private BacktrackState _backtrackState;
    private NewChasingState _newChasingState;
    private DeathState _deathState;


    private void Awake()
    {
        _backtrackState = GetComponent<BacktrackState>();
        _stationaryState = GetComponent<StationaryState>();

        _deathState = GetComponent<DeathState>();
        _newChasingState = GetComponent<NewChasingState>();

        _patrolState = GetComponent<PatrolState>();
    }

    private void OnEnable()
    {
        if (_currentState != null)
        {
            transform.position = _startPos;
            transform.rotation = _startRot;

            ChangeState(_patrolState);
           /* _currentState = _patrolState;
            _currentState.EnterState();*/
        }
    }

    private void OnDisable()
    {
        //EnemyDeathController.Instance.AddtoDisabledList(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;

        
        _currentState = _patrolState;
        _currentState.EnterState();
    }

    public void RequestStateChange(BaseState newState)
    {
        ChangeState(newState);
    }

    

    // Update is called once per frame
    void Update()
    {

        Debug.LogError("Current state of gameObject: "+gameObject.GetType()+" is: " + _currentState.ToString());
        _currentState.UpdateState();
    }

    private void LateUpdate()
    {
        _currentState.LateUpdateState();
    }

    private void ChangeState(BaseState state)
    {
        _currentState.ExitState();

        _currentState = state;
        _currentState.EnterState();
    }
}
