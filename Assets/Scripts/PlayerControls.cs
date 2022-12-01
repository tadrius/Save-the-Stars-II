using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] InputAction movement;
    [SerializeField] InputAction shoot;

    private static string Horizontal = "Horizontal";
    private static string Vertical = "Vertical";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable() {
        movement.Enable();
        shoot.Enable();
    }

    void OnDisable() {
        movement.Disable();
        shoot.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // USING NEWER INPUT SYSTEM
        Vector2 movementValue = movement.ReadValue<Vector2>();
        float horizontalAxis = movementValue.x;
        float verticalAxis = movementValue.y;

        // USING OLDER INPUT MANAGER
        // float horizontalAxis = Input.GetAxis(Horizontal);
        // float verticalAxis = Input.GetAxis(Vertical);

        Debug.Log("Horizontal Axis: " + horizontalAxis);
        Debug.Log("Vertical Axis: " + verticalAxis);
    }
}
