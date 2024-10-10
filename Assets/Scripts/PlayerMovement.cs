using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Transform orientation;
    private CharacterController controller;
    private bool prevInteractUIState = false;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Input.GetAxis("Horizontal") * orientation.right + Input.GetAxis("Vertical") * orientation.forward;
        if (Input.GetKey(KeyCode.LeftShift)) {
            controller.Move(1.5f * speed * Time.deltaTime * movement);
        } else {
            controller.Move(speed * Time.deltaTime * movement);
        }
        bool isCloseToGameModeFrame = Physics.Raycast(transform.position, Vector3.forward, out hit, 50f);
        if (isCloseToGameModeFrame != prevInteractUIState) {
            prevInteractUIState = !prevInteractUIState;
        }
    }
}
