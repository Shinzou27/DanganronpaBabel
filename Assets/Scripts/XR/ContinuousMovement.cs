using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.XR.ARFoundation;
using System;


public class ContinuousMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float speed = 1;
    public float additionalHeight;
    public Transform cameraOffset;

    private XROrigin rig;
    private Vector2 inputAxis;
    private CharacterController character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();
    }



    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }
    
    private void CapsuleFollowHeadset() {
        character.height = rig.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.gameObject.transform.position);
        // character.center = new(0, character.height/2 + character.skinWidth, 0);
        character.center = new(capsuleCenter.x, character.height/2 + character.skinWidth, capsuleCenter.z);
        cameraOffset.localPosition = new(0, additionalHeight, 0);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        Quaternion headYaw = Quaternion.Euler(0, rig.Camera.transform.eulerAngles.y, 0);
        if (!(Singleton.Instance.onDialogue || Singleton.Instance.onTrial)) {
            Vector3 direction = headYaw *  new Vector3(inputAxis.x, 0, inputAxis.y);
            RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, 1f);
            RaycastHit[] floorHits = Physics.RaycastAll(rig.Camera.transform.position, Vector3.down, 5f);
            if (hits.Length > 0) {
                GameFlow.Instance.inHouse = LetTheBodiesHitTheFloor(hits);
            }
            Vector3 nextPosition = transform.position + speed * Time.fixedDeltaTime * direction;
            if (NotWalkingOnWater(nextPosition)) {
                character.Move(speed * Time.fixedDeltaTime * direction);
            }
        }
        transform.position = new(transform.position.x, 0, transform.position.z);
    }
    private void OnTriggerEnter(Collider other) {
        // if (other.gameObject.name == "Door Frame") {
        //     DormRoom dormRoom = other.GetComponentInParent<DormRoom>();
        //     bool isLocked = dormRoom.GetLockedState();
        //     if (dormRoom != null && openedDoor) {
        //         openedDoor = false;
        //     // if (dormRoom != null && !isLocked) {
        //         // GameFlow.Instance.ToggleInHouse();
        //         if (GameFlow.Instance.inHouse) {
        //             Debug.Log("Entrei num quarto");
        //             GameFlow.Instance.lastEnteredRoom = dormRoom;
        //             Singleton.Instance.OnDormRoomEntered?.Invoke(this, new Singleton.DormRoomEventArgs {
        //                 dormRoom = dormRoom
        //             });
        //         } else {
        //             Debug.Log("Saí de um quarto");
        //             Singleton.Instance.OnDormRoomExited?.Invoke(this, new Singleton.DormRoomEventArgs {
        //                 dormRoom = dormRoom
        //             });
        //         }
        //     }
        // }
    }

    private bool LetTheBodiesHitTheFloor(RaycastHit[] hits) {
        foreach (RaycastHit hit in hits) {
            if (hit.collider.gameObject.name == "Floor") {
                if (!GameFlow.Instance.inHouse) {
                    DormRoom dormRoom = hit.collider.gameObject.GetComponentInParent<DormRoom>();
                    GameFlow.Instance.lastEnteredRoom = dormRoom;
                    Singleton.Instance.OnDormRoomEntered?.Invoke(this, new Singleton.DormRoomEventArgs {
                        dormRoom = dormRoom
                    });
                }
                return true;
            }
        }
        if (GameFlow.Instance.inHouse) {
            Singleton.Instance.OnDormRoomExited?.Invoke(this, new Singleton.DormRoomEventArgs {
                dormRoom = GameFlow.Instance.lastEnteredRoom
            });
        }
        return false;
    }
    private bool NotWalkingOnWater(Vector3 nextPosition) {
        RaycastHit[] hits = Physics.RaycastAll(nextPosition, Vector3.down, 1.0f);
        List<string> collisions = new();
        foreach (RaycastHit hit in hits) {
            string name = hit.collider.gameObject.name;
            collisions.Add(name);
        }
        if (collisions.Contains("Water") || collisions.Contains("Pool")) {
            if (collisions.Contains("VerticalWood") || collisions.Contains("HorizontalWoods") || collisions.Contains("DormRoomPlataforms") || collisions.Contains("Floor")) {
                return true;
            }
            return false;
        } else {
            return true;
        }
    }
}