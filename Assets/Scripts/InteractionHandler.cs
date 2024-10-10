using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        other.gameObject.TryGetComponent(out BuffObjectTagger component);
        if (component != null && component.GetBuffObject() != BuffObject.Handbook) {
            GameFlow.Instance.SetHoldingObject(component.GetBuffObject());
        }
    }
    private void OnTriggerExit(Collider other) {
        other.gameObject.TryGetComponent(out BuffObjectTagger component);
        if (component != null) {
            // GameFlow.Instance.ReleaseObject();
        }
    }
}
