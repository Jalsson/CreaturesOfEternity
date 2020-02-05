using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalClass : MonoBehaviour {

    private GameObject crystal;
    [SerializeField]
    private Rigidbody rb;
    Camera cam;
    private float speed = 2f;
    PlayerController player;
    
    
    void Update()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            float hitdist = 0.0f;

            if (plane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                Vector3 target = targetPoint - transform.position;
                gameObject.GetComponent<Rigidbody>().velocity = target * speed;
            }
        }
            //Quaternion rotation = player.getRotation();
        //gameObject.GetComponent<Rigidbody>().velocity = rotation * speed;
    }

    void OnEnable()
    {
       


    }
    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
    }
    private void CheckAreaForCollisions()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, 1);

        foreach (Collider collider in collidersInRange)
        {
           CharacterStats gotComponent = collider.GetComponent<CharacterStats>();
            
            if (gotComponent != null)
            {
                for (int i = 0; i < 1; i++)
                {
                    FindObjectOfType<AudioManager>().Play("MonsterScream");

                    Debug.Log("making damage");
                    gotComponent.TakeDamage(100);
                }
            }
        }
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Plane")
        {
            
            for(int i = 0; i< 1; i++)
            {
                Debug.Log("Pallo osui lattiaan");
                CheckAreaForCollisions();
                
                
            }
            
        }
    }
    
}
