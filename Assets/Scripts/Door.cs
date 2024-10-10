using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] HingeJoint knobJoint;
    [SerializeField] Collider knobCollider;
    [SerializeField] Collider blockCollider;
    [SerializeField] Animator doorAnimator;
    [SerializeField] Rigidbody knobRigidbody;
    [SerializeField] DormRoom dormRoom;
    public bool isBathroomDoor;
    private bool isOpen;
    private bool beingGrabbed;
    private bool reachedMaxLimit;
    private float openDoorLimit = 5f;
    private float openDoorCountdown = 0;
    private float knobAngle;
    // Start is called before the first frame update
    void Start()
    {
        Singleton.Instance.OnDormRoomEntered += AnimateDoorClose;
        Singleton.Instance.OnDormRoomExited += AnimateDoorClose;
        Singleton.Instance.OnWrongDormRoomExited += AnimateDoorCloseFromOutdoor;
        Singleton.Instance.OnLeavingFreeTime += DisableKnob;
    }


    // Update is called once per frame
    void Update()
    {
        knobAngle = Mathf.Abs(knobJoint.gameObject.transform.eulerAngles.x);
        if (isOpen && !dormRoom.GetLockedState()) {
            openDoorCountdown += Time.deltaTime;
            if (openDoorCountdown > openDoorLimit) {
                doorAnimator.SetBool("Open", false);
                isOpen = false;
                AudioManager.Instance.PlayDoorClose();
                EnableKnob();
            }
        } else {
            openDoorCountdown = 0;
        }
        if (knobAngle < 35 && !isOpen)
        {
            reachedMaxLimit = true;
        }
        if (knobAngle > 85 && reachedMaxLimit && !isOpen && !beingGrabbed)
        {
            if (!isBathroomDoor) {
                bool isLocked = dormRoom.GetLockedState();
                blockCollider.enabled = isLocked;
                if (isLocked) {
                    //player n deveria abrir ESSA porta, então há o evento de porta indesejada
                    GameFlow.Instance.AddToVisit(dormRoom.GetCharacter());
                    Singleton.Instance.OnWrongDormRoom?.Invoke(this, new Singleton.DormRoomEventArgs {
                        dormRoom = dormRoom
                    });
                }
            } else {
                blockCollider.enabled = false;
            }
            reachedMaxLimit = false;
            doorAnimator.SetBool("Open", true);
            AudioManager.Instance.PlayDoorOpen();
            DisableKnob();
            isOpen = true;
            if (!isBathroomDoor) {
                dormRoom.SetLocked(false);
            }
        }
    }
    private void AnimateDoorClose(object sender, Singleton.DormRoomEventArgs e)
    {
        if (e.dormRoom == dormRoom && GameFlow.Instance.inHouse) {
            GameFlow.Instance.AddToVisit(dormRoom.GetCharacter());
            doorAnimator.SetBool("Open", false);
            isOpen = false;
            AudioManager.Instance.PlayDoorClose();
            EnableKnob();
        }
    }
    private void AnimateDoorCloseFromOutdoor(object sender, Singleton.DormRoomEventArgs e)
    {
        if (e.dormRoom == dormRoom && !GameFlow.Instance.inHouse) {
            GameFlow.Instance.AddToVisit(dormRoom.GetCharacter());
            doorAnimator.SetBool("Open", false);
            isOpen = false;
            AudioManager.Instance.PlayDoorClose();
            EnableKnob();
        }
    }

    private void DisableKnob(object sender = null, EventArgs e = null)
    {
        if (!isBathroomDoor && dormRoom.GetCharacter().characterIndex != 5) {
            knobCollider.enabled = false;
        }
    }
    private void EnableKnob(object sender = null, EventArgs e = null)
    {
        knobCollider.enabled = true;
    }
    public void SetGrabState(bool state) {
        beingGrabbed = state;
    }
}
