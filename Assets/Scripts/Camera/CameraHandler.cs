using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Vector2 sensitivity;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerPosition;
    private Vector2 rotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = new (
            Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity.x,
            Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity.y
            );
        rotation.y += mouse.x;
        rotation.x -= mouse.y;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        // rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);
        // if (rotation.x > 0f) {
        //     transform.RotateAround(playerPosition.position, Vector3.up, rotation.y * Time.deltaTime);
        // }
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        // playerPosition.rotation = Quaternion.Euler(0, rotation.y, 0);
        orientation.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
}
