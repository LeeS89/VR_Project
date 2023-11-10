using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class ChaseUpdated : BaseState
{
    //public GameObject _player;
    private Transform _dest;
    private NavMeshObstacle _obstacle;

    //public Transform playerTransform;
    public float rotationSpeed;
    //private NavMeshAgent _agent;
    public Transform destination;
    private Transform tempDestination;

    private float _tempStopDistance = 0.0f;
    //[SerializeField] Animator _animator;

    public GameObject _raycastPoint;
    public float speed = 1.0f;
    //private FieldOfView _fieldOfView;
    private bool _canSeePlayer = false;

    private NavMeshPath _path;

    //private float nextPathCheckTime = 0.5f;
    //private float pathCheckInterval = 0.5f;

    private Vector3 playerPos;
    //private Vector3 closestPointOnNavMesh;
    float stoppingDistanceBuffer = 1.0f;
    public bool isBackTracking = false;
    private Vector3 _animDestination;

    Vector3 escapeDirection = Vector3.zero;
    Vector3 newDestination = Vector3.zero;
    float distToDestination = 0.0f;

    public GameObject _cube;
    private Vector3 backtrackingStartPosition;
    Vector3 newPosition;

    private bool isSidetracking = false;
    private Vector3 sidetrackPosition;
    //private EnemyAnimationController _animation;

    private bool _carve = false;
    private float distanceWeight = 1.0f;    // You can adjust these weights based on your design
    private float proximityWeight = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        //_animation = GetComponent<EnemyAnimationController>();
        _obstacle= GetComponent<NavMeshObstacle>();
        tempDestination = destination;
        _path = new NavMeshPath();
        //_fieldOfView = GetComponent<FieldOfView>();
    }

    protected override void OnEnable()
    {
        //_agent = GetComponent<NavMeshAgent>();
        _tempStopDistance = _agent.stoppingDistance;
        
    }

    public override void EnterState()
    {
        StartCoroutine(SetDest());

        /*_agent.SetDestination(playerPos);
        UpdatePlayerPos();*/
    }


    IEnumerator SetDest()
    {
        yield return new WaitForEndOfFrame();

        _agent.SetDestination(playerPos);
    }


    public override void UpdateState()
    {
        ShouldDisableAgent();

        if (_carve && _obstacle.carving)
        {
            _obstacle.carving = false;
        }
        else if (!_carve && !_obstacle.carving)
        {
            _obstacle.carving = true;
        }


        /*if (_agent.enabled && _obstacle.carving)
        {
            _obstacle.carving = false;
        }
        if (!_agent.enabled && !_obstacle.carving)
        {
            _obstacle.carving = true;
        }*/
        //SampleBacktrackPoints();

        Debug.LogWarning("Backtracking value: " + isBackTracking);
        Debug.LogError("Stopping distance: " + _tempStopDistance);
        UpdateFieldOfView();
        //closestPointOnNavMesh = GetClosestPointOnNavMesh(transform.position);

        /*if (_agent.enabled)
        {
            //UpdatePlayerPos();
            UpdateAgentDestination();
        }*/
       /* Vector3 currentDestination = _agent.destination;
        Debug.LogWarning(currentDestination + ": New destination should work");
        if (_agent.destination == newDestination)
        {
            Debug.LogWarning("Destinationvshould be the new destination");

            if (distToDestination <= _agent.stoppingDistance)
            {
                isBackTracking = false;
                _agent.enabled = false;
                _agent.stoppingDistance = 5;
                destination = tempDestination;
                _agent.SetDestination(playerPos);
            }
        }*/
    }

    private void LateUpdate()
    {

        UpdatePlayerPos();

        /*///if (_agent.enabled)
        //{
            UpdateAgentDestination();
        //}*/
        if (isBackTracking)
        {
            /*if (backtrackingStartPosition == Vector3.zero)
            {
                // Sample navmesh points and choose the best destination
                Vector3 newDestination = SampleBacktrackPoints();

                // Set the new destination for the backtracking
                backtrackingStartPosition = newDestination;
                _agent.SetDestination(newDestination);
            }

            // Check if backtracking is complete
            if (Vector3.Distance(transform.position, backtrackingStartPosition) < _agent.stoppingDistance)
            {
                isBackTracking = false;
                backtrackingStartPosition = Vector3.zero;
            }*/
            Backtrack();
            //Backtracking();
        }
        else
        {
            UpdateAgentDestination();
            //RotateTowardsPlayer();
        }


       
            //RotateTowardsPlayer();
        
    }

    private void UpdateFieldOfView()
    {
        _canSeePlayer = _fieldOfView.PlayerInView();
    }

    private void UpdatePlayerPos()
    {
        playerPos = PlayerPosSearch.instance.GetPlayerPos();
    }

    private void StartBacktracking()
    {
       /* if (_obstacle.carving)
        {
            _obstacle.carving = false;
        }*/

        
        isBackTracking = true;
        Vector3 newDestination = SampleBacktrackPoints();
        _animDestination = newDestination;
        //_agent.stoppingDistance = 0.0f;

        StartCoroutine(SetDestination(newDestination, null,0.0f));
        //SetAgentDestination(newDestination, null);
        //_animation.SetBackTracking(true);
        //_agent.SetDestination(newDestination);
    }

    /*private void SetAgentDestination(Vector3 dest, Transform dest2)
    {
        if(_agent.enabled && _obstacle.carving)
        {
            _obstacle.carving = false;
        }else if(!_agent.enabled && !_obstacle.carving)
        {
            _obstacle.carving = true;
        }


        if(dest == Vector3.zero)
        {
            _agent.SetDestination(dest2.position);
        }
        else if(dest2 == null)
        {
            _agent.SetDestination(dest);
        }
    }*/

    IEnumerator SetDestination(Vector3 dest, Transform dest2, float stopDist)
    {
        //_agent.enabled = ShouldDisableAgent();
        


        yield return new WaitForSeconds(0.2f);

        _agent.stoppingDistance = stopDist;
        if (dest == Vector3.zero)
        {
            _agent.SetDestination(dest2.position);
        }
        else if (dest2 == null)
        {
            _agent.SetDestination(dest);
        }
    }

    private void Backtrack()
    {
        if (Vector3.Distance(transform.position, _agent.destination) <= (_agent.stoppingDistance + 0.5f))
        {
            // Backtracking completed
            isBackTracking = false;
            //_agent.SetDestination(playerPos);
            //_animation.SetBackTracking(false);
        }
    }

    private Vector3 SampleBacktrackPoints()
    {
        Vector3 playerToAgent = transform.position - playerPos;
        float angleRange = 360.0f; // Adjust this angle based on your design
        int numSamples = 8; // Adjust the number of samples based on your needs
        float samplingRadius = 5.0f; // Adjust the radius based on your needs

        Vector3 bestPoint = Vector3.zero;
        float bestScore = float.MinValue;

        for (int i = 0; i < numSamples; i++)
        {
            float angle = (i / (float)(numSamples - 1)) * angleRange - angleRange / 2.0f;
            Vector3 rotatedDirection = Quaternion.Euler(0, angle, 0) * playerToAgent;
            Vector3 samplePosition = transform.position + rotatedDirection.normalized * samplingRadius;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(samplePosition, out hit, samplingRadius, NavMesh.AllAreas))
            {
                float score = CalculateScore(samplePosition);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestPoint = samplePosition;
                }
            }
        }

        return bestPoint;
    }

    private float CalculateScore(Vector3 position)
    {
        float distanceScore = Vector3.Distance(position, playerPos) * distanceWeight;
        float proximityScore = CalculateEdgeProximityScore(position) * proximityWeight;

        return distanceScore + proximityScore;
    }

    private float CalculateEdgeProximityScore(Vector3 point)
    {
        NavMeshHit hit;

        // Cast a ray from the point downwards to find the nearest point on the NavMesh
        if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas))
        {
            // Calculate the distance from the point to the nearest point on the NavMesh
            float distanceToNavMeshEdge = Vector3.Distance(point, hit.position);
            float edgeProximityScore = 1.0f - Mathf.Clamp01(distanceToNavMeshEdge);
            return edgeProximityScore;
        }

        // Point is not on the NavMesh, return a low score
        return 0.0f;
    }

    /*private float CalculateObstacleProximityScore(Vector3 point)
    {
        const float maxProximityDistance = 5.0f;  // Adjust this distance based on your needs

        // Cast rays in multiple directions and measure the distance to nearby obstacles
        int numRays = 8;  // You can adjust the number of rays based on your needs
        float totalProximity = 0.0f;

        for (int i = 0; i < numRays; i++)
        {
            float angle = i * 360.0f / numRays;
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Ray ray = new Ray(point, rayDirection);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxProximityDistance))
            {
                // Measure the distance to the obstacle and normalize it
                float proximity = hit.distance / maxProximityDistance;
                totalProximity += proximity;
            }
        }

        // Calculate the average proximity and normalize it
        float averageProximity = totalProximity / numRays;
        return averageProximity;
    }*/

    /*private int GetBestScoreIndex(float[] scores)
    {
        int bestIndex = 0;
        float bestScore = scores[0];
        for (int i = 1; i < scores.Length; i++)
        {
            if (scores[i] > bestScore)
            {
                bestScore = scores[i];
                bestIndex = i;
            }
        }
        return bestIndex;
    }*/


    /*private void SampleNavmeshPoints()
    {
        Vector3 playerToAgent = transform.position - playerPos;
        float angleRange = 180.0f; // Adjust this angle based on your design
        int numSamples = 8; // Adjust the number of samples based on your needs
        float samplingRadius = 5.0f; // Adjust the radius based on your needs

        for (int i = 0; i < numSamples; i++)
        {
            float angle = (i / (float)(numSamples - 1)) * angleRange - angleRange / 2.0f;
            Vector3 rotatedDirection = Quaternion.Euler(0, angle, 0) * playerToAgent;
            Vector3 samplePosition = transform.position + rotatedDirection.normalized * samplingRadius;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(samplePosition, out hit, samplingRadius, NavMesh.AllAreas))
            {
                // This is a valid position to backtrack to
                // You can store the valid positions and choose the best one
                Debug.DrawLine(transform.position, hit.position, Color.green, 2.0f);
            }
            else
            {
                Debug.DrawLine(transform.position, samplePosition, Color.red, 2.0f);
            }
        }
    }*/


    private void Backtracking()
    {

        /*Vector3 playerDirection = playerPos - transform.position;
        Vector3 playerDirectionFlat = new Vector3(playerDirection.x, 0, playerDirection.z).normalized;
        Vector3 backtrackingDirection = -playerDirectionFlat;
        float distToPlayer = playerDirection.magnitude;


        RaycastHit hit;
        if (Physics.Raycast(transform.position, -backtrackingDirection, out hit, 2.5f))
        {
            // Obstacle detected, calculate sidetracking direction
            Vector3 sidetrackDirection = Vector3.Cross(Vector3.up, backtrackingDirection);
            Vector3 sidetrackPosition = transform.position + sidetrackDirection * _agent.stoppingDistance;
            //newPosition = transform.position + sidetrackDirection * (_agent.stoppingDistance - distanceToPlayer);

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(sidetrackPosition, out navHit, 5f, NavMesh.AllAreas))
            {
                // Set the agent's destination to the new sidetrack position
                newPosition = navHit.position;
                _agent.SetDestination(navHit.position);
            }
            // Move the agent towards the new position while keeping it on the NavMesh
            //_agent.Move((newPosition - transform.position) * Time.deltaTime);
        }
        else
        {
            // Calculate backtracking direction
            //Vector3 backtrackingDirection = playerToAgent.normalized;
            Debug.LogWarning("This is what should be called!!!!");
            // Calculate the new position where the agent should move to
            newPosition = closestPointOnNavMesh + backtrackingDirection * (3);
            _animDestination = newPosition;
            _agent.SetDestination(newPosition);

            // Move the agent towards the new position while keeping it on the NavMesh
            //_agent.Move((newPosition - transform.position) * Time.deltaTime);
        }
        GameObject e = Instantiate(_cube, newPosition, _cube.transform.rotation);

        _agent.stoppingDistance = 0.0f;




        //Vector3 _distanceToTarget = Vector3.Distance(_agent.destination, transform.position);
        //_animDestination = _agent.destination;
        Vector3 targetDirection = newDestination - closestPointOnNavMesh;
        float distToTarget = targetDirection.magnitude;

        if (distToTarget <= (_agent.stoppingDistance + 1f))
        {
            destination = tempDestination;
            _agent.SetDestination(playerPos);
            isBackTracking = false;
            _agent.stoppingDistance = 5;
        }*/

        // Calculate the backtracking direction
        Vector3 playerToAgent = transform.position - playerPos;
        Vector3 backtrackingDirection = playerToAgent.normalized;

        if (!isSidetracking)
        {
            // Check for obstacles in the backtracking direction
            if (Physics.Raycast(transform.position, -backtrackingDirection, out RaycastHit hit, 2.5f))
            {
                // Obstacle detected, calculate sidetracking direction
                Vector3 sidetrackDirection = Vector3.Cross(Vector3.up, backtrackingDirection);
                sidetrackPosition = transform.position + sidetrackDirection * _agent.stoppingDistance;

                // Set sidetracking flag
                isSidetracking = true;
            }
        }

        if (isSidetracking)
        {
            // Move towards the sidetrack position
            _agent.stoppingDistance = 0.0f;
            _agent.SetDestination(sidetrackPosition);

            // Check if the agent has reached the sidetrack position
            float distToSidetrack = Vector3.Distance(transform.position, sidetrackPosition);
            if (distToSidetrack <= (_agent.stoppingDistance + 0.5f))
            {
                // Sidetracking completed, reset sidetracking flag and proceed to the backtracking position
                isSidetracking = false;
                _agent.stoppingDistance = 0.0f;
                _agent.SetDestination(transform.position);
            }
        }
        else
        {

            if (backtrackingStartPosition == Vector3.zero)
            {
                // Calculate the new position where the agent should move to
                backtrackingStartPosition = transform.position;
                newPosition = transform.position + backtrackingDirection * (3);
                _animDestination = newPosition;
                //GameObject e = Instantiate(_cube, newPosition, _cube.transform.rotation);
            }
            // Set the agent's destination to the new position
            _agent.stoppingDistance = 0.0f;
            _agent.SetDestination(newPosition);

            // Check if the agent has reached the new destination
            float distToNewDestination = Vector3.Distance(transform.position, newPosition);
            if (distToNewDestination <= (_agent.stoppingDistance + 0.1f))
            {
                
                isBackTracking = false; // Reset backtracking flag
                _agent.stoppingDistance = _tempStopDistance; // Reset stopping distance
                destination = tempDestination; // Reset destination
                backtrackingStartPosition = Vector3.zero;
            }
        }


    }

    private void UpdateAgentDestination()
    {
        



        Vector3 playerDirection = playerPos - transform.position;
        float distToPlayer = playerDirection.magnitude;

        /*Vector3 playerDirection = playerPos - transform.position;
        Vector3 playerDirectionFlat = new Vector3(playerDirection.x, 0, playerDirection.z).normalized;
        Vector3 backtrackingDirection = -playerDirectionFlat;
        float distToPlayer = playerDirection.magnitude;*/

        //Debug.LogWarning(isBackTracking + ": Testing");


        if (_agent.destination == playerPos)
        {
            Debug.LogWarning("Destination is PlayerPos");
        }

        if (_agent.destination != playerPos)
        {
            Debug.LogWarning("Destination is NOT!!!! PlayerPos");
        }

        


        // From here
        /*if (!isBackTracking)
        {
            if (_agent.enabled)
            {
                bool _checkValidPath = CheckPathValidity(closestPointOnNavMesh, playerPos);
                if (_checkValidPath && distToPlayer > _agent.stoppingDistance)
                {
                    _agent.stoppingDistance = 5;
                    destination = tempDestination;
                    _agent.SetDestination(playerPos);
                }
                else
                {
                    _agent.stoppingDistance = 0;
                    destination = transform;
                    _agent.SetDestination(transform.position);
                }
            }
        }*/
        // To here




        /*if (distToPlayer <= (_tempStopDistance - 1.0f) && !isBAckTracking)
        {
            //if (!isBAckTracking)
            //{
                isBAckTracking = true;



                for (int i = 0; i < 5; i++)
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    Vector3 randomPoint = closestPointOnNavMesh + new Vector3(randomDirection.x, 0f, randomDirection.y) * 5f;

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
                    {
                        NavMeshPath escapePath = new NavMeshPath();
                        if (NavMesh.CalculatePath(closestPointOnNavMesh, hit.position, NavMesh.AllAreas, escapePath))
                        {
                            escapeDirection = escapePath.corners[1] - closestPointOnNavMesh;
                            if (escapeDirection.magnitude > 0)
                            {
                                newDestination = closestPointOnNavMesh + escapeDirection;
                                _animDestination = newDestination;
                                _agent.stoppingDistance = 0.0f;
                                _agent.SetDestination(newDestination);
                            }
                        }
                        break;
                    }
                }

                //if (escapeDirection != Vector3.zero)
                //{
                    //newDestination = transform.position + escapeDirection;
                    *//* Transform tempTransform = tempDestination; // Create a temporary Transform
                     tempTransform.position = newDestination; // Update the position of the temporary Transform
                     destination = tempTransform;*//*
                    //_animDestination = newDestination;
                    //_agent.stoppingDistance = 0.0f;
                    //_agent.SetDestination(newDestination);

                    

                    
                //}
            //}
        }
        Vector3 newDestDirection = newDestination - transform.position;
        distToDestination = newDestDirection.magnitude;*/

        //Here  13th August 2023
        /*if (distToPlayer <= (_tempStopDistance - 1.0f) && !isBackTracking)
        {
            isBackTracking = true;

            //for (int i = 0; i < 5; i++)
            //{
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector3 randomPoint = closestPointOnNavMesh + new Vector3(randomDirection.x, 0f, randomDirection.y) * 5f;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
                {
                    NavMeshPath escapePath = new NavMeshPath();
                    if (NavMesh.CalculatePath(closestPointOnNavMesh, hit.position, NavMesh.AllAreas, escapePath))
                    {
                        escapeDirection = escapePath.corners[1] - closestPointOnNavMesh;
                        if (escapeDirection.magnitude > 0)
                        {
                            newDestination = closestPointOnNavMesh + escapeDirection;
                            _animDestination = newDestination;
                            _agent.stoppingDistance = 0.0f;
                            //_agent.SetDestination(newDestination);
                        }
                    }
                    //break;
                }
            //}
        }*/
        // TO Here
        Vector3 playerToAgent = transform.position - playerPos;
        float distanceToPlayer = playerToAgent.magnitude;

       
        /*if (distanceToPlayer <= (_tempStopDistance - 1.0f))
        {

            isBackTracking = true;

        }*/


        if (_agent.destination == playerPos)
        {
            bool _checkValidPath = CheckPathValidity(transform.position, playerPos);
            if (_checkValidPath && distToPlayer > _agent.stoppingDistance)
            {
                //_agent.stoppingDistance = 5;
                destination = tempDestination;

                //ShouldDisableAgent();
                StartCoroutine(SetDestination(playerPos,null, 5.0f));
                //SetAgentDestination(playerPos, null);
                //_agent.SetDestination(playerPos);
            }
            else
            {
                //_agent.stoppingDistance = 0;
                destination = transform;

                if (_agent.enabled)
                {
                    //ShouldDisableAgent();
                    StartCoroutine(SetDestination(transform.position, null, 0.0f));
                    //SetAgentDestination(transform.position, null);
                }
                //_agent.SetDestination(transform.position);
            }
        }

        if (_agent.destination != playerPos)
        {
            bool _checkValidPaths = CheckPathValidity(transform.position, playerPos);
            if (_checkValidPaths)
            {
                //_agent.stoppingDistance = 5;
                destination = tempDestination;

                //ShouldDisableAgent();
                StartCoroutine(SetDestination(playerPos, null, 5.0f));
                //SetAgentDestination(playerPos, null);
            }
            //_agent.SetDestination(playerPos);
        }

        if (distanceToPlayer <= (_tempStopDistance - 1.0f))
        {
            StartBacktracking();
        }


           /* //_agent.stoppingDistance = 5;
            //Vector3 backtrackingDirection = playerToAgent.normalized;
            //Vector3 backtrackingDirection = transform.position - new Vector3(0,0,1f);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -backtrackingDirection, out hit, 2.5f))
            {
                // Obstacle detected, calculate sidetracking direction
                Vector3 sidetrackDirection = Vector3.Cross(Vector3.up, backtrackingDirection);
                Vector3 sidetrackPosition = transform.position + sidetrackDirection * _agent.stoppingDistance;
                //newPosition = transform.position + sidetrackDirection * (_agent.stoppingDistance - distanceToPlayer);

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(sidetrackPosition, out navHit, 5f, NavMesh.AllAreas))
                {
                    // Set the agent's destination to the new sidetrack position
                    newPosition = navHit.position;
                    _agent.SetDestination(navHit.position);
                }
                // Move the agent towards the new position while keeping it on the NavMesh
                //_agent.Move((newPosition - transform.position) * Time.deltaTime);
            }
            else
            {
                // Calculate backtracking direction
                //Vector3 backtrackingDirection = playerToAgent.normalized;
                Debug.LogWarning("This is what should be called!!!!");
                // Calculate the new position where the agent should move to
                newPosition = closestPointOnNavMesh + backtrackingDirection * (3);
                _animDestination = newPosition;
                _agent.SetDestination(newPosition);

                // Move the agent towards the new position while keeping it on the NavMesh
                //_agent.Move((newPosition - transform.position) * Time.deltaTime);
            }
            GameObject e = Instantiate(_cube, newPosition, _cube.transform.rotation);

            _agent.stoppingDistance = 0.0f;

            
            //_agent.SetDestination(newPosition);

            *//*Vector3 destToAgent = transform.position - newPosition;
            float distanceToDest = destToAgent.magnitude;

            if (distanceToDest <= 0.5f)
            {
                _agent.enabled = false;
                isBackTracking = false;
            }*/

            /*Vector3 newDestDirection = newDestination - closestPointOnNavMesh;
            distToDestination = newDestDirection.magnitude;

            // Check if the agent has reached the newDestinaton
            if (distToDestination <= (_agent.stoppingDistance + stoppingDistanceBuffer))
            {
                isBackTracking = false; // Reset backtracking flag
                _agent.stoppingDistance = _tempStopDistance; // Reset stopping distance
                destination = tempDestination; // Reset destination

            }
            else
            {
                // Continue moving towards the newDestination
                _agent.SetDestination(newDestination);
            }*//*
            
        }

        if (isBackTracking)
        {
            //Vector3 _distanceToTarget = Vector3.Distance(_agent.destination, transform.position);
            //_animDestination = _agent.destination;
            Vector3 targetDirection = newDestination - closestPointOnNavMesh;
            float distToTarget = targetDirection.magnitude;

            if (distToTarget <= (_agent.stoppingDistance + 1f))
            {
                destination = tempDestination;
                _agent.SetDestination(playerPos);
                isBackTracking = false;
                _agent.stoppingDistance = 5;
            }
        }

*/




        /* else
         {
             if (_agent.destination != playerPos)
             {
                 _agent.stoppingDistance = _tempStopDistance;
                 _agent.SetDestination(playerPos);
             }
         }*/




        /*if (distToPlayer < (_tempStopDistance - 1.0f))
        {
            isBAckTracking = true;

            Vector3 backtrackDirection = -playerDirection.normalized;
            Vector3 newDestination = transform.position + backtrackDirection * (_agent.stoppingDistance + stoppingDistanceBuffer);
            Transform tempTransform = destination; // Create a temporary Transform
            tempTransform.position = newDestination; // Update the position of the temporary Transform
            destination = tempTransform;

            _agent.stoppingDistance = 0.0f;
            _agent.SetDestination(newDestination);

            // Update agent's speed and animation as needed
        }
        else
        {
            
            if (_agent.destination != playerPos)
            {
                
                // Player is outside stopping distance, move normally
                _agent.SetDestination(playerPos);
                
            }
            if (distToPlayer >= _tempStopDistance)
            {
                _agent.stoppingDistance = _tempStopDistance;
                isBAckTracking = false;
                destination = tempDestination;
            }
            // Update agent's speed and animation as needed
        }*/
    }

    private void ShouldDisableAgent()
    {
        //_obstacle.carving = !_animation.GetShouldDisable();
        //_agent.enabled = _animation.GetShouldDisable();
        _carve = _agent.enabled;
    }

    public Vector3 GetAnimDestination()
    {
        return _animDestination;
    }

    public Transform GetDestination()
    {
        return destination;
    }

    public void DisableAgent()
    {
        _agent.enabled = false;
    }

    public void EnableAgent()
    {
        _agent.enabled = true;
    }

    private Vector3 GetClosestPointOnNavMesh(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerDirection = playerPos - transform.position;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 4f);
    }

    private bool CheckPathValidity(Vector3 _startPoint, Vector3 _endPoint)
    {
        //NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(_startPoint, _endPoint, NavMesh.AllAreas, _path);

        NavMeshPathStatus pathStatus = _path.status;
        if (pathStatus != NavMeshPathStatus.PathComplete)
        {
            
            Debug.LogWarning("Path Status: " + pathStatus);
            return false;
        }

        return hasPath && _path.corners.Length > 0; ;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void LateUpdateState()
    {
        throw new System.NotImplementedException();
    }
}
