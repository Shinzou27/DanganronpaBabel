using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class CanvasEnabler : MonoBehaviour
{
    private TrackedDeviceGraphicRaycaster graphicRaycaster;
    // Start is called before the first frame update
    void Start()
    {
        graphicRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>();
        Singleton.Instance.OnDormRoomEntered += HandleInteraction;
        Singleton.Instance.OnWrongDormRoom += HandleInteraction;
        Singleton.Instance.OnDormRoomExited += Disable;
        Disable();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    private void Disable(object sender = null, Singleton.DormRoomEventArgs e = null)
    {
        graphicRaycaster.enabled = false;
    }
    private void HandleInteraction(object sender, Singleton.DormRoomEventArgs e)
    {
        DormRoom dormRoom = GetComponentInParent<DormRoom>();
        if (e.dormRoom == dormRoom) {
            graphicRaycaster.enabled = true;
        } else {
            graphicRaycaster.enabled = false;
        }
    }
}
