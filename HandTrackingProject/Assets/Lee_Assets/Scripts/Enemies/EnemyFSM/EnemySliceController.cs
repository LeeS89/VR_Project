using UnityEngine;
using EzySlice;


public class EnemySliceController : MonoBehaviour
{
   
    [SerializeField] Material sliceMaterial;
    public float cutForce = 2000f;
    [SerializeField] public AudioSource _deflect;


    [Header ("Object to slice - This game object")]
    [SerializeField] private GameObject _targetObj;

    [Header ("Direction of the slice")]
    private Transform _slicePlane;
    
    
    public bool _shouldSlice = false;
    private DeathState _deathState;
    private StateController _stateController;

    private void Awake()
    {
        _stateController = GetComponent<StateController>();
        _deathState = GetComponent<DeathState>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (_shouldSlice)
        {
            GameObject sliceMesh = EnemySlicePool.SharedInstance.GetPooledObject();
            if (sliceMesh != null)
            {

                sliceMesh.SetActive(true);
                sliceMesh.transform.position = gameObject.transform.position;
                sliceMesh.transform.rotation = gameObject.transform.rotation;
                _slicePlane = sliceMesh.transform.GetChild(0);

                
                
                //_targetObj = sliceMesh;

                Slice(sliceMesh);
                _deflect.Play();

                _shouldSlice = false;

            }
            
            

        }

    }

    public void InstantDeath()
    {
        SliceSetup();
    }

    private void SliceSetup()
    {
        GameObject sliceMesh = EnemySlicePool.SharedInstance.GetPooledObject();
        if (sliceMesh != null)
        {

            sliceMesh.SetActive(true);
            sliceMesh.transform.position = gameObject.transform.position;
            sliceMesh.transform.rotation = gameObject.transform.rotation;
            _slicePlane = sliceMesh.transform.GetChild(0);


            Slice(sliceMesh);
            _deflect.Play();

            

        }
    }

    
    /// <summary>
    /// When called, this method separates the target (which is a separate gameobject with the same mesh of the object to slice,
    /// is spawned from a pool to the position of the intended object to slice) 
    /// the original game object is disabled and the pooled object is split
    /// into 2 halves depending on where the plane is positioned
    /// 
    /// Then adds an explosion force for seperation
    /// </summary>
    /// <param name="target"></param>
    public void Slice(GameObject target)
    {
        

        SlicedHull hull = target.Slice(_slicePlane.position, _slicePlane.up);
        //SlicedHull hull2 = target.Slice(planeDebug2.position, planeDebug2.up);
        

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, sliceMaterial);
            SetUpSLicedComponent(upperHull);
            upperHull.transform.position = target.transform.position;
            upperHull.transform.rotation = target.transform.rotation;

            GameObject lowerHull = hull.CreateLowerHull(target, sliceMaterial);
            SetUpSLicedComponent(lowerHull);
            lowerHull.transform.position = target.transform.position;
            lowerHull.transform.rotation = target.transform.rotation;

            Destroy(lowerHull, 3.0f);
            Destroy(upperHull, 3.0f);

        }
        target.SetActive(false);

        _deathState.deathType = DeathState.DeathType.Sliced;
        //_deathState.SelectDeathType(_deathState.GetType());
        //BaseState newState = new DeathState();
        _stateController.RequestStateChange(_deathState);
        //gameObject.SetActive(false);
        
        /* if (hull2 != null)
         {
             GameObject upperHull = hull2.CreateUpperHull(target, sliceMaterial);
             SetUpSLicedComponent(upperHull);

             GameObject lowerHull = hull2.CreateLowerHull(target, sliceMaterial);
             SetUpSLicedComponent(lowerHull);

             //Destroy(target);
             //StartCoroutine(Explosion2(upperHull,lowerHull));
             Destroy(lowerHull, 3.0f);
             Destroy(upperHull, 3.0f);
         }*/
    }

    public void SetUpSLicedComponent(GameObject slicedTarget)
    {
        Rigidbody rb = slicedTarget.AddComponent<Rigidbody>();
        MeshCollider collider = slicedTarget.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedTarget.transform.position, 1);


    }

    

  /*  private void OnTriggerEnter(Collider other)
    {
        *//*if (other.tag == "Agent")
        {
            //skinnedMeshRenderer.enabled = false;
            //_gun.enabled = false;
            //Slice(_targetObj);
            _deflect.Play();
        }*/

       /* if (other.name == "Robot_Soldier_Body")
        {
            //Slice(_targetObj);
            _deflect.Play();
        }*/

/*
        if (other.tag == "Blade")
        {
            //ChangeColor();
            *//*Destroy(_bulletSpawn);
            Slice(target);*//*
            //ChangeColor();
            //_deflect.Play();
            SliceSetup();
            //Destroy(other.transform.parent.gameObject);
        }*//*

    }*/
}
