using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVelocityControl : MonoBehaviour {

   
    private Vector3 CameraVector;
    private Vector3 endVector;

    [SerializeField]
    private float speed = 1;
    Transform debugTransform;
    private GameObject playerCamera;

    private float oldEulerAngle;
    private Rigidbody rb;

    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<CameraController>().gameObject;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        Vector2 inputVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        // getting camera forward and right vector eliminating the y axis by making it 0
        Vector3 camF = new Vector3(playerCamera.transform.forward.x , 0 , playerCamera.transform.forward.z).normalized;
        Vector3 camR = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;

        Debug.DrawLine(gameObject.transform.position, ((camF * inputVector.y + camR * inputVector.x) * speed) + gameObject.transform.position , Color.green);
        Debug.DrawLine(gameObject.transform.position, transform.forward * 1 + gameObject.transform.position, Color.green);

        //setting turning target location
        Vector3 targetLocation = (camF * inputVector.y + camR * inputVector.x);

        targetLocation = transform.InverseTransformDirection(targetLocation);

        // Amount that there is left to turning a character to desired angle. If negative character is turning left. Positive when turning right
        float turnAmount = Mathf.Atan2(targetLocation.x, targetLocation.z);
        float forwardAmount = inputVector.y;
        

        if (inputVector.x != 0 || inputVector.y != 0)
        {
            var targetRotation = Quaternion.LookRotation((camF * inputVector.y + camR * inputVector.x));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed / 100);
            
        }

    }
}
