using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabler : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MeshRenderer>())
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MeshRenderer>())
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
