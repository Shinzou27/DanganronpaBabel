using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDebugger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.collider.gameObject.name);
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
    }
}
