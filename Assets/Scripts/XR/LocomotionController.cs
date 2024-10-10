using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;

    public XRRayInteractor leftRayInteractor;
    public XRRayInteractor rightRayInteractor;
    public XRRayInteractor leftRayTeleport;
    public XRRayInteractor rightRayTeleport;

    public bool EnableLeftTeleport {get; set;} = true;
    public bool EnableRightTeleport {get; set;} = true;
    private Vector3 initialSpawnPosition = new(-54, 0, -52);
    private Vector3 gameOverSpawnPosition = new(-58, 0, -52);
    private Vector3 startSpawnPosition = new(40, 0, 0);
    private Vector3 trialSpawnPosition = new(90.625f, 0, 2.78f);
    private void Start() {
        transform.position = initialSpawnPosition;
        transform.Rotate(new(0, 90, 0));
        // transform.position = trialSpawnPosition;
        Singleton.Instance.OnTrialEnter += SetTrialPosition;
        Singleton.Instance.OnStartGame += SetStartPosition;
        Singleton.Instance.OnGameOver += SetInitialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        leftRayTeleport.enabled = !(Singleton.Instance.onDialogue || Singleton.Instance.onTrial || GameFlow.Instance.inHouse);
        rightRayTeleport.enabled = !(Singleton.Instance.onDialogue || Singleton.Instance.onTrial || GameFlow.Instance.inHouse);
        GetComponent<DeviceBasedSnapTurnProvider>().enabled = !Singleton.Instance.onWheelActive;
        if(leftTeleportRay)
        {
            bool isHovering = leftRayInteractor.TryGetHitInfo(out Vector3 pos, out Vector3 norm, out int index, out bool validTarget);
            leftTeleportRay.gameObject.SetActive(EnableLeftTeleport && !isHovering && CheckIfActivated(leftTeleportRay));
        }

        if (rightTeleportRay)
        {
            bool isHovering = rightRayInteractor.TryGetHitInfo(out Vector3 pos, out Vector3 norm, out int index, out bool validTarget);
            rightTeleportRay.gameObject.SetActive(EnableRightTeleport && !isHovering && CheckIfActivated(rightTeleportRay));
        }

    }


    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
    private void SetInitialPosition(object sender, EventArgs e) {
        transform.position = gameOverSpawnPosition;
        transform.Rotate(new(0, -90, 0));
    }
    private void SetStartPosition(object sender, EventArgs e) {
        transform.position = startSpawnPosition;
    }
    private void SetTrialPosition(object sender, EventArgs e) {
        transform.position = trialSpawnPosition;
    }
}