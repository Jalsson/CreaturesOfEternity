using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform target;
    
    public Vector3 offset;
    public Vector3 offset2;

    private Vector3 cameralocation1;
    private Vector3 cameralocation2;

    public float pitch = 2f;
    public float currentZoom = 10f;

    public float yawSpeed = 100f;
    private float yawInput = 0f;
    private float vertispeed = 2f;
    private bool combatMode;
    

    private void Start()
    {
        target = PlayerManager.S_INSTANCE.player.transform;
        transform.RotateAround(target.position, Vector3.up, yawInput);
        cameralocation1 = new Vector3(offset.x, offset.y, offset.z);
        cameralocation2 = new Vector3(offset2.x, offset2.y, offset2.z);
        combatMode = false;
    }
    void FixedUpdate()
    {
       
        //yawInput += Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.X))
            combatMode = !combatMode;
        
        if (!combatMode)
        {
           /* offset = new Vector3(0, -0.6f, 0.5f);
            offset2 = new Vector3(0, -0.6f, 0);*/
            
        }
        else
        {
            offset = cameralocation1;
            offset2 = cameralocation2;
            currentZoom = 2.4f;
        }

        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + offset2 * pitch);
        transform.RotateAround(target.position, Vector3.up, yawInput);
    }

}
