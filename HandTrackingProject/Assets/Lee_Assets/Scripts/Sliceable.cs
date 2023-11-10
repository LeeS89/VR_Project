using UnityEngine;
using EzySlice;
using System.Collections;
using System;

public class Sliceable : MonoBehaviour
{
    public Transform planeDebug;
    //public Transform planeDebug2;
    //[SerializeField] GameObject _bulletSpawn;
    public GameObject target;
    public Material sliceMaterial;
    public float cutForce = 2000f;
    [SerializeField] public AudioSource _deflect;

    //public SkinnedMeshRenderer skinnedMeshRenderer;
    //public MeshRenderer meshRenderer;

    public bool sliceTest = false;
    public GameObject slicedParent;


    [SerializeField] GameObject _targetObj;
    public GameObject _parent;
    [SerializeField] Transform _slicePlane;
    //[SerializeField] SkinnedMeshRenderer _gun;
    [SerializeField] GameObject _slicedMesh;
    public bool _shouldSlice = false;

    /* private float _hurtTimer = 0.0f;
     private bool _hurt = false;*/

    //[SerializeField] GameObject _explosion;

    //[SerializeField] Material[] _materials;
    //private int _index = 0;

    // Start is called before the first frame update

    


    // Update is called once per frame
    void Update()
    {
        if(sliceTest)
        {
            //GameObject o = Instantiate(slicedParent,target.transform.position,target.transform.rotation);
            slicedParent.SetActive(true);
            slicedParent.transform.position = target.transform.position;
            slicedParent.transform.rotation = target.transform.rotation;
            target = slicedParent;
            //skinnedMeshRenderer.enabled = false;
            //meshRenderer.enabled = true;
            Slice(target);
            _deflect.Play();
            sliceTest= false;
        }

        if (_shouldSlice)
        {
            _slicedMesh.SetActive(true);
            _slicedMesh.transform.position = _targetObj.transform.position;
            _slicedMesh.transform.rotation = _targetObj.transform.rotation;
            _targetObj = _slicedMesh;
            _parent.SetActive(false);
            //skinnedMeshRenderer.enabled = false;
            //_gun.enabled = false;
            //_slicedMesh.enabled = true;
            Slice(_targetObj);
            _deflect.Play();
            _shouldSlice= false;
        }

        /* if (Input.GetKey(KeyCode.Space))
         {
             Slice(target);
         }*/

        /* if (_hurt)
         {

             if (_hurtTimer > 0.0f)
             {
                 GetComponent<MeshRenderer>().material = _materials[0];
                 _hurtTimer -= Time.deltaTime;
             }
             else
             {
                 GetComponent<MeshRenderer>().material = _materials[3];
                 _hurt = false;
             }
         }*/
    }

    /*IEnumerator Explosion1(Rigidbody a)
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(_explosion, a.transform.position, a.transform.rotation);

    }

    IEnumerator Explosion2(Rigidbody a)
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(_explosion, a.transform.position, a.transform.rotation);

    }

    IEnumerator Explosion3(Rigidbody a)
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(_explosion, a.transform.position, a.transform.rotation);

    }

    IEnumerator Explosion4(Rigidbody a)
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(_explosion, a.transform.position, a.transform.rotation);

    }*/

    public void Slice(GameObject target)
    {
        //planeDebug = target.transform.GetChild(5);

       /* if (target.name != "Robot_Soldier_Red (1)")
        {
            planeDebug2 = target.transform.GetChild(1);
        }*/

        SlicedHull hull = _targetObj.Slice(planeDebug.position, planeDebug.up);
        //SlicedHull hull2 = target.Slice(planeDebug2.position, planeDebug2.up);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(_targetObj, sliceMaterial);
            SetUpSLicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(_targetObj, sliceMaterial);
            SetUpSLicedComponent(lowerHull);

            Destroy(_targetObj);
            //StartCoroutine(Explosion1(upperHull,lowerHull));
            Destroy(lowerHull, 3.0f);
            Destroy(upperHull, 3.0f);

        }
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

    /* private void ChangeColor()
     {
         *//*GetComponent<MeshRenderer>().material = _materials[_index];
         _index++;

         if (_index == _materials.Length)
         {
             Destroy(_bulletSpawn.transform.parent);
             Instantiate(_explosion,transform.position,transform.rotation);
             Slice(target);

             _index = 0;
         }*//*

         _index++;

         if (_index == 4)
         {
             Destroy(_bulletSpawn);
             Instantiate(_explosion, transform.position, transform.rotation);
             Slice(target);
         }
         else
         {
             _hurtTimer = 0.5f;
             _hurt = true;
         }

     }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Agent")
        {
            //skinnedMeshRenderer.enabled= false;
            //_gun.enabled= false;
            Slice(target);
            _deflect.Play();
        }

        if (other.name == "Robot_Soldier_Body")
        {
            Slice(target);
            _deflect.Play();
        }


        if (other.tag == "Head")
        {
            //ChangeColor();
            /*Destroy(_bulletSpawn);
            Slice(target);*/
            //ChangeColor();
            _deflect.Play();

            //Destroy(other.transform.parent.gameObject);
        }

    }
}
